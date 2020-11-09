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
using ThirdParty.MD5;

namespace Amazon.Runtime.Internal.Util
{
    public partial class HashingWrapper : IHashingWrapper
    {
        private static string MD5ManagedName = typeof(MD5Managed).FullName;

        private MD5Managed _algorithm = null;
        private void Init(string algorithmName)
        {
            if (string.Equals(MD5ManagedName, algorithmName, StringComparison.Ordinal))
                _algorithm = new MD5Managed();
            else
                throw new ArgumentOutOfRangeException(algorithmName, "Unsupported hashing algorithm");
        }


        #region IHashingWrapper Members

        public void Clear()
        {
            _algorithm.Initialize();
        }

        public byte[] ComputeHash(byte[] buffer)
        {
            return _algorithm.ComputeHash(buffer);
        }

        public byte[] ComputeHash(Stream stream)
        {
            return _algorithm.ComputeHash(stream);
        }

        public void AppendBlock(byte[] buffer, int offset, int count)
        {
            _algorithm.TransformBlock(buffer, offset, count);
        }

        public byte[] AppendLastBlock(byte[] buffer, int offset, int count)
        {
            var hash = _algorithm.TransformFinalBlock(buffer, offset, count);
            return hash;
        }

        #endregion

        #region Dispose Pattern Implementation

        /// <summary>
        /// Implements the Dispose pattern
        /// </summary>
        /// <param name="disposing">Whether this object is being disposed via a call to Dispose
        /// or garbage collected.</param>
        protected virtual void Dispose(bool disposing)
        {
            var disposable = _algorithm as IDisposable;
            if (disposing && disposable != null)
            {
                disposable.Dispose();
                _algorithm = null;
            }
        }

        #endregion
    }

    public class HashingWrapperMD5 : HashingWrapper
    {
        public HashingWrapperMD5()
            : base(typeof(MD5Managed).FullName)
        { }
    }
}
