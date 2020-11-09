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
using Amazon.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Amazon.DynamoDBv2
{
    /// <summary>
    /// Class that implements IPropertyConverter to convert collections
    /// to PrimitiveList types, which are then stored as sets (NS, SS, BS)
    /// in DynamoDB.
    /// 
    /// Use this converter to bypass the default schema behavior for a particular
    /// property.
    /// The Conversion property can be customized in the constructor of a subclass
    /// to use a different conversion scheme for converting individual elements.
    /// The default value for this field is the standard V1 conversion.
    /// </summary>
    public class SetPropertyConverter<TCollection, TElement> : IPropertyConverter
        where TCollection : ICollection<TElement>, new()
    {
        /// <summary>
        /// Reference to the type object for the TCollection generic.
        /// </summary>
        protected static readonly Type collectionType = typeof(TCollection);

        /// <summary>
        /// Reference to the type object for the TElement generic.
        /// </summary>
        protected static readonly Type elementType = typeof(TElement);

        /// <summary>
        /// Gets and sets the Conversion property.
        /// </summary>
        public DynamoDBEntryConversion Conversion { get; set; }

        /// <summary>
        /// Set the conversion to V1.
        /// </summary>
        public SetPropertyConverter()
        {
            Conversion = DynamoDBEntryConversion.V1;
        }

        /// <summary>
        /// Convert object to DynamoDBEntry using the V1 converter rules.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DynamoDBEntry ToEntry(object value)
        {
            if (value == null)
                return null;

            DynamoDBEntry entry = null;
            var items = value as IEnumerable;

            // In case U is passed in instead of ICollection<U>, convert to a Primitive
            if (value is TElement)
                entry = Conversion.ConvertToEntry(elementType, value).AsPrimitive();
            // Else, try to convert ICollection<U> to PrimitiveList
            else if (items != null)
                entry = Conversion.ItemsToPrimitiveList(items);

            if (entry == null)
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to convert value [{0}] of type {1} to PrimitiveList or Primitive", value, value.GetType().FullName));

            return entry;
        }

        /// <summary>
        /// Convert DyanmoDBEntry to object using the V1 converter rules.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public object FromEntry(DynamoDBEntry entry)
        {
            IEnumerable<object> items = null;

            var pl = entry as PrimitiveList;
            if (pl != null)
            {
                var primitives = pl.Entries;
                items = Conversion.ConvertFromEntries(elementType, primitives.Cast<DynamoDBEntry>());
            }

            var l = entry as DynamoDBList;
            if (l != null)
            {
                var entries = l.Entries;
                items = Conversion.ConvertFromEntries(elementType, entries);
            }

            object collection;
            if (items == null ||
                !Utils.ItemsToCollection(collectionType, items, out collection))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture,
                    "Unable to convert entry [{0}] of type {1} to {2}", entry, entry.GetType().FullName, collectionType.FullName));

            return collection;
        }
    }

    /// <summary>
    /// Class that implements IPropertyConverter to convert collections
    /// to PrimitiveList types, which are then stored as sets (NS, SS, BS)
    /// in DynamoDB.
    /// This converter assumes that the target is of type List{TElement}.
    /// 
    /// Use this converter to bypass the default schema behavior for a particular
    /// property.
    /// The Conversion property can be customized in the constructor of a subclass
    /// to use a different conversion scheme for converting individual elements.
    /// The default value for this field is the standard V1 conversion.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public class ListToSetPropertyConverter<TElement> : SetPropertyConverter<List<TElement>, TElement>
    { }

    /// <summary>
    /// Class that implements IPropertyConverter to convert bool/Boolean .NET
    /// types to DynamoDB numeric (N) type, where true is stored as N:1, false as N:0.
    /// 
    /// Use this converter to bypass the default schema behavior for a particular
    /// property.
    /// </summary>
    public class BoolAsNConverter : IPropertyConverter
    {
        private static BoolConverterV1 v1Converter = new BoolConverterV1();
        private static Type boolType = typeof(bool);

        /// <summary>
        /// Converts object to DynamoDBEntry
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public DynamoDBEntry ToEntry(object value)
        {
            return v1Converter.ToEntry(value);
        }

        /// <summary>
        /// Converts DynamoDBEntry to object
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        public object FromEntry(DynamoDBEntry entry)
        {
            return v1Converter.FromEntry(entry, boolType);
        }
    }
}
