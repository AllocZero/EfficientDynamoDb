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
using System.Collections;
using System.Collections.Generic;

using ThirdParty.Json.LitJson;
using Amazon.Util;
using Amazon.Util.Internal;
using Amazon.Util.Internal.PlatformServices;
using Amazon.Runtime;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This class composes Client Context header for Amazon Web Service client.
    /// It contains information like app title, version code, version name, client id, OS platform etc.
    /// </summary>
    public partial class ClientContext
    {
        //client related keys
        private const string CLIENT_KEY = "client";
        private const string CLIENT_ID_KEY = "client_id";
        private const string CLIENT_APP_TITLE_KEY = "app_title";
        private const string CLIENT_APP_VERSION_NAME_KEY = "app_version_name";
        private const string CLIENT_APP_VERSION_CODE_KEY = "app_version_code";
        private const string CLIENT_APP_PACKAGE_NAME_KEY = "app_package_name";
        
        //custom keys
        private const string CUSTOM_KEY = "custom";
        
        //env related keys
        private const string ENV_KEY = "env";
        private const string ENV_PLATFORM_KEY = "platform";
        private const string ENV_MODEL_KEY = "model";
        private const string ENV_MAKE_KEY = "make";
        private const string ENV_PLATFORM_VERSION_KEY = "platform_version";
        private const string ENV_LOCALE_KEY = "locale";
        
        //servies related keys
        private const string SERVICES_KEY = "services";
        // TODO: add support to other services
        private const string SERVICE_MOBILE_ANALYTICS_KEY = "mobile_analytics";
        private const string SERVICE_MOBILE_ANALYTICS_APP_ID_KEY = "app_id";
        

        private IDictionary<string,string> _client;
        private IDictionary<string,string> _custom;
        private IDictionary<string,string> _env;
        private IDictionary<string,IDictionary> _services;
        
        private IDictionary _clientContext;
        private static object _lock = new object();
#if BCL
        private static string _clientID = null;
#endif
        private const string APP_ID_KEY = "APP_ID_KEY";
        private const string CLIENT_ID_CACHE_FILENAME = "client-ID-cache";

        public string AppID { get; set; }

        /// <summary>
        /// Adds the custom attributes to the Client Context.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="value">Value.</param>
        public void AddCustomAttributes(string key, string value)
        {
            lock(_lock)
            {
                if (_custom == null)
                    _custom = new Dictionary<string, string>();

                _custom.Add(key,value);
            }
        }
        
        /// <summary>
        /// Gets a Json Representation of the Client Context.
        /// </summary>
        /// <returns>Json Representation of Client Context</returns>
        public String ToJsonString()
        {
            lock(_lock)
            {
                _client = new Dictionary<string, string>();
                _env = new Dictionary<string, string>();
                _services = new Dictionary<string, IDictionary>();
#if BCL
                // client
                _client.Add(CLIENT_ID_KEY, _clientID);

                if (!string.IsNullOrEmpty(_config.AppTitle))
                    _client.Add(CLIENT_APP_TITLE_KEY, _config.AppTitle);

                if (!string.IsNullOrEmpty(_config.AppVersionName))
                    _client.Add(CLIENT_APP_VERSION_NAME_KEY, _config.AppVersionName);

                if (!string.IsNullOrEmpty(_config.AppVersionCode))
                    _client.Add(CLIENT_APP_VERSION_CODE_KEY, _config.AppVersionCode);

                if (!string.IsNullOrEmpty(_config.AppPackageName))
                    _client.Add(CLIENT_APP_PACKAGE_NAME_KEY, _config.AppPackageName);


                // env
                if (!string.IsNullOrEmpty(_config.Platform))
                    _env.Add(ENV_PLATFORM_KEY, _config.Platform);
                else
                    _env.Add(ENV_PLATFORM_KEY, Environment.OSVersion.Platform.ToString());

                if (!string.IsNullOrEmpty(_config.PlatformVersion))
                    _env.Add(ENV_PLATFORM_VERSION_KEY, _config.PlatformVersion);
                else
                    _env.Add(ENV_PLATFORM_VERSION_KEY, Environment.OSVersion.Version.ToString());

                if (!string.IsNullOrEmpty(_config.Locale))
                    _env.Add(ENV_LOCALE_KEY, _config.Locale);
                else
                    _env.Add(ENV_LOCALE_KEY, System.Globalization.CultureInfo.CurrentCulture.Name);

                if (!string.IsNullOrEmpty(_config.Make))
                    _env.Add(ENV_MAKE_KEY, _config.Make);

                if (!string.IsNullOrEmpty(_config.Model))
                    _env.Add(ENV_MODEL_KEY, _config.Model);
#endif
                // services
                if (!string.IsNullOrEmpty(this.AppID))
                {
                    IDictionary mobileAnalyticsService = new Dictionary<string, string>();
                    mobileAnalyticsService.Add(SERVICE_MOBILE_ANALYTICS_APP_ID_KEY, AppID);
                    _services.Add(SERVICE_MOBILE_ANALYTICS_KEY, mobileAnalyticsService);
                }

                _clientContext = new Dictionary<string, IDictionary>();
                _clientContext.Add(CLIENT_KEY, _client);
                _clientContext.Add(ENV_KEY, _env);
                _clientContext.Add(CUSTOM_KEY, _custom);
                _clientContext.Add(SERVICES_KEY, _services);

                return JsonMapper.ToJson(_clientContext); 
            
            }
        }
    }
}

