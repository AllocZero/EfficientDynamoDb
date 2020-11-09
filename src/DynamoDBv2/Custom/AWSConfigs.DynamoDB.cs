using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml.Linq;

#if BCL
using System.Xml;
using System.Configuration;
#endif

using Amazon.Util;
using Amazon.Util.Internal;
using Amazon.DynamoDBv2;

namespace Amazon
{
    /// <summary>
    /// Configurations for accessing DynamoDB
    /// </summary>
    public static class AWSConfigsDynamoDB
    {
        private const string dynamoDBKey = "dynamoDB";

        static AWSConfigsDynamoDB()
        {
            try
            {
#if BCL
                var root = new RootConfig();
                var section = root.GetServiceSection(dynamoDBKey);
                if (section == null)
                    return;

                var rootSection = new DynamoDBSectionRoot(section);
                if (rootSection.DynamoDB != null)
                    AWSConfigsDynamoDB.Configure(rootSection.DynamoDB);
#endif
            }
            finally
            {
                // If no configuration exist at least
                // configure the context config to the default.
                if (Context == null)
                {
                    Context = new DynamoDBContextConfig();
                    ConversionSchema = ConversionSchema.V1;;
                }
            }

        }

        #region DynamoDBContext TableNamePrefix


        /// <summary>
        /// Key for the DynamoDBContextTableNamePrefix property.
        /// <seealso cref="AWSConfigsDynamoDB.DynamoDBContextTableNamePrefix"/>
        /// </summary>
        public const string DynamoDBContextTableNamePrefixKey = "AWS.DynamoDBContext.TableNamePrefix";

        /// <summary>
        /// Configures the default TableNamePrefix that the DynamoDBContext will use if
        /// not manually configured.
        /// Changes to this setting will only take effect in newly-constructed instances of
        /// DynamoDBContextConfig and DynamoDBContext.
        /// 
        /// The setting can be configured through App.config, for example:
        /// <code>
        /// &lt;appSettings&gt;
        ///   &lt;add key="AWS.DynamoDBContext.TableNamePrefix" value="Test-"/&gt;
        /// &lt;/appSettings&gt;
        /// </code>
        /// </summary>
        [Obsolete("This property is obsolete. Use DynamoDBConfig.Context.TableNamePrefix instead.")]
        public static string DynamoDBContextTableNamePrefix
        {
            get { return Context.TableNamePrefix; }
            set { Context.TableNamePrefix = value; }
        }

        #endregion

        /// <summary>
        /// Conversion schema to use for converting .NET types to DynamoDB types.
        /// </summary>
        internal static ConversionSchema ConversionSchema { get; set; }

        /// <summary>
        /// Settings for DynamoDBContext.
        /// </summary>
        public static DynamoDBContextConfig Context { get; private set; }

#if BCL
        internal static void Configure(DynamoDBSection section)
        {
            Context = new DynamoDBContextConfig();
            ConversionSchema = ConversionSchema.V1;

            if (section != null && section.ElementInformation.IsPresent)
            {
                ConversionSchema = section.ConversionSchema;
                if(section.Context != null)
                    Context.Configure(section.Context);
            }
        }

#endif  
    }
}

namespace Amazon.Util
{
    #region DynamoDB Config

    /// <summary>
    /// Settings for DynamoDBContext.
    /// </summary>
    public partial class DynamoDBContextConfig
    {
        /// <summary>
        /// Configures the default TableNamePrefix that the DynamoDBContext will use if
        /// not manually configured.
        /// 
        /// TableNamePrefix is used after TableAliases have been applied.
        /// </summary>
        public string TableNamePrefix { get; set; }

        /// <summary>
        /// A string-to-string dictionary (From-Table to To-Table) used by DynamoDBContext to
        /// use a different table from one that is configured for a type.
        /// 
        /// Remapping is done before applying TableNamePrefix.
        /// </summary>
        public Dictionary<string, string> TableAliases { get; private set; }

        /// <summary>
        /// A Type-to-TypeMapping (type to TypeMapping defining its DynamoDB conversion) used by
        /// DynamoDBContext to modify or configure a particular type.
        /// </summary>
        public Dictionary<Type, TypeMapping> TypeMappings { get; private set; }

