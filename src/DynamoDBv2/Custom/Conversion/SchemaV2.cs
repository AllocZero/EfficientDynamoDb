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

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Util.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Amazon.DynamoDBv2
{
    #region Same converter behavior as V1

    internal class ByteConverterV2 : ByteConverterV1
    { }

    internal class SByteConverterV2 : SByteConverterV1
    { }

    internal class UInt16ConverterV2 : UInt16ConverterV1
    { }

    internal class Int16ConverterV2 : Int16ConverterV1
    { }

    internal class UInt32ConverterV2 : UInt32ConverterV1
    { }

    internal class Int32ConverterV2 : Int32ConverterV1
    { }

    internal class UInt64ConverterV2 : UInt64ConverterV1
    { }

    internal class Int64ConverterV2 : Int64ConverterV1
    { }

    internal class SingleConverterV2 : SingleConverterV1
    { }

    internal class DoubleConverterV2 : DoubleConverterV1
    { }

    internal class DecimalConverterV2 : DecimalConverterV1
    { }

    internal class CharConverterV2 : CharConverterV1
    { }

    internal class StringConverterV2 : StringConverterV1
    { }

    internal class DateTimeConverterV2 : DateTimeConverterV1
    { }

    internal class GuidConverterV2 : GuidConverterV1
    { }

    internal class BytesConverterV2 : BytesConverterV1
    { }

    internal class MemoryStreamConverterV2 : MemoryStreamConverterV1
    { }

    internal class DictionaryConverterV2 : DictionaryConverterV1
    { }

    internal class EnumConverterV2 : EnumConverterV1
    { }

    #endregion

    /// <summary>
    /// A boolean converter which reads booleans as N or BOOL types,
    /// but writes out BOOL type.
    /// </summary>
    internal class BoolConverterV2 : BoolConverterV1
    {
        protected override bool TryTo(bool value, out DynamoDBBool b)
        {
            b = new DynamoDBBool(value);
            return true;
        }
        protected override bool TryTo(bool value, out Primitive p)
        {
            p = null;
            return false;
        }
    }

    /// <summary>
    /// A collection converter which reads both sets of collections (sets and lists)
    /// but writes out different types depending on input:
    /// HashSet input - converts to a DynamoDB set (NS, SS, BS)
    /// Any other IEnumerable input - converts to a DynamoDB list (L)
    /// </summary>
    internal class CollectionConverterV2 : PrimitiveCollectionConverterV1
    {
        private static ITypeInfo setTypeInfo = TypeFactory.GetTypeInfo(typeof(HashSet<>));
        private static Type enumerableType = typeof(IEnumerable<>);

        /// <summary>
        /// If value is HashSet{T}, converts the items to PrimitiveList
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pl"></param>
        /// <returns></returns>
        public override bool TryTo(object value, out PrimitiveList pl)
        {
            var inputType = value.GetType();
            var inputTypeInfo = TypeFactory.GetTypeInfo(inputType);

            if (inputTypeInfo.IsGenericType)
            {
                var genericTypeInfo = TypeFactory.GetTypeInfo(inputTypeInfo.GetGenericTypeDefinition());
                if (setTypeInfo.IsAssignableFrom(genericTypeInfo))
                {
                    pl = Conversion.ItemsToPrimitiveList(value as IEnumerable);
                    return true;
                }
            }

            pl = null;
            return false;
        }
        /// <summary>
        /// If value implements IEnumerable{T}, converts the items to DynamoDBList
        /// This method is called after the PrimitiveList version of TryTo, so this will
        /// never work on a HashSet{T}.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public override bool TryTo(object value, out DynamoDBList l)
        {
            var inputType = value.GetType();

            if (DataModel.Utils.ImplementsInterface(inputType, enumerableType))
            {
                var elementType = Utils.GetElementType(inputType);
                var entries = Conversion.ConvertToEntries(elementType, value as IEnumerable);
                l = new DynamoDBList(entries);
                return true;
            }

            l = null;
            return false;
        }
    }

    internal class DynamoDBListConverter : CollectionConverter
    {
        public DynamoDBListConverter()
            : this(Utils.PrimitiveTypes)
        { }
        public DynamoDBListConverter(IEnumerable<Type> memberTypes)
            : base(memberTypes)
        { }

        public override bool TryTo(object value, out DynamoDBList l)
        {
            var items = value as IEnumerable;
            if (items != null)
            {
                l = new DynamoDBList();
                foreach(var item in items)
                {
                    var itemType = item.GetType();
                    var entry = Conversion.ConvertToEntry(itemType, item);
                    l.Add(entry);
                }
                return true;
            }

            l = null;
            return false;
        }

        public override bool TryFrom(DynamoDBList l, Type targetType, out object result)
        {
            var elementType = Utils.GetElementType(targetType);
            var entries = l.Entries;
            return EntriesToCollection(targetType, elementType, entries, out result);
        }
    }

}
