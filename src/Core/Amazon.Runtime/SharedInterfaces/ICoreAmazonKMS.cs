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
using System.Collections.Generic;
using System.Net;

namespace Amazon.Runtime.SharedInterfaces
{
    /// <summary>
    /// ICoreAmazonKMS is not meant to be used directly. It defines Key Management Service
    /// with basic .NET types and allows other services to be able to use the service as
    /// a runtime dependency. This interface is implemented by the AmazonKeyManagementServiceClient
    /// defined in the AWSSDK.KeyManagementService assembly.
    /// </summary>
    public interface ICoreAmazonKMS : IDisposable
    {
        GenerateDataKeyResult GenerateDataKey(string keyID, Dictionary<string, string> encryptionContext, string keySpec);
        byte[] Decrypt(byte[] ciphertextBlob, Dictionary<string, string> encryptionContext);

#if AWS_ASYNC_API

        System.Threading.Tasks.Task<GenerateDataKeyResult> GenerateDataKeyAsync(string keyID, Dictionary<string, string> encryptionContext, string keySpec);
        System.Threading.Tasks.Task<byte[]> DecryptAsync(byte[] ciphertextBlob, Dictionary<string, string> encryptionContext);

#endif
    }

    /// <summary>
    /// The result of the GenerateDataKey and GenerateDataKeyAsync operations.
    /// </summary>
    public class GenerateDataKeyResult
    {
        /// <summary>
        /// The plaintext for the data key.
        /// </summary>
        public byte[] KeyPlaintext { get; set; }

        /// <summary>
        /// The ciphertext for the data key.
        /// </summary>
        public byte[] KeyCiphertext { get; set; }
    }
}