        /// <summary>
        /// Adds a TableAlias to the TableAliases property.
        /// An exception is thrown if there is already a TableAlias with the same FromTable configured.
        /// </summary>
        /// <param name="tableAlias"></param>
        public void AddAlias(TableAlias tableAlias)
        {
            TableAliases.Add(tableAlias.FromTable, tableAlias.ToTable);
        }

        /// <summary>
        /// Adds a TypeMapping to the TypeMappings property.
        /// An exception is thrown if there is already a TypeMapping with the same Type configured.
        /// </summary>
        /// <param name="typeMapping"></param>
        public void AddMapping(TypeMapping typeMapping)
        {
            TypeMappings.Add(typeMapping.Type, typeMapping);
        }

        internal DynamoDBContextConfig()
        {
            this.TableNamePrefix = AWSConfigs.GetConfig(AWSConfigsDynamoDB.DynamoDBContextTableNamePrefixKey);

            TableAliases = new Dictionary<string, string>(StringComparer.Ordinal);
            TypeMappings = new Dictionary<Type, TypeMapping>();
        }

#if BCL
        internal void Configure(DynamoDBContextSection section)
        {
            if (section != null && section.ElementInformation.IsPresent)
            {
                TableNamePrefix = section.TableNamePrefix;

                InternalSDKUtils.FillDictionary(section.TypeMappings.Items, t => t.Type, t => new TypeMapping(t), TypeMappings);
                InternalSDKUtils.FillDictionary(section.TableAliases.Items, t => t.FromTable, t => t.ToTable, TableAliases);
            }
        }
#endif
    }

    /// <summary>
    /// Single DynamoDB table alias
    /// </summary>
    public partial class TableAlias
    {
        /// <summary>
        /// Source table
        /// </summary>
        public string FromTable { get; set; }

        /// <summary>
        /// Destination table
        /// </summary>
        public string ToTable { get; set; }

        /// <summary>
        /// Initializes an empty TableAlias object
        /// </summary>
        public TableAlias() { }

        /// <summary>
        /// Initializes a TableAlias object with specific source and
        /// destination tables
        /// </summary>
        /// <param name="fromTable">Source table</param>
        /// <param name="toTable">Destination table</param>
        public TableAlias(string fromTable, string toTable)
        {
            FromTable = fromTable;
            ToTable = toTable;
        }
    }

    /// <summary>
    /// Single DynamoDB type mapping config
    /// </summary>
    public partial class TypeMapping
    {
        /// <summary>
        /// Type to which the mapping applies
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// Target table for the type
        /// </summary>
        public string TargetTable { get; set; }

        /// <summary>
        /// A string-to-PropertyConfig dictionary (property name to PropertyConfig) describing
        /// how each property on the type should be treated.
        /// </summary>
        public Dictionary<string, PropertyConfig> PropertyConfigs { get; private set; }

        /// <summary>
        /// Adds a PropertyConfig to the PropertyConfigs property.
        /// An exception is thrown if there is already a PropertyConfig with the same Name configured.
        /// </summary>
        /// <param name="propertyConfig"></param>
        public void AddProperty(PropertyConfig propertyConfig)
        {
            PropertyConfigs.Add(propertyConfig.Name, propertyConfig);
        }

        /// <summary>
        /// Initializes a TypeMapping object for a specific type and target table.
        /// </summary>
        /// <param name="type">Target type</param>
        /// <param name="targetTable">Target table</param>
        public TypeMapping(Type type, string targetTable)
        {
            Type = type;
            TargetTable = targetTable;
            PropertyConfigs = new Dictionary<string, PropertyConfig>(StringComparer.Ordinal);
        }

#if BCL
        internal TypeMapping(TypeMappingElement mapping)
            : this(mapping.Type, mapping.TargetTable)
        {
            InternalSDKUtils.FillDictionary(mapping.PropertyConfigs.Items, p => p.Name, p => new PropertyConfig(p), PropertyConfigs);
        }
#endif
    }

