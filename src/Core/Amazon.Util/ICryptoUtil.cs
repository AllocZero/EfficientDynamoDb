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
using System.Text;

using Amazon.Runtime;

namespace Amazon.Util
{
    public interface ICryptoUtil
    {
        string HMACSign(string data, string key, SigningAlgorithm algorithmName);
        string HMACSign(byte[] data, string key, SigningAlgorithm algorithmName);

        byte[] ComputeSHA256Hash(byte[] data);
        byte[] ComputeSHA256Hash(Stream steam);

        byte[] ComputeMD5Hash(byte[] data);
        byte[] ComputeMD5Hash(Stream steam);

        byte[] HMACSignBinary(byte[] data, byte[] key, SigningAlgorithm algorithmName);
    }
}
