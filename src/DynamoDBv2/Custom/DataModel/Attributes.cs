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
using System.Linq;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Base DynamoDB attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, Inherited=true, AllowMultiple=false)]
    public abstract class DynamoDBAttribute : Attribute
    {
    }


    /// <summary>
    /// DynamoDB attribute that marks a class.
    /// Specifies that this object can be stored in DynamoDB, the name
    /// of the target table, and if attribute names must be automatically
    /// converted to lowerCamelCase.
    /// 
    /// Need not be declared on subclasses if present on base class.
    /// Can be defined on subclasses to specify different target table
    /// or specify different attribute casing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited=true, AllowMultiple=false)]
    public sealed class DynamoDBTableAttribute : DynamoDBAttribute
    {
        /// <summary>
        /// Gets and sets the TableName property.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets and sets the LowerCamelCaseProperties property.
        /// </summary>
        public bool LowerCamelCaseProperties { get; set; }

        /// <summary>
        /// Construct an instance of DynamoDBTableAttribute
        /// </summary>
        /// <param name="tableName"></param>
        public DynamoDBTableAttribute(string tableName)
            : this(tableName, false) { }

        /// <summary>
        /// Construct an instance of DynamoDBTableAttribute
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="lowerCamelCaseProperties"></param>
        public DynamoDBTableAttribute(string tableName, bool lowerCamelCaseProperties)
        {
            TableName = tableName;
            LowerCamelCaseProperties = lowerCamelCaseProperties;
        }
    }

    /// <summary>
    /// DynamoDB attribute that directs the specified attribute not to
    /// be included when saving or loading objects.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DynamoDBIgnoreAttribute : DynamoDBAttribute
    {
    }


    /// <summary>
    /// DynamoDB property attribute.
    /// Can be used to specify an alternative attribute name or configure
    /// a custom converter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public abstract class DynamoDBRenamableAttribute : DynamoDBAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        protected DynamoDBRenamableAttribute()
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        protected DynamoDBRenamableAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        /// <summary>
        /// Name of attribute that is associated with this member.
        /// </summary>
        public string AttributeName { get; set; }
    }


    /// <summary>
    /// DynamoDB property that marks up current member as item version.
    /// At most one member in a class may be marked with this attribute.
    /// 
    /// Members that are marked as version must be of primitive,
    /// numeric, integer, nullable type (e.g. int?, long?, byte?)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DynamoDBVersionAttribute : DynamoDBRenamableAttribute
    {
    }


    /// <summary>
    /// DynamoDB property attribute.
    /// Can be used to specify an alternative attribute name or configure
    /// a custom converter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DynamoDBPropertyAttribute : DynamoDBRenamableAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamoDBPropertyAttribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        public DynamoDBPropertyAttribute(string attributeName)
            : base(attributeName)
        {
        }

        /// <summary>
        /// Constructor that specifies a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBPropertyAttribute(Type converter)
        {
            Converter = converter;
        }

        /// <summary>
        /// Constructor that specifies that this type should be stored as epoch seconds.
        /// </summary>
        /// <param name="storeAsEpoch">
        /// Whether the data should be stored as epoch seconds.
        /// If false, data is stored as ISO-8601 string.
        /// </param>
        public DynamoDBPropertyAttribute(bool storeAsEpoch)
        {
            StoreAsEpoch = storeAsEpoch;
        }


        /// <summary>
        /// Constructor that specifies an alternate attribute name and a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBPropertyAttribute(string attributeName, Type converter)
            : base(attributeName)
        {
            Converter = converter;
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name and that this type should
        /// be stored as epoch seconds.
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        /// <param name="storeAsEpoch">
        /// Whether the data should be stored as epoch seconds.
        /// If false, data is stored as ISO-8601 string.
        /// </param>
        public DynamoDBPropertyAttribute(string attributeName, bool storeAsEpoch)
            : base(attributeName)
        {
            StoreAsEpoch = storeAsEpoch;
        }

        /// <summary>
        /// Type of the custom converter.
        /// Cannot be set at the same time as StoreAsEpoch.
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// Flag that directs DynamoDBContext to store this data as epoch seconds integer.
        /// If false, data is stored as ISO-8601 string.
        /// Cannot be set at the same time as Converter.
        /// </summary>
        public bool StoreAsEpoch { get; set; }
    }

    /// <summary>
    /// DynamoDB property that marks up current member as a hash key element.
    /// Exactly one member in a class must be marked with this attribute.
    /// 
    /// Members that are marked as hash key must be convertible to
    /// a Primitive object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DynamoDBHashKeyAttribute : DynamoDBPropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamoDBHashKeyAttribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        public DynamoDBHashKeyAttribute(string attributeName)
            : base(attributeName)
        {
        }

        /// <summary>
        /// Constructor that specifies a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBHashKeyAttribute(Type converter)
            : base(converter)
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name and a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBHashKeyAttribute(string attributeName, Type converter)
            : base(attributeName, converter)
        {
        }
    }

    /// <summary>
    /// DynamoDB property that marks up current member as range key element (for a hash-and-range primary key).
    /// At most one member in a class may be marked with this attribute.
    /// 
    /// Members that are marked as a range key element must be convertible to
    /// a Primitive object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DynamoDBRangeKeyAttribute : DynamoDBPropertyAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DynamoDBRangeKeyAttribute()
            : base()
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        public DynamoDBRangeKeyAttribute(string attributeName)
            : base(attributeName)
        {
        }

        /// <summary>
        /// Constructor that specifies a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBRangeKeyAttribute(Type converter)
            : base(converter)
        {
        }

        /// <summary>
        /// Constructor that specifies an alternate attribute name and a custom converter.
        /// 
        /// Converter must be the type of a class that implements IPropertyConverter.
        /// </summary>
        /// <param name="attributeName">
        /// Name of attribute to be associated with property or field.
        /// </param>
        /// <param name="converter">Custom converter type.</param>
        public DynamoDBRangeKeyAttribute(string attributeName, Type converter)
            : base(attributeName, converter)
        {
        }
    }


    /// DynamoDB property attribute that marks up current member as a hash key element for a Global Secondary Index on a table.
    /// 
    /// Members that are marked as a Global Secondary Index hash key element must be convertible to a Primitive object.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DynamoDBGlobalSecondaryIndexHashKeyAttribute : DynamoDBHashKeyAttribute
    {
        /// <summary>
        /// Index associated with this range key
        /// </summary>
        public string[] IndexNames { get; set; }

        /// <summary>
        /// Constructor that accepts a single inde name.
        /// </summary>
        /// <param name="indexName">Name of the Local Secondary Index this range key belongs to.</param>
        public DynamoDBGlobalSecondaryIndexHashKeyAttribute(string indexName)
            : base()
        {
            IndexNames = new string[] { indexName };
        }

        /// <summary>
        /// Constructor that accepts multiple index names.
        /// </summary>
        /// <param name="indexNames">Names of the Local Secondary Indexes this range key belongs to.</param>
        public DynamoDBGlobalSecondaryIndexHashKeyAttribute(params string[] indexNames)
            : base()
        {
            IndexNames = indexNames.Distinct(StringComparer.Ordinal).ToArray();
        }
    }

    /// DynamoDB property attribute that marks up current member as range key element for a Global Secondary Index on a table.
    /// 
    /// Members that are marked as a Global Secondary Index range key element must be convertible to a Primitive object.
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class DynamoDBGlobalSecondaryIndexRangeKeyAttribute : DynamoDBRangeKeyAttribute
    {
        /// <summary>
        /// Index associated with this range key
        /// </summary>
        public string[] IndexNames { get; set; }

        /// <summary>
        /// Constructor that accepts a single inde name.
        /// </summary>
        /// <param name="indexName">Name of the Local Secondary Index this range key belongs to.</param>
        public DynamoDBGlobalSecondaryIndexRangeKeyAttribute(string indexName)
            : base()
        {
            IndexNames = new string[] { indexName };
        }

        /// <summary>
        /// Constructor that accepts multiple index names.
        /// </summary>
        /// <param name="indexNames">Names of the Local Secondary Indexes this range key belongs to.</param>
        public DynamoDBGlobalSecondaryIndexRangeKeyAttribute(params string[] indexNames)
            : base()
        {
            IndexNames = indexNames.Distinct(StringComparer.Ordinal).ToArray();
        }
    }


    /// <summary>
    /// DynamoDB property that marks up current member as range key element for a Local Secondary Index on a table.
    /// 
    /// Members that are marked as a Local Secondary Index range key element must be convertible to a Primitive object.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DynamoDBLocalSecondaryIndexRangeKeyAttribute : DynamoDBPropertyAttribute
    {
        /// <summary>
        /// Index associated with this range key
        /// </summary>
        public string[] IndexNames { get; set; }

        /// <summary>
        /// Constructor that accepts a single inde name.
        /// </summary>
        /// <param name="indexName">Name of the Local Secondary Index this range key belongs to.</param>
        public DynamoDBLocalSecondaryIndexRangeKeyAttribute(string indexName)
            : base()
        {
            IndexNames = new string[] { indexName };
        }

        /// <summary>
        /// Constructor that accepts multiple index names.
        /// </summary>
        /// <param name="indexNames">Names of the Local Secondary Indexes this range key belongs to.</param>
        public DynamoDBLocalSecondaryIndexRangeKeyAttribute(params string[] indexNames)
            : base()
        {
            IndexNames = indexNames.Distinct(StringComparer.Ordinal).ToArray();
        }
    }
}