    /// <summary>
    /// Single DynamoDB property mapping config
    /// </summary>
    public partial class PropertyConfig
    {
        /// <summary>
        /// Property name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Attribute name
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Whether this property should be ignored by DynamoDBContext
        /// </summary>
        public bool Ignore { get; set; }

        /// <summary>
        /// Whether this property should be treated as a version property
        /// </summary>
        public bool Version { get; set; }

        /// <summary>
        /// The type of converter that should be used on this property
        /// </summary>
        public Type Converter { get; set; }

        /// <summary>
        /// Whether this property should be stored as epoch seconds integer.
        /// </summary>
        public bool StoreAsEpoch { get; set; }

        /// <summary>
        /// Initializes a PropertyConfig object for a specific property
        /// </summary>
        /// <param name="propertyName"></param>
        public PropertyConfig(string propertyName)
        {
            Name = propertyName;
        }

#if BCL
        internal PropertyConfig(PropertyConfigElement prop)
            : this(prop.Name)
        {
            Attribute = prop.Attribute;
            Ignore = prop.Ignore.GetValueOrDefault(false);
            Version = prop.Version.GetValueOrDefault(false);
            Converter = prop.Converter;
            StoreAsEpoch = prop.StoreAsEpoch.GetValueOrDefault(false);
        }
#endif
    }

    #endregion

    #region DynamoDB sections

#if BCL

    internal class DynamoDBSectionRoot : WritableConfigurationElement
    {
        private const string dynamoDBKey = "dynamoDB";

        public DynamoDBSectionRoot(XElement section)
        {
            if (section != null)
            {
                XmlTextReader reader = new XmlTextReader(new StringReader(section.ToString()))
                {
                    WhitespaceHandling = WhitespaceHandling.None
                };
                this.DeserializeElement(reader, false);
            }
        }

        [ConfigurationProperty(dynamoDBKey)]
        public DynamoDBSection DynamoDB
        {
            get { return (DynamoDBSection)this[dynamoDBKey]; }
            set { this[dynamoDBKey] = value; }
        }
    }
    /// <summary>
    /// Root DynamoDB section
    /// </summary>
    internal class DynamoDBSection : WritableConfigurationElement
    {
        private const string contextKey = "dynamoDBContext";
        private const string conversionKey = "conversionSchema";

        public DynamoDBSection()
        {

        }

        [ConfigurationProperty(contextKey)]
        public DynamoDBContextSection Context
        {
            get { return (DynamoDBContextSection)this[contextKey]; }
            set { this[contextKey] = value; }
        }

        [ConfigurationProperty(conversionKey)]
        public ConversionSchema ConversionSchema
        {
            get { return (ConversionSchema)this[conversionKey]; }
            set { this[conversionKey] = value; }
        }

    }

    /// <summary>
    /// DynamoDBContext section
    /// </summary>
    internal class DynamoDBContextSection : WritableConfigurationElement
    {

        private const string tableNamePrefixKey = "tableNamePrefix";
        private const string tableAliasesKey = "tableAliases";
        private const string mappingsKey = "mappings";

        [ConfigurationProperty(tableNamePrefixKey)]
        public string TableNamePrefix
        {
            get { return (string)this[tableNamePrefixKey]; }
            set { this[tableNamePrefixKey] = value; }
        }

        [ConfigurationProperty(tableAliasesKey)]
        public TableAliasesCollection TableAliases
        {
            get { return (TableAliasesCollection)this[tableAliasesKey]; }
            set { this[tableAliasesKey] = value; }
        }

        [ConfigurationProperty(mappingsKey)]
        public TypeMappingsCollection TypeMappings
        {
            get { return (TypeMappingsCollection)this[mappingsKey]; }
            set { this[mappingsKey] = value; }
        }
    }

    /// <summary>
    /// Single DDB table alias
    /// </summary>
    internal class TableAliasElement : SerializableConfigurationElement
    {
        private const string fromTableKey = "fromTable";
        private const string toTableKey = "toTable";

        [ConfigurationProperty(fromTableKey)]
        public string FromTable
        {
            get { return (string)this[fromTableKey]; }
            set { this[fromTableKey] = value; }
        }

        [ConfigurationProperty(toTableKey, IsRequired = true)]
        public string ToTable
        {
            get { return (string)this[toTableKey]; }
            set { this[toTableKey] = value; }
        }

