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

using Amazon.Util.Internal.PlatformServices;
using System.Collections.Generic;
using System;
using Amazon.Util.Internal;
using System.IO;

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// This class composes Client Context header for Amazon Web Service client.
    /// It contains information like app title, version code, version name, client id, OS platform etc.
    /// </summary>
    public partial class ClientContext
    {
        private ClientContextConfig _config;

        public ClientContext(string appId, ClientContextConfig config)
        {
            _config = config;
            this.AppID = appId;
            this._custom = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(_clientID))
            {
                string fullPath = InternalSDKUtils.DetermineAppLocalStoragePath(CLIENT_ID_CACHE_FILENAME);
                var directoryPath = InternalSDKUtils.DetermineAppLocalStoragePath();

                lock (_lock)
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    if (!System.IO.File.Exists(fullPath) || new System.IO.FileInfo(fullPath).Length == 0)
                    {
                        _clientID = Guid.NewGuid().ToString();
                        System.IO.File.WriteAllText(fullPath, _clientID);
                    }
                    else
                    {
                        using (System.IO.StreamReader file = new System.IO.StreamReader(fullPath))
                        {
                            _clientID = file.ReadToEnd();
                            file.Close();
                        }
                    }
                }
            }
        }


    }

}