/*
* Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
*
* Licensed under the Apache License, Version 2.0 (the "License").
* You may not use this file except in compliance with the License.
* A copy of the License is located at
*
*  http://aws.amazon.com/apache2.0
*
* or in the "license" file accompanying this file. This file is distributed
* on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
* express or implied. See the License for the specific language governing
* permissions and limitations under the License.
*/
using Amazon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This class is responsible for managing adaptive retry mode client rate limiting.
    /// </summary>
    public class TokenBucket
    {
        private const int MaxAttempts = 15;
        private readonly object _bucketLock = new object();
        private readonly double _minFillRate;
        private readonly double _minCapacity;
        private readonly double _beta;
        private readonly double _scaleConstant;
        private readonly double _smooth;
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1);

        /// <summary>
        /// The rate at which token are replenished.
        /// </summary>
        protected double? FillRate { get; set; }

        /// <summary>
        /// The maximum capacity allowed in the token 
        /// </summary>
        protected double? MaxCapacity { get; set; }

        /// <summary>
        /// The current capacity of the token 
        /// </summary>
        protected double CurrentCapacity { get; set; }

        /// <summary>
        /// The last time the token bucket was refilled.
        /// </summary>
        protected double? LastTimestamp { get; set; }

        /// <summary>
        /// The smoothed rate which tokens are being retrieved.
        /// </summary>
        protected double MeasuredTxRate { get; set; }

        /// <summary>
        /// The last half second time bucket used.
        /// </summary>
        protected double LastTxRateBucket { get; set; }

        /// <summary>
        /// The number of requests seen within the current time 
        /// </summary>
        protected long RequestCount { get; set; }

        /// <summary>
        /// The maximum rate when the client was last throttled.
        /// </summary>
        protected double LastMaxRate { get; set; }

        /// <summary>
        /// The last time when the client was throttled.
        /// </summary>
        protected double LastThrottleTime { get; set; }

        /// <summary>
        /// The cached time window calculation
        /// </summary>
        protected double TimeWindow { get; set; }

        /// <summary>
        /// Boolean indicating if the token bucket is enabled.
        /// </summary>
        protected bool Enabled { get; set; }


        public TokenBucket() 
            : this(minFillRate: 0.5, minCapacity: 1.0, beta: 0.7, scaleConstant: 0.4, smooth: 0.8)
        {
        }

        public TokenBucket(double minFillRate, double minCapacity, double beta, double scaleConstant, double smooth)
        {
            _minFillRate = minFillRate;
            _minCapacity = minCapacity;
            _beta = beta;
            _scaleConstant = scaleConstant;
            _smooth = smooth;

            LastTxRateBucket = Math.Floor(GetTimestamp());
            LastThrottleTime = GetTimestamp();
        }        

        /// <summary>
        /// This method attempts to acquire capacity from the client's token 
        /// </summary>        
        /// <param name="amount">The amount of capacity to obtain from the token bucket</param>
        /// <param name="failFast">Indicates that the client should or shouldn't sleep and 
        /// try again if capacity cannot be obtained.
        /// </param>        
        public bool TryAcquireToken(double amount, bool failFast)
        {
            var result = SetupAcquireToken(amount);
            if (result != null)
            {
                return result.Value;
            }

            //We should never get to 15 attempts as the operation is only
            //waiting for any good capacity. If we still cannot obtain 
            //capacity return false indicating that capacity couldn't be
            //obtained.            
            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                var delay = ObtainCapacity(amount);
                if (delay == 0)
                {
                    break;
                }

                //If the client has asked us to fail quickly if we cannot get a send token
                //return indicating a token couldn't be obtained.
                if (failFast || attempt + 1 == MaxAttempts)
                {
                    return false;
                }

                WaitForToken(delay);
            }

            return true;                        
        }

#if AWS_ASYNC_API
        /// <summary>
        /// This method attempts to acquire capacity from the client's token 
        /// </summary>        
        /// <param name="amount">The amount of capacity to obtain from the token bucket</param>
        /// <param name="failFast">Indicates that the client should or shouldn't sleep and 
        /// try again if capacity cannot be obtained.
        /// </param>
        /// <param name="cancellationToken">Token which can be used to cancel the task.</param>        
        public async System.Threading.Tasks.Task<bool> TryAcquireTokenAsync(double amount, bool failFast, CancellationToken cancellationToken)
        {
            var result = SetupAcquireToken(amount);
            if(result != null)
            {
                return result.Value;
            }

            //We should never get to 15 attempts as the operation is only
            //waiting for any good capacity. If we still cannot obtain 
            //capacity return false indicating that capacity couldn't be
            //obtained.            
            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                var delay = ObtainCapacity(amount);
                if(delay == 0)
                {
                    break;
                }

                //If the client has asked us to fail quickly if we cannot get a send token
                //return indicating a token couldn't be obtained.
                if (failFast || attempt + 1 == MaxAttempts)
                {
                    return false;
                }

                await WaitForTokenAsync(delay, cancellationToken).ConfigureAwait(false);
            }

            return true;
        }
