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
using System.Collections.Generic;
using System.Linq;


namespace Amazon.Runtime.Internal.Util
{
    public static class Hashing
    {
        /// <summary>
        /// Hashes a set of objects.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Hash(params object[] value)
        {
            var hashes = value
                .Select(v => v == null ? 0 : v.GetHashCode())
                .ToArray();
            var result = CombineHashes(hashes);
            return result;
        }

        /// <summary>
        /// Combines a set of hashses.
        /// </summary>
        /// <param name="hashes"></param>
        /// <returns></returns>
        public static int CombineHashes(params int[] hashes)
        {
            int result = 0;
            foreach (int hash in hashes)
            {
                result = CombineHashesInternal(result, hash);
            }
            return result;
        }

        /// <summary>
        /// Combines two hashes.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static int CombineHashesInternal(int a, int b)
        {
            return unchecked(((a << 5) + a) ^ b);
        }
    }
}
