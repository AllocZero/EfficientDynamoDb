using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// This is a utility class to be used in last resort for code paths that are synchronous but need to call an asynchronous method.
    /// This should never be used for methods that are called at high volume as this utility class has a performance cost. For example this
    /// class was added for the refreshing credentials like Cognito which would need to use this about once an hour.
    /// </summary>
    public static class AsyncHelpers
    {
        public static void RunSync(Func<Task> workItem)
        {
            var prevContext = SynchronizationContext.Current;
            try
            {
                var synch = new ExclusiveSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(synch);
                synch.Post(async _ =>
                {
                    try
                    {
                        await workItem().ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        synch.ObjectException = e;
                        throw;
                    }
                    finally
                    {
                        synch.EndMessageLoop();
                    }
                }, null);
                synch.BeginMessageLoop();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevContext);
            }
        }

        public static T RunSync<T>(Func<Task<T>> workItem)
        {
            var prevContext = SynchronizationContext.Current;
            try
            {
                var synch = new ExclusiveSynchronizationContext();
                SynchronizationContext.SetSynchronizationContext(synch);
                T ret = default(T);
                synch.Post(async _ =>
                {
                    try
                    {
                        ret = await workItem().ConfigureAwait(false);
                    }
                    catch (Exception e)
                    {
                        synch.ObjectException = e;
                        throw;
                    }
                    finally
                    {
                        synch.EndMessageLoop();
                    }
                }, null);
                synch.BeginMessageLoop();
                return ret;
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(prevContext);
            }
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception ObjectException { get; set; }
            readonly AutoResetEvent pendingObjects = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> objects =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (objects)
                {
                    objects.Enqueue(Tuple.Create(d, state));
                }
                pendingObjects.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> workItem = null;
                    lock (objects)
                    {
                        if (objects.Count > 0)
                        {
                            workItem = objects.Dequeue();
                        }
                    }
                    if (workItem != null)
                    {
                        workItem.Item1(workItem.Item2);
                        if (ObjectException != null)
                            ExceptionDispatchInfo.Capture(ObjectException).Throw();
                    }
                    else
                    {
                        pendingObjects.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }
}