#endif

        private bool? SetupAcquireToken(double amount)
        {
            if (amount <= 0)
            {
                return false;
            }

            lock (_bucketLock)
            {
                if (!Enabled)
                {
                    return true;
                }

                TokenBucketRefill();
            }

            return null;
        }

        private int ObtainCapacity(double amount)
        {
            //Next see if we have capacity for the requested amount.
            double currentCapacity;
            double fillRate;
            lock (_bucketLock)
            {
                if (amount <= CurrentCapacity)
                {
                    CurrentCapacity -= amount;
                    return 0;
                }

                currentCapacity = CurrentCapacity;
                fillRate = FillRate.Value;
            }

            return CalculateWait(amount, currentCapacity, fillRate);
        }

        /// <summary>
        /// Updates the sending rate within the client's token bucket
        /// </summary>        
        /// <param name="isThrottlingError">Indicates if the request resulted in a throttling error.</param>
        public void UpdateClientSendingRate(bool isThrottlingError)
        {
            lock (_bucketLock)
            {
                UpdateMeasuredRate();

                double calculatedRate;

                if (isThrottlingError)
                {
                    double rateToUse;
                    if (!Enabled)
                    {
                        rateToUse = MeasuredTxRate;
                    }
                    else
                    {
                        rateToUse = Math.Min(MeasuredTxRate, FillRate.Value);
                    }

                    //The fill_rate is from the token 
                    LastMaxRate = rateToUse;
                    CalculateTimeWindow();
                    LastThrottleTime = GetTimestamp();
                    calculatedRate = CUBICThrottle(rateToUse);
                    Enabled = true;
                }
                else
                {
                    CalculateTimeWindow();
                    calculatedRate = CUBICSuccess(GetTimestamp());
                }

                var newRate = Math.Min(calculatedRate, 2.0 * MeasuredTxRate);
                TokenBucketUpdateRate(newRate);
            }            
        }

        protected virtual void TokenBucketRefill()
        {
            double timestamp = GetTimestamp();
            if (LastTimestamp == null)
            {
                LastTimestamp = timestamp;
                return;
            }

            double fillAmount = (timestamp - LastTimestamp.Value) * FillRate.Value;
            CurrentCapacity = Math.Min(MaxCapacity.Value, CurrentCapacity + fillAmount);
            LastTimestamp = timestamp;
        }

        protected virtual void TokenBucketUpdateRate(double newRps)
        {
            //Refill based on our current rate before we update to the new fill rate.
            TokenBucketRefill();
            FillRate = Math.Max(newRps, _minFillRate);
            MaxCapacity = Math.Max(newRps, _minCapacity);

            //When we scale down we can't have a current capacity that exceeds our max_capacity.
            CurrentCapacity = Math.Min(CurrentCapacity, MaxCapacity.Value);
        }

        protected virtual void UpdateMeasuredRate()
        {
            var timestamp = GetTimestamp();
            var time_bucket = Math.Floor(timestamp * 2) / 2;
            RequestCount++;

            if (time_bucket > LastTxRateBucket)
            {
                var current_rate = RequestCount / (time_bucket - LastTxRateBucket);
                MeasuredTxRate = (current_rate * _smooth) + (MeasuredTxRate * (1 - _smooth));
                RequestCount = 0;
                LastTxRateBucket = time_bucket;
            }             
        }

        protected virtual void CalculateTimeWindow()
        {
            //This is broken out into a separate calculation because it only
            //gets updated when LastMaxRate changes so it can be cached.
            TimeWindow = Math.Pow(((LastMaxRate * (1.0 - _beta)) / _scaleConstant), (1.0 / 3.0));                         
        }

        /// <summary>
        /// Calculates the rate
        /// </summary>        
        /// <param name="timestamp"></param>
        /// <returns>Returns the calculated rate for a successful call</returns>
        protected virtual double CUBICSuccess(double timestamp)
        {
            timestamp -= LastThrottleTime;
            return (_scaleConstant * Math.Pow(timestamp - TimeWindow, 3)) + LastMaxRate;            
        }

        /// <summary>
        /// Calculates the rate.
        /// </summary>
        /// <param name="rateToUse">The rate to use in the calculation</param>
        /// <returns>Returns the calculated rate for a throttled call</returns>
        protected virtual double CUBICThrottle(double rateToUse)
        {
            return rateToUse * _beta;
        }

        protected virtual int CalculateWait(double amount, double currentCapacity, double fillRate)
        {
            return (int)((amount - currentCapacity) / fillRate * 1000.0);
        }

        protected virtual void WaitForToken(int delayMs)
        {
            AWSSDKUtils.Sleep(delayMs);
        }

#if AWS_ASYNC_API
        protected virtual async System.Threading.Tasks.Task WaitForTokenAsync(int delayMs, CancellationToken cancellationToken)
        {
            await System.Threading.Tasks.Task.Delay(delayMs, cancellationToken).ConfigureAwait(false);
        }
#endif

        protected virtual double GetTimestamp()
        {
            return GetTimeInSeconds();
        }

        private static double GetTimeInSeconds()
        {            
            return (DateTime.UtcNow - _epoch).TotalSeconds;            
        }
    }        
}
