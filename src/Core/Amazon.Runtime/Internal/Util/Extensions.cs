using Amazon.Util;
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
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Amazon.Runtime.Internal.Util
{
    public static partial class Extensions
    {
        private static readonly long TicksPerSecond = TimeSpan.FromSeconds(1).Ticks;
        private static readonly double TickFrequency = TicksPerSecond / (double)Stopwatch.Frequency;
        public static long GetElapsedDateTimeTicks(this Stopwatch self)
        {
            double stopwatchTicks = self.ElapsedTicks;
            var ticks = (long)(stopwatchTicks * TickFrequency);
            return ticks;
        }

        /// <summary>
        /// Returns true if the Content is set or there are
        /// query parameters.
        /// </summary>
        /// <param name="request">This request</param>
        /// <returns>True if data is present; false otherwise.</returns>
        public static bool HasRequestData(this IRequest request)
        {
            if (request == null)
                return false;

            if (request.ContentStream != null || request.Content != null)
                return true;

            return request.Parameters.Count > 0;
        }

#if BCL35
        public static bool Wait(this WaitHandle semaphore)
        {
            return semaphore.WaitOne();
        }
        
        public static void Dispose(this WaitHandle semaphore)
        {
            semaphore.Close();
        }
#endif
    }
}
