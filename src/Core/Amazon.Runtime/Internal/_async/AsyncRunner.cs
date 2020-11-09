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
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This class is no longer used anymore and should be avoided as it is just meant
    /// as a last resort when blending async and sync operations. The code is still
    /// here to support versions before 3.3.9.0 of AWSSDK.DynamoDBv2.
    /// </summary>
    public static class AsyncRunner
    {
        public static Task Run(Action action, CancellationToken cancellationToken)
        {
            return Run<object>(() =>
            {
                action();
                return null;
            }, cancellationToken);
        }

        public static Task<T> Run<T>(Func<T> action, CancellationToken cancellationToken)
        {
            return Task<T>.Run(async () =>
            {
                Exception exception = null;
                T result = default(T);

                using (var semaphore = new SemaphoreSlim(0))
                {
                    Thread thread = new Thread(() =>
                    {
                        try
                        {
                            result = action();
                        }
                        catch (Exception e)
                        {
                            exception = e;
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });
#if !NETSTANDARD
                using (var ctr = cancellationToken.Register(() =>
                {
                    if (thread.IsAlive)
                        thread.Abort();
                }))
#endif
                    {
                        thread.Start();
                        await semaphore.WaitAsync().ConfigureAwait(false);
                        thread.Join();

                        if (exception != null)
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(exception).Throw();
                        }

                        return result;
                    }
                }
            }, cancellationToken);
        }
    }
}