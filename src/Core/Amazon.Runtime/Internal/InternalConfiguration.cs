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
using System.Globalization;

using Amazon.Runtime.Internal.Util;
using System.Collections.Generic;
using Amazon.Util;
#if BCL || NETSTANDARD
using Amazon.Runtime.CredentialManagement;
#endif
using System.ComponentModel;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// InternalConfiguration holds the cached SDK configuration values
    /// obtained from the environment and profile configuration
    /// factories. These configuration values are loaded internally and
    /// are not the same as user exposed AWSConfigs.
    /// </summary>
    public class InternalConfiguration
    {
        /// <summary>
        /// Flag indicating if Endpoint Discovery is enabled.
        /// </summary>
        public bool? EndpointDiscoveryEnabled { get; set; }
        
        /// <summary>
        /// The retry mode to use: Legacy, Standard, or Adaptive.
        /// </summary>
        public RequestRetryMode? RetryMode { get; set; }
        
        /// <summary>
        /// The max number of request attempts.
        /// </summary>
        public int? MaxAttempts { get; set; }
    }

#if BCL || NETSTANDARD

    /// <summary>
    /// Determines the configuration values based on environment variables. If 
    /// no values is found for a configuration the value will be set to null.
    /// </summary>
    public class EnvironmentVariableInternalConfiguration : InternalConfiguration
    {
        private Logger _logger = Logger.GetLogger(typeof(EnvironmentVariableInternalConfiguration));

        public const string ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY = "AWS_ENABLE_ENDPOINT_DISCOVERY";
        public const string ENVIRONMENT_VARIABLE_AWS_MAX_ATTEMPTS = "AWS_MAX_ATTEMPTS";
        public const string ENVIRONMENT_VARIABLE_AWS_RETRY_MODE = "AWS_RETRY_MODE";
        
#if NETSTANDARD13
        /// <summary>
        /// Filler for netstandard 1.3 not supporting TypeConverter.
        /// </summary>        
        private static object ConvertToType(string value, Type type)
        {
            Object convertedObj = null;
            if (type == typeof(bool?))
            {
                convertedObj = (bool?)Convert.ToBoolean(value);
            }
            else if (type == typeof(RequestRetryMode?))
            {
                convertedObj = (RequestRetryMode?)Enum.Parse(typeof(RequestRetryMode), value, true);
            }
            else if (type == typeof(int?))
            {
                convertedObj = (int?)Convert.ToInt32(value);
            }

            return convertedObj;
        }
#endif

        /// <summary>
        /// Attempts to construct a configuration instance of configuration environment 
        /// variables. If an environment variable value isn't found then the individual value 
        /// for that environment variable will be null. If unable to obtain a value converter 
        /// to convert a configuration string to the appropriate type a InvalidOperationException 
        /// is thrown.
        /// </summary>
        public EnvironmentVariableInternalConfiguration()
        {
            EndpointDiscoveryEnabled = GetEnvironmentVariable<bool>(ENVIRONMENT_VARIABLE_AWS_ENABLE_ENDPOINT_DISCOVERY);
            MaxAttempts = GetEnvironmentVariable<int>(ENVIRONMENT_VARIABLE_AWS_MAX_ATTEMPTS);
            RetryMode = GetEnvironmentVariable<RequestRetryMode>(ENVIRONMENT_VARIABLE_AWS_RETRY_MODE);
        }

        private bool TryGetEnvironmentVariable(string environmentVariableName, out string value)
        {
            value = Environment.GetEnvironmentVariable(environmentVariableName);
            if (string.IsNullOrEmpty(value))
            {
                _logger.InfoFormat($"The environment variable {environmentVariableName} was not set with a value.");
                value = null;
                return false;
            }

            return true;
        }

        private T? GetEnvironmentVariable<T>(string name) where T : struct
        {
            if (!TryGetEnvironmentVariable(name, out var value))
            {
                return null;
            }
#if !NETSTANDARD13
            var converter = TypeDescriptor.GetConverter(typeof(T?));
            if (converter == null)
            {
                throw new InvalidOperationException($"Unable to obtain type converter for type {typeof(T?)} " +
                    $"to convert environment variable {name}.");
            }
#endif

            try
            {
#if !NETSTANDARD13
                return (T?)converter.ConvertFromString(value);
#else
                return (T?)ConvertToType(value, typeof(T?));
#endif
            }
            catch (Exception e)
            {
                _logger.Error(e, $"The environment variable {name} was set with value {value}, but it could not be parsed as a valid value.");
            }
            return null;
        }
    }

    /// <summary>
    /// Determines configuration values based on a <see cref="CredentialProfile"/> stored in an <see cref="ICredentialProfileSource"/>.
    /// If the profile doesn't exist, the status will be logged and no value will be set in the configuration.
    /// </summary>
    public class ProfileInternalConfiguration : InternalConfiguration
    {
        private Logger _logger = Logger.GetLogger(typeof(ProfileInternalConfiguration));        

        /// <summary>
        /// Attempts to construct an instance of <see cref="ProfileInternalConfiguration"/>.
        /// If the AWS_PROFILE environment variable is set the instance will be constructed using that profile,
        /// otherwise it will use the default profile.
        ///
        /// If the profile doesn't exist status will be logged and no value will be set in the configuration.
        /// </summary>
        /// <param name="source">The ICredentialProfileSource to read the profile from.</param>
        public ProfileInternalConfiguration(ICredentialProfileSource source)
        {
            var profileName = Environment.GetEnvironmentVariable(FallbackCredentialsFactory.AWS_PROFILE_ENVIRONMENT_VARIABLE);
            if (profileName == null)
                profileName = FallbackCredentialsFactory.DefaultProfileName;
            Setup(source, profileName);
        }

        /// <summary>
        /// Attempts to construct an instance of <see cref="ProfileInternalConfiguration"/>.
        /// If the profile doesn't exist status will be logged and no value will be set in the configuration.
        /// </summary>
        /// <param name="source">The ICredentialProfileSource to read the profile from.</param>
        /// <param name="profileName">The name of the profile.</param>
        public ProfileInternalConfiguration(ICredentialProfileSource source, string profileName)
        {
            Setup(source, profileName);
        }

        private void Setup(ICredentialProfileSource source, string profileName)
        {
            CredentialProfile profile;
            if (source.TryGetProfile(profileName, out profile))
            {
                EndpointDiscoveryEnabled = profile.EndpointDiscoveryEnabled;
                RetryMode = profile.RetryMode;
                MaxAttempts = profile.MaxAttempts;
            }
            else
            {
                _logger.InfoFormat("Unable to find a profile named '" + profileName + "' in store " + source.GetType());
                return;
            }

            var items = new KeyValuePair<string, object>[]
            {
                new KeyValuePair<string, object>("endpoint_discovery_enabled", profile.EndpointDiscoveryEnabled),
                new KeyValuePair<string, object>("retry_mode", profile.RetryMode),
                new KeyValuePair<string, object>("max_attempts", profile.MaxAttempts)
            };

            foreach(var item in items)
            {
                _logger.InfoFormat(item.Value == null
                ? $"There is no {item.Key} set in the profile named '{profileName}' in store {source.GetType()}"
                : $"{item.Key} found in profile '{profileName}' in store {source.GetType()}"
                );
            }
        }                
    }

