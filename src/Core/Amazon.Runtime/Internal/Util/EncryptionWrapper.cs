/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */

using System;
using System.IO;
using System.Security.Cryptography;

namespace Amazon.Runtime.Internal.Util
{
    public abstract class EncryptionWrapper : IEncryptionWrapper
    {
        private SymmetricAlgorithm algorithm;
        private ICryptoTransform encryptor;
        private const int encryptionKeySize = 256;

        protected EncryptionWrapper()
        {
            algorithm = CreateAlgorithm();
        }

        protected abstract SymmetricAlgorithm CreateAlgorithm();

        #region IEncryptionWrapper Members

        public int AppendBlock(byte[] buffer, int offset, int count, byte[] target, int targetOffset)
        {
            int bytesRead = encryptor.TransformBlock(buffer, offset, count, target, targetOffset);
            return bytesRead;
        }

        public byte[] AppendLastBlock(byte[] buffer, int offset, int count)
        {
            byte[] finalTransform = encryptor.TransformFinalBlock(buffer, offset, count);
            return finalTransform;
        }

        public void CreateEncryptor()
        {
            encryptor = algorithm.CreateEncryptor();
        }

        public void SetEncryptionData(byte[] key, byte[] IV)
        {
            algorithm.KeySize = encryptionKeySize;
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.Mode = CipherMode.CBC;
            algorithm.Key = key;
            algorithm.IV = IV;
        }

        public void Reset()
        {
            CreateEncryptor();
        }

        #endregion
    }


    public class EncryptionWrapperAES : EncryptionWrapper
    {
        public EncryptionWrapperAES()
            : base() { }

        protected override SymmetricAlgorithm CreateAlgorithm()
        {
            return Aes.Create();
        }
    }
}
