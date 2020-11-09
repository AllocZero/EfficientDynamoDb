using System;
using System.Collections.Generic;

using System.Diagnostics.CodeAnalysis;

namespace Amazon.Util.Internal.PlatformServices
{
    public class ServiceFactory
    {
        internal const string NotImplementedErrorMessage =
            "This functionality is not implemented in the portable version of this assembly. "+
            "You should reference the AWSSDK.Core NuGet package from your main application project in order to reference the platform-specific implementation.";

        enum InstantiationModel
        {
            Singleton,
            InstancePerCall
        }

        private static readonly object _lock = new object();
        private static bool _factoryInitialized = false;

        private static IDictionary<Type, Type> _mappings =
            new Dictionary<Type, Type>()
            {
                {typeof(IApplicationSettings),typeof(ApplicationSettings)},
                {typeof(INetworkReachability),typeof(NetworkReachability)},
                {typeof(IApplicationInfo),typeof(ApplicationInfo)},
                {typeof(IEnvironmentInfo),typeof(EnvironmentInfo)}
            };

        private IDictionary<Type, InstantiationModel> _instantationMappings =
            new Dictionary<Type, InstantiationModel>()
            {
                {typeof(IApplicationSettings), InstantiationModel.InstancePerCall},
                {typeof(INetworkReachability), InstantiationModel.Singleton},
                {typeof(IApplicationInfo),InstantiationModel.Singleton},
                {typeof(IEnvironmentInfo),InstantiationModel.Singleton}
            };

        private IDictionary<Type, object> _singletonServices =
            new Dictionary<Type, object>();

        private ServiceFactory()
        {
            // Instantiate services registered as singletons.
            foreach (var service in _instantationMappings)
            {
                var serviceType = service.Key;
                if (service.Value == InstantiationModel.Singleton)
                {
                    var serviceInstance = Activator.CreateInstance(_mappings[serviceType]);
                    _singletonServices.Add(serviceType, serviceInstance);
                }
            }
            _factoryInitialized = true;
        }

        [SuppressMessage("Microsoft.Usage", "CA2211:NonConstantFieldsShouldNotBeVisible")]
        public static ServiceFactory Instance = new ServiceFactory();

        public static void RegisterService<T>(Type serviceType)
        {
            if (_factoryInitialized)
            {
                throw new InvalidOperationException(
                    "New services can only be registered before ServiceFactory is accessed by calling ServiceFactory.Instance .");
            }

            lock (_lock)
            {
                _mappings[typeof(T)] = serviceType;
            }
        }

        public T GetService<T>()
        {
            var serviceType = typeof(T);
            if (_instantationMappings[serviceType] == InstantiationModel.Singleton)
            {
                return (T)_singletonServices[serviceType];
            }

            var concreteType = GetServiceType<T>();
            return (T)Activator.CreateInstance(concreteType);
        }

        private static Type GetServiceType<T>()
        {
            lock (_lock)
            {
                return _mappings[typeof(T)];
            }
        }
    }
}
