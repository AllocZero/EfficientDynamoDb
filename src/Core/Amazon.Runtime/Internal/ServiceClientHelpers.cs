using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;

using Amazon.Util.Internal;

namespace Amazon.Runtime.Internal
{
    public static class ServiceClientHelpers
    {
        public const string S3_ASSEMBLY_NAME = "AWSSDK.S3";
        public const string S3_SERVICE_CLASS_NAME = "Amazon.S3.AmazonS3Client";

        public const string STS_ASSEMBLY_NAME = "AWSSDK.SecurityToken";
        public const string STS_SERVICE_CLASS_NAME = "Amazon.SecurityToken.AmazonSecurityTokenServiceClient";
        public const string STS_SERVICE_CONFIG_NAME = "Amazon.SecurityToken.AmazonSecurityTokenServiceConfig";

        public const string KMS_ASSEMBLY_NAME = "AWSSDK.KeyManagementService";
        public const string KMS_SERVICE_CLASS_NAME = "Amazon.KeyManagementService.AmazonKeyManagementServiceClient";

        public static TClient CreateServiceFromAnother<TClient, TConfig>(AmazonServiceClient originalServiceClient)
            where TConfig : ClientConfig, new ()
            where TClient : AmazonServiceClient
        {
            var credentials = originalServiceClient.Credentials;
            var newConfig = originalServiceClient.CloneConfig<TConfig>();

            var newServiceClientTypeInfo = TypeFactory.GetTypeInfo(typeof(TClient));

            var constructor = newServiceClientTypeInfo.GetConstructor(new ITypeInfo[]
                {
                    TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
                    TypeFactory.GetTypeInfo(newConfig.GetType())
                });

            var newServiceClient = constructor.Invoke(new object[] { credentials, newConfig }) as TClient;

            return newServiceClient;
        }

        public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName,
            RegionEndpoint region)
            where TClient : class
        {
            var serviceClientTypeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);

            var constructor = serviceClientTypeInfo.GetConstructor(new ITypeInfo[]
                {
                    TypeFactory.GetTypeInfo(typeof(RegionEndpoint))
                });

            var newServiceClient = constructor.Invoke(new object[] { region }) as TClient;

            return newServiceClient;
        }

        public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName, 
            AWSCredentials credentials, RegionEndpoint region)
            where TClient : class
        {
            var serviceClientTypeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);

            var constructor = serviceClientTypeInfo.GetConstructor(new ITypeInfo[]
                {
                    TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
                    TypeFactory.GetTypeInfo(typeof(RegionEndpoint))
                });

            var newServiceClient = constructor.Invoke(new object[] { credentials, region }) as TClient;

            return newServiceClient;
        }

        public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName,
            AWSCredentials credentials, ClientConfig config)
            where TClient : class
        {
            var serviceClientTypeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);
        
            var constructor = serviceClientTypeInfo.GetConstructor(new ITypeInfo[]
                {
                    TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
                    TypeFactory.GetTypeInfo(config.GetType())
                });

            var newServiceClient = constructor.Invoke(new object[] { credentials, config }) as TClient;

            return newServiceClient;
        }

        public static TClient CreateServiceFromAssembly<TClient>(string assemblyName, string serviceClientClassName, AmazonServiceClient originalServiceClient)
            where TClient : class
        {
            var serviceClientTypeInfo = LoadServiceClientType(assemblyName, serviceClientClassName);

            var config = CreateServiceConfig(assemblyName, serviceClientClassName.Replace("Client", "Config"));
            originalServiceClient.CloneConfig(config);

            var constructor = serviceClientTypeInfo.GetConstructor(new ITypeInfo[]
                {
                    TypeFactory.GetTypeInfo(typeof(AWSCredentials)),
                    TypeFactory.GetTypeInfo(config.GetType())
                });

            var newServiceClient = constructor.Invoke(new object[] { originalServiceClient.Credentials, config }) as TClient;

            return newServiceClient;
        }

        public static ClientConfig CreateServiceConfig(string assemblyName, string serviceConfigClassName)
        {
            var typeInfo = LoadServiceConfigType(assemblyName, serviceConfigClassName);

            var ci = typeInfo.GetConstructor(new ITypeInfo[0]);
            var config = ci.Invoke(new object[0]);

            return config as ClientConfig;
        }

        private static ITypeInfo LoadServiceClientType(string assemblyName, string serviceClientClassName)
        {
            var assembly = GetSDKAssembly(assemblyName);
            var type = assembly.GetType(serviceClientClassName);

            return TypeFactory.GetTypeInfo(type);
        }

        private static ITypeInfo LoadServiceConfigType(string assemblyName, string serviceConfigClassName)
        {
            var assembly = GetSDKAssembly(assemblyName);
            var type = assembly.GetType(serviceConfigClassName);

            return TypeFactory.GetTypeInfo(type);
        }

        private static Assembly GetSDKAssembly(string assemblyName)
        {
            Assembly assembly = null;
#if BCL || (NETSTANDARD && !NETSTANDARD13)
            assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => string.Equals(x.GetName().Name, assemblyName, StringComparison.Ordinal));
#endif
            if (assembly == null)
            {
                assembly = Assembly.Load(new AssemblyName(assemblyName));
            }

            if (assembly == null)
            {
                throw new AmazonClientException
                    (
                        string.Format(CultureInfo.InvariantCulture, "Failed to load assembly. Be sure to include a reference to {0}.", assemblyName)
                    );
            }

            return assembly;
        }
    }
}