#endif

        /// <summary>
        /// Probing mechanism to determine the configuration values from various sources.
        /// </summary>
        public static class FallbackInternalConfigurationFactory
    {
#if BCL || NETSTANDARD
        private static CredentialProfileStoreChain _credentialProfileChain = new CredentialProfileStoreChain();
#endif
        private static InternalConfiguration _cachedConfiguration;

        static FallbackInternalConfigurationFactory()
        {
            Reset();
        }

        private delegate InternalConfiguration ConfigGenerator();

        /// <summary>
        /// Resets all the configuration values reloading as needed. This method will use
        /// the AWS_PROFILE environment variable if set to construct the instance. Otherwise 
        /// the default profile will be used.
        /// </summary>        
        public static void Reset()
        {            
#if BCL || NETSTANDARD
            //Preload configurations that are fast or pull all the values at the same time. Slower configurations
            //should be called for specific values dynamically.
            InternalConfiguration environmentVariablesConfiguration =  new EnvironmentVariableInternalConfiguration();
            InternalConfiguration profileConfiguration = new ProfileInternalConfiguration(_credentialProfileChain);
#endif

            _cachedConfiguration = new InternalConfiguration();
            var standardGenerators = new List<ConfigGenerator>
            {  
#if BCL || NETSTANDARD
                () => environmentVariablesConfiguration,
                () => profileConfiguration,
#endif
            };

            //Find the priority first ordered config value for each property
            _cachedConfiguration.EndpointDiscoveryEnabled = SeekValue(standardGenerators, (c) => c.EndpointDiscoveryEnabled);
            _cachedConfiguration.RetryMode = SeekValue(standardGenerators, (c) => c.RetryMode);
            _cachedConfiguration.MaxAttempts = SeekValue(standardGenerators, (c) => c.MaxAttempts);
        }        
                
        private static T? SeekValue<T>(List<ConfigGenerator> generators, Func<InternalConfiguration, T?> getValue) where T : struct
        {
            //Look for the configuration value stopping at the first generator that returns the expected value.
            foreach (var generator in generators)
            {
                var configuration = generator();
                T? value = getValue(configuration);
                if (value.HasValue)
                {
                    return value;
                }
            }

            return null;
        }

        /// <summary>
        /// Flag that specifies if endpoint discovery is enabled, disabled, 
        /// or not set.
        /// </summary>
        public static bool? EndpointDiscoveryEnabled
        {
            get
            {
                return _cachedConfiguration.EndpointDiscoveryEnabled;
            }
        }

        /// <summary>
        /// Flag that specifies which retry mode to use or if retry mode has 
        /// not been set.
        /// </summary>
        public static RequestRetryMode? RetryMode
        {
            get
            {
                return _cachedConfiguration.RetryMode;
            }
        }

        /// <summary>
        /// Flag that specifies the max number of request attempts or if max 
        /// attempts has not been set.
        /// </summary>
        public static int? MaxAttempts
        {
            get
            {
                return _cachedConfiguration.MaxAttempts;
            }
        }
    }
}
