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

#if !NETSTANDARD
using System.Runtime.Serialization;
#endif

namespace Amazon.Runtime.Internal
{
    /// <summary>
    /// Collection used to indicate if the property was initialized was created by the SDK.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AutoConstructedList<T> : List<T>
    {
    }

    /// <summary>
    /// Collection used to indicate if the property was initialized was created by the SDK.
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
#if !NETSTANDARD
    [Serializable]
#endif
    public class AutoConstructedDictionary<K, V> : Dictionary<K, V>
    {
#if !NETSTANDARD        
        protected AutoConstructedDictionary(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
#endif
    }
}
