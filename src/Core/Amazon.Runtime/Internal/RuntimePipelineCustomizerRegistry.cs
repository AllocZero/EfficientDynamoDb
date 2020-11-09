using Amazon.Runtime.Internal.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// A registry of object that will manipulate the runtime pipeline used by service clients.
    /// </summary>
    public class RuntimePipelineCustomizerRegistry : IDisposable
    {
        public static RuntimePipelineCustomizerRegistry Instance { get; } = new RuntimePipelineCustomizerRegistry();
        private RuntimePipelineCustomizerRegistry()
        {

        }

        Logger _logger = Logger.GetLogger(typeof(RuntimePipelineCustomizerRegistry));
        ReaderWriterLockSlim _rwlock = new ReaderWriterLockSlim();

        // List is used instead of a dictionary to maintain order
        IList<IRuntimePipelineCustomizer> _customizers = new List<IRuntimePipelineCustomizer>();

        /// <summary>
        /// Registers a customizer that will be applied for all service clients created. Each customizer has a unique name associated with it. If a customizer is registered more 
        /// than once with the same unique name then the calls after the first will be ignored.
        /// </summary>
        /// <param name="customizer"></param>
        public void Register(IRuntimePipelineCustomizer customizer)
        {
            _rwlock.EnterWriteLock();
            try
            {
                // If the customizer is already registered then skip adding it again. This could happen if 2 separate libraries are added
                // to a project and they both one to turn on a third party customizer
                if (_customizers.FirstOrDefault(x => string.Equals(x.UniqueName, customizer.UniqueName)) != null)
                {
                    _logger.InfoFormat("Skipping registration because runtime pipeline customizer {0} already registered", customizer.UniqueName);
                    return;
                }

                _logger.InfoFormat("Registering runtime pipeline customizer {0}", customizer.UniqueName);
                _customizers.Add(customizer);
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Deregistered the runtime pipeline customizer
        /// </summary>
        /// <param name="customizer"></param>
        public void Deregister(IRuntimePipelineCustomizer customizer)
        {
            Deregister(customizer.UniqueName);
        }

        /// <summary>
        /// Deregistered the runtime pipeline customizer
        /// </summary>
        /// <param name="uniqueName"></param>
        public void Deregister(string uniqueName)
        {
            _rwlock.EnterWriteLock();
            try
            {
                int pos = -1;
                for(int i = 0; i < _customizers.Count; i++)
                {
                    if(string.Equals(uniqueName, _customizers[i].UniqueName, StringComparison.Ordinal))
                    {
                        pos = i;
                        break;
                    }
                }

                if(pos != -1)
                {
                    _logger.InfoFormat("Deregistering runtime pipeline customizer {0}", uniqueName);
                    _customizers.RemoveAt(pos);
                }
                else
                {
                    _logger.InfoFormat("Runtime pipeline customizer {0} not found to deregister", uniqueName);
                }
            }
            finally
            {
                _rwlock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Applies all of the registered customizers on the runtime pipeline
        /// </summary>
        /// <param name="pipeline">The service clients runtime pipeline.</param>
        /// <param name="type">Type object for the service client being created</param>
        internal void ApplyCustomizations(Type type, RuntimePipeline pipeline)
        {
            _rwlock.EnterReadLock();
            try
            {
                foreach (var customizer in _customizers)
                {
                    _logger.InfoFormat("Applying runtime pipeline customization {0}", customizer.UniqueName);
                    customizer.Customize(type, pipeline);
                }
            }
            finally
            {
                _rwlock.ExitReadLock();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_rwlock != null)
                {
                    _rwlock.Dispose();
                    _rwlock = null;
                }
            }
        }


        #endregion
    }

    /// <summary>
    /// Interface for objects that will customize the runtime pipleine for newly created service clients.
    /// </summary>
    public interface IRuntimePipelineCustomizer
    {
        /// <summary>
        /// The unique name for the customizer that identifies the customizer in the registry. The name is also used to identify the customizer on the SDK logs.
        /// </summary>
        string UniqueName { get; }

        /// <summary>
        /// Called on service clients as they are being constructed to customize their runtime pipeline.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <param name="type">Type object for the service client being created</param>
        void Customize(Type type, RuntimePipeline pipeline);
    }
}
