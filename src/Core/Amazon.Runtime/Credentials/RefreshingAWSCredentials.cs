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

using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System;
using System.Globalization;
using System.Threading;

namespace Amazon.Runtime
{
    /// <summary>
    /// Abstract class for automatically refreshing AWS credentials
    /// </summary>
    public abstract class RefreshingAWSCredentials : AWSCredentials, IDisposable
    {
        private Logger _logger = Logger.GetLogger(typeof(RefreshingAWSCredentials));

        #region Refresh data

        /// <summary>
        /// Refresh state container consisting of credentials
        /// and the date of the their expiration
        /// </summary>
        public class CredentialsRefreshState
        {
            public ImmutableCredentials Credentials
            {
                get; 
                set;
            }
            public DateTime Expiration { get; set; }

            public CredentialsRefreshState()
            {
            }

            public CredentialsRefreshState(ImmutableCredentials credentials, DateTime expiration)
            {
                Credentials = credentials;
                Expiration = expiration;
            }

            internal bool IsExpiredWithin(TimeSpan preemptExpiryTime)
            {
#pragma warning disable CS0612 // Type or member is obsolete
                var now = AWSSDKUtils.CorrectedUtcNow;
#pragma warning restore CS0612 // Type or member is obsolete
                var exp = Expiration.ToUniversalTime();
                return (now > exp - preemptExpiryTime);
            }
        }

        /// <summary>
        /// Represents the current state of the Credentials.
        /// </summary>
        /// <remarks>This can be cleared without synchronization.</remarks>
        protected CredentialsRefreshState currentState;

        #region Private members

        private TimeSpan _preemptExpiryTime = TimeSpan.FromMinutes(0);
        private bool _disposed;
#if BCL35
        /// <summary>
        /// Semaphore to control thread access to GetCredentialsAsync method.
        /// The semaphore will allow only one thread to generate new credentials and
        /// update the current state.
        /// </summary>
        private readonly Semaphore _updateGeneratedCredentialsSemaphore = new Semaphore(1, 1);
#else
        /// <summary>
        /// Semaphore to control thread access to GetCredentialsAsync method.
        /// The semaphore will allow only one thread to generate new credentials and
        /// update the current state.
        /// </summary>
        private readonly SemaphoreSlim _updateGeneratedCredentialsSemaphore = new SemaphoreSlim(1, 1);
#endif

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// The time before actual expiration to expire the credentials.        
        /// Property cannot be set to a negative TimeSpan.
        /// </summary>
        public TimeSpan PreemptExpiryTime
        {
            get { return _preemptExpiryTime; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", "PreemptExpiryTime cannot be negative");
                _preemptExpiryTime = value;
            }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Returns an instance of ImmutableCredentials for this instance
        /// </summary>
        /// <returns></returns>
        public override ImmutableCredentials GetCredentials()
        {
            _updateGeneratedCredentialsSemaphore.Wait();
            try
            {
                // We save the currentState as it might be modified or cleared.
                var tempState = currentState;
                // If credentials are expired or we don't have any state yet, update
                if (ShouldUpdateState(tempState, PreemptExpiryTime))
                {
                    tempState = GenerateNewCredentials();
                    UpdateToGeneratedCredentials(tempState, PreemptExpiryTime);
                    currentState = tempState;                    
                }
                return tempState.Credentials.Copy();
            }
            finally
            {
                _updateGeneratedCredentialsSemaphore.Release();
            }
        }

#if AWS_ASYNC_API
        public override async System.Threading.Tasks.Task<ImmutableCredentials> GetCredentialsAsync()
        {
            await _updateGeneratedCredentialsSemaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                // We save the currentState as it might be modified or cleared.
                var tempState = currentState;
                // If credentials are expired, update
                if (ShouldUpdateState(tempState, PreemptExpiryTime))
                {
                    tempState = await GenerateNewCredentialsAsync().ConfigureAwait(false);
                    UpdateToGeneratedCredentials(tempState, PreemptExpiryTime);
                    currentState = tempState;
                }
                return tempState.Credentials.Copy();
            }
            finally
            {
                _updateGeneratedCredentialsSemaphore.Release();
            }
        }
#endif

        #endregion

        #region Private/protected credential update methods