        public TableAliasElement() { }

        public TableAliasElement(string fromTable, string toTable)
        {
            FromTable = fromTable;
            ToTable = toTable;
        }
    }

    /// <summary>
    /// Collection of DDB table aliases
    /// </summary>
    [ConfigurationCollection(typeof(TableAliasElement))]
    internal class TableAliasesCollection : WritableConfigurationElementCollection<TableAliasElement>
    {
        protected override string ItemPropertyName { get { return "alias"; } }

        public TableAliasesCollection() : base() { }
        public TableAliasesCollection(params TableAliasElement[] remaps)
            : base()
        {
            if (remaps != null)
                Add(remaps);
        }
    }

    /// <summary>
    /// Single DDB type mapping config
    /// </summary>
    internal class TypeMappingElement : SerializableConfigurationElement
    {
        private const string typeKey = "type";
        private const string targetTableKey = "targetTable";
        private const string itemKey = "";

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(typeKey, IsRequired = true)]
        public Type Type
        {
            get { return (Type)this[typeKey]; }
            set { this[typeKey] = value; }
        }

        [ConfigurationProperty(targetTableKey, IsRequired = true)]
        public string TargetTable
        {
            get { return (string)this[targetTableKey]; }
            set { this[targetTableKey] = value; }
        }

        [ConfigurationProperty(itemKey, IsDefaultCollection = true)]
        public PropertyConfigsCollection PropertyConfigs
        {
            get { return (PropertyConfigsCollection)this[itemKey]; }
            set { this[itemKey] = value; }
        }
    }

    /// <summary>
    /// Collection of DDB type mapping configs
    /// </summary>
    [ConfigurationCollection(typeof(TypeMappingElement))]
    internal class TypeMappingsCollection : WritableConfigurationElementCollection<TypeMappingElement>
    {
        protected override string ItemPropertyName { get { return "map"; } }

        public TypeMappingsCollection() : base() { }
        public TypeMappingsCollection(params TypeMappingElement[] mappings)
            : base()
        {
            if (mappings != null)
                Add(mappings);
        }
    }

    /// <summary>
    /// Single DDB property mapping config
    /// </summary>
    internal class PropertyConfigElement : SerializableConfigurationElement
    {
        private const string nameKey = "name";
        private const string attributeKey = "attribute";
        private const string ignoreKey = "ignore";
        private const string versionKey = "version";
        private const string converterKey = "converter";
        private const string storeAsEpochKey = "storeAsEpoch";

        [ConfigurationProperty(nameKey, IsRequired = true)]
        public string Name
        {
            get { return (string)this[nameKey]; }
            set { this[nameKey] = value; }
        }

        [ConfigurationProperty(attributeKey)]
        public string Attribute
        {
            get { return (string)this[attributeKey]; }
            set { this[attributeKey] = value; }
        }

        [ConfigurationProperty(ignoreKey)]
        public bool? Ignore
        {
            get { return (bool?)this[ignoreKey]; }
            set { this[ignoreKey] = value; }
        }

        [ConfigurationProperty(versionKey)]
        public bool? Version
        {
            get { return (bool?)this[versionKey]; }
            set { this[versionKey] = value; }
        }

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(converterKey)]
        public Type Converter
        {
            get { return (Type)this[converterKey]; }
            set { this[converterKey] = value; }
        }

        [ConfigurationProperty(storeAsEpochKey)]
        public bool? StoreAsEpoch
        {
            get { return (bool?)this[storeAsEpochKey]; }
            set { this[storeAsEpochKey] = value; }
        }
    }

    /// <summary>
    /// Collection of DDB property mapping configs
    /// </summary>
    [ConfigurationCollection(typeof(PropertyConfigElement))]
    internal class PropertyConfigsCollection : WritableConfigurationElementCollection<PropertyConfigElement>
    {
        protected override string ItemPropertyName { get { return "property"; } }

        public PropertyConfigsCollection() : base() { }
        public PropertyConfigsCollection(params PropertyConfigElement[] configs)
            : base()
        {
            if (configs != null)
                Add(configs);
        }
    }
#endif
    #endregion

}
