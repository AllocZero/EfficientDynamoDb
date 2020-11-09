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

 using Amazon.Runtime.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;

namespace Amazon.Runtime.SharedInterfaces.Internal
{
    /// <summary>
    /// CoreAmazonKMS is not meant to use directly. It implements ICoreAmazonKMS
    /// and allows other service clients to be able to use the KMS service as a runtime dependency.
    ///
    /// This class waits until a method is actually called on the ICoreAmazonKMS interface
    /// before requiring the KMS assembly to be present and loaded.
    /// </summary>
    public class CoreAmazonKMS : ICoreAmazonKMS
    {
        private object wrappedClientLock = new object();
        private ICoreAmazonKMS wrappedClient;
        private AmazonServiceClient existingClient;
        private string feature;

        private bool disposed = false;

        public CoreAmazonKMS(AmazonServiceClient existingClient, string feature)
        {
            this.existingClient = existingClient;
            this.feature = feature;
        }

        public byte[] Decrypt(byte[] ciphertextBlob, Dictionary<string, string> encryptionContext)
        {
            EnsureWrappedClientIsInstantiated();
            return wrappedClient.Decrypt(ciphertextBlob, encryptionContext);
        }

        public GenerateDataKeyResult GenerateDataKey(string keyID, Dictionary<string, string> encryptionContext, string keySpec)
        {
            EnsureWrappedClientIsInstantiated();
            return wrappedClient.GenerateDataKey(keyID, encryptionContext, keySpec);
        }

#if AWS_ASYNC_API

        public async System.Threading.Tasks.Task<byte[]> DecryptAsync(byte[] ciphertextBlob, Dictionary<string, string> encryptionContext)
        {
            EnsureWrappedClientIsInstantiated();
            return await wrappedClient.DecryptAsync(ciphertextBlob, encryptionContext).ConfigureAwait(false);
        }

        public async System.Threading.Tasks.Task<GenerateDataKeyResult> GenerateDataKeyAsync(string keyID, Dictionary<string, string> encryptionContext, string keySpec)
        {
            EnsureWrappedClientIsInstantiated();
            return await wrappedClient.GenerateDataKeyAsync(keyID, encryptionContext, keySpec).ConfigureAwait(false);
        }
#endif

        private void EnsureWrappedClientIsInstantiated()
        {
            if (wrappedClient == null)
            {
                lock (wrappedClientLock)
                {
                    if (wrappedClient == null)
                    {
                        wrappedClient = CreateFromExistingClient(existingClient, feature);
                    }
                }
            }
        }

        private static ICoreAmazonKMS CreateFromExistingClient(AmazonServiceClient existingClient, string feature)
        {
            ICoreAmazonKMS coreKMSClient = null;
            try
            {
                coreKMSClient = ServiceClientHelpers.CreateServiceFromAssembly<ICoreAmazonKMS>(
                    ServiceClientHelpers.KMS_ASSEMBLY_NAME, ServiceClientHelpers.KMS_SERVICE_CLASS_NAME,
                    existingClient);
            }
            catch (Exception e)
            {
                var msg = string.Format(CultureInfo.CurrentCulture,
                    "Error instantiating {0} from assembly {1}.  " +
                    "The assembly and class must be available at runtime in order to use {2}.",
                    ServiceClientHelpers.KMS_SERVICE_CLASS_NAME, ServiceClientHelpers.KMS_ASSEMBLY_NAME, feature);
                throw new InvalidOperationException(msg, e);
            }

            return coreKMSClient;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                lock (wrappedClientLock)
                {
                    if (wrappedClient != null)
                    {
                        wrappedClient.Dispose();
                        wrappedClient = null;
                    }
                }

                disposed = true;
            }
        }
    }
}