        private static void UpdateToGeneratedCredentials(CredentialsRefreshState state, TimeSpan preemptExpiryTime)
        {
            // Check if the new credentials are already expired
            if (ShouldUpdateState(state, preemptExpiryTime))
            {
                string errorMessage;
                if (state == null)
                    errorMessage = "Unable to generate temporary credentials";
                else
                    errorMessage = string.Format(CultureInfo.InvariantCulture,
                        "The retrieved credentials have already expired: Now = {0}, Credentials expiration = {1}",
#pragma warning disable CS0612 // Type or member is obsolete
                        AWSSDKUtils.CorrectedUtcNow.ToLocalTime(), state.Expiration);
#pragma warning restore CS0612 // Type or member is obsolete
                throw new AmazonClientException(errorMessage);
            }

            // Offset the Expiration by PreemptExpiryTime. This produces the expiration window 
            // where the credentials should be updated before they actually expire.
            state.Expiration -= preemptExpiryTime;

            if (ShouldUpdateState(state, preemptExpiryTime))
            {
                // This could happen if the default value of PreemptExpiryTime is
                // overridden and set too high such that ShouldUpdate returns true.
                var logger = Logger.GetLogger(typeof(RefreshingAWSCredentials));
                logger.InfoFormat(
                    "The preempt expiry time is set too high: Current time = {0}, Credentials expiry time = {1}, Preempt expiry time = {2}.",
#pragma warning disable CS0612 // Type or member is obsolete
                    AWSSDKUtils.CorrectedUtcNow.ToLocalTime(),
#pragma warning restore CS0612 // Type or member is obsolete
                    state.Expiration, preemptExpiryTime);
            }
        }

        /// <summary>
        /// Test credentials existence and expiration time
        /// should update if:
        /// credentials have not been loaded yet
        /// it's past the expiration time. At this point currentState.Expiration may
        /// have the PreemptExpiryTime baked into to the expiration from a call to
        /// UpdateToGeneratedCredentials but it may not if this is new application load.
        /// </summary>
        protected bool ShouldUpdate
        {
            get
            {
                return ShouldUpdateState(currentState, PreemptExpiryTime);
            }
        }

        // Test credentials existence and expiration time
        // should update if:
        //  credentials have not been loaded yet
        // it's past the expiration time. At this point currentState.Expiration may 
        // have the PreemptExpiryTime baked into to the expiration from a call to 
        // UpdateToGeneratedCredentials but it may not if this is new application 
        // load.
        private static bool ShouldUpdateState(CredentialsRefreshState state, TimeSpan preemptExpiryTime)
        {
            // it's past the expiration time. At this point currentState.Expiration may 
            // have the PreemptExpiryTime baked into to the expiration from a call to 
            // UpdateToGeneratedCredentials but it may not if this is new application 
            // load.
            var isExpired = state?.IsExpiredWithin(TimeSpan.Zero) ?? true;
            if (state != null && isExpired)
            {
#pragma warning disable CS0612 // Type or member is obsolete
                var logger = Logger.GetLogger(typeof(RefreshingAWSCredentials));
                logger.InfoFormat("Determined refreshing credentials should update. Expiration time: {0}, Current time: {1}",
                                state.Expiration.Add(preemptExpiryTime).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture),
                                AWSSDKUtils.CorrectedUtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK", CultureInfo.InvariantCulture));
#pragma warning restore CS0612 // Type or member is obsolete
            }

            return isExpired;
        }

        /// <summary>
        /// When overridden in a derived class, generates new credentials and new expiration date.
        /// 
        /// Called on first credentials request and when expiration date is in the past.
        /// </summary>
        /// <returns></returns>
        protected virtual CredentialsRefreshState GenerateNewCredentials()
        {
            throw new NotImplementedException();
        }
#if AWS_ASYNC_API
        /// <summary>
        /// When overridden in a derived class, generates new credentials and new expiration date.
        /// 
        /// Called on first credentials request and when expiration date is in the past.
        /// </summary>
        /// <returns></returns>
        protected virtual System.Threading.Tasks.Task<CredentialsRefreshState> GenerateNewCredentialsAsync()
        {
            return System.Threading.Tasks.Task.Run(() => this.GenerateNewCredentials());
        }
#endif
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _updateGeneratedCredentialsSemaphore.Dispose();
            }

            _disposed = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears currently-stored credentials, forcing the next GetCredentials call to generate new credentials.
        /// </summary>
        public virtual void ClearCredentials()
        {
            currentState = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}