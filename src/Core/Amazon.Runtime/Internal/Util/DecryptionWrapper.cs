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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime;

namespace Amazon.Runtime.Internal.Util
{
    public abstract class DecryptionWrapper : IDecryptionWrapper
    {
        private SymmetricAlgorithm algorithm;
        private ICryptoTransform decryptor;
        private const int encryptionKeySize = 256;

        protected DecryptionWrapper()
        {
            algorithm = CreateAlgorithm();
        }

        #region IDecryptionWrapper Members

        protected abstract SymmetricAlgorithm CreateAlgorithm();
        
        public ICryptoTransform Transformer
        {
            get { return this.decryptor; }
        }

        public void SetDecryptionData(byte[] key, byte[] IV)
        {
            algorithm.KeySize = encryptionKeySize;
            algorithm.Padding = PaddingMode.PKCS7;
            algorithm.Mode = CipherMode.CBC;
            algorithm.Key = key;
            algorithm.IV = IV;
        }

        public void CreateDecryptor()
        {
            decryptor = algorithm.CreateDecryptor();
        }
        #endregion
    }

    public class DecryptionWrapperAES : DecryptionWrapper
    {
        public DecryptionWrapperAES()
            : base()
        { }

        protected override SymmetricAlgorithm CreateAlgorithm()
        {
            return Aes.Create();
        }
    }
}
