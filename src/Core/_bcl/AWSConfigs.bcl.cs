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
 *
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Amazon.Runtime.Internal.Util;
using Amazon.Util;
using System.Xml;
using System.Reflection;
using System.Text;

namespace Amazon
{
    /// <summary>
    /// Configuration options that apply to the entire SDK.
    /// </summary>
    public static partial class AWSConfigs
    {
        #region ApplicationName

        /// <summary>
        /// The unique application name for the current application. This values is currently used 
        /// by high level APIs (Mobile Analytics Manager and Cognito Sync Manager) to create a unique file
        /// path to store local database files.
        /// Changes to this setting will only take effect in newly-constructed objects using this property.
        /// <code>
        /// &lt;configSections&gt;
        ///   &lt;section name="aws" type="Amazon.AWSSection, AWSSDK"/&gt;
        /// &lt;/configSections&gt;
        /// &lt;aws applicationName="" /&gt;
        /// </code>
        /// </summary>
        public static string ApplicationName
        {
            get { return _rootConfig.ApplicationName; }
            set { _rootConfig.ApplicationName = value; }
        }

        #endregion

        #region Config

        public static string GetConfig(string name)
        {
            NameValueCollection appConfig = ConfigurationManager.AppSettings;
            if (appConfig == null)
                return null;
            string value = appConfig[name];
            return value;
        }

        internal static T GetSection<T>(string sectionName)
            where T : class, new()
        {
            object section = ConfigurationManager.GetSection(sectionName);
            if (section == null)
                return new T();
            return section as T;
        }

        internal static bool XmlSectionExists(string sectionName)
        {
            var section = ConfigurationManager.GetSection(sectionName);
            var element = section as System.Xml.XmlElement;
            return (element != null);
        }

        #endregion

        #region TraceListeners
        private static Dictionary<string, List<TraceListener>> _traceListeners
            = new Dictionary<string, List<TraceListener>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Add a listener for SDK logging. 
        /// </summary>
        /// <remarks>If the listener does not have a name, you will not be able to remove it later.</remarks>
        /// <param name="source">The source to log for, e.g. "Amazon", or "Amazon.DynamoDB".</param>
        /// <param name="listener">The listener to add.</param>
        public static void AddTraceListener(string source, TraceListener listener)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source cannot be null or empty", "source");
            if (null == listener)
                throw new ArgumentException("Listener cannot be null", "listener");

            lock (_traceListeners)
            {
                if (!_traceListeners.ContainsKey(source))
                    _traceListeners.Add(source, new List<TraceListener>());
                _traceListeners[source].Add(listener);
            }

            Logger.ClearLoggerCache();
        }

        /// <summary>
        /// Remove a trace listener from SDK logging.
        /// </summary>
        /// <param name="source">The source the listener was added to.</param>
        /// <param name="name">The name of the listener.</param>
        public static void RemoveTraceListener(string source, string name)
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentException("Source cannot be null or empty", "source");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name cannot be null or empty", "name");

            lock (_traceListeners)
            {
                if (_traceListeners.ContainsKey(source))
                {
                    foreach (var l in _traceListeners[source])
                    {
                        if (l.Name.Equals(name, StringComparison.Ordinal))
                        {
                            _traceListeners[source].Remove(l);
                            break;
                        }
                    }
                }
            }

            Logger.ClearLoggerCache();
        }

        // Used by Logger.Diagnostic to add listeners to TraceSources when loggers 
        // are created.
        internal static TraceListener[] TraceListeners(string source)
        {
            lock (_traceListeners)
            {
                List<TraceListener> temp;

                if (_traceListeners.TryGetValue(source, out temp))
                {
                    return temp.ToArray();
                }

                return new TraceListener[0];
            }
        }

        #endregion

        #region Generate Config Template

        /// <summary>
        /// Generates a sample XML representation of the SDK configuration section.
        /// </summary>
        /// <remarks>
        /// The XML returned has an example of every tag in the SDK configuration
        /// section, and sample input for each attribute. This can be included in 
        /// an App.config or Web.config and edited to suit. Where a section contains
        /// a collection, multiple of the same tag will be output.
        /// </remarks>
        /// <returns>Sample XML configuration string.</returns>
        public static string GenerateConfigTemplate()
        {
            Assembly a = typeof(AWSConfigs).Assembly;
            Type t = a.GetType("Amazon.AWSSection");

            var xmlSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                IndentChars = "    ",
                NewLineOnAttributes = true,
                NewLineChars = "\n"
            };

            var sb = new StringBuilder();

            using (var xml = XmlWriter.Create(sb, xmlSettings))
            {
                FormatConfigSection(t, xml, tag: "aws");
            }

            return sb.ToString();
        }

        private static void FormatConfigSection(Type section, XmlWriter xml, string tag = null)
        {
            var props = section.GetProperties(
                BindingFlags.Instance |
                BindingFlags.Public |
                BindingFlags.DeclaredOnly);

            var attrs = props.Where(p => !IsConfigurationElement(p)).ToList();
            var subsections = props.Where(p => IsConfigurationElement(p)
                && !IsConfigurationElementCollection(p)).ToList();
            var collections = props.Where(p => IsConfigurationElementCollection(p)).ToList();

            xml.WriteStartElement(tag);
            foreach (var prop in attrs)
            {
                var name = ConfigurationPropertyName(prop);
                xml.WriteAttributeString(name, GetExampleForType(prop));
            }

            foreach (var prop in subsections)
            {
                var sectionName = ConfigurationPropertyName(prop);
                if (!string.IsNullOrEmpty(sectionName))
                    FormatConfigSection(prop.PropertyType, xml, sectionName);
            }

            foreach (var coll in collections)
            {
                FormatConfigurationListItems(coll, xml);
            }

            xml.WriteEndElement();
        }

        private static string GetExampleForType(PropertyInfo prop)
        {
            if (prop.PropertyType.Equals(typeof(bool?)))
                return "true | false";
            if (prop.PropertyType.Equals(typeof(int?)))
                return "1234";
            if (prop.PropertyType.Equals(typeof(String)))
                return "string value";
            if (prop.PropertyType.Equals(typeof(Type)))
                return "NameSpace.Class, Assembly";

            if (prop.PropertyType.IsEnum)
            {
                var members = Enum.GetNames(prop.PropertyType);
                var separator = IsFlagsEnum(prop) ? ", " : " | ";
                return string.Join(separator, members.ToArray());
            }

            return "( " + prop.PropertyType.FullName + " )";
        }

        private static void FormatConfigurationListItems(PropertyInfo section, XmlWriter xml)
        {
            var sectionName = ConfigurationPropertyName(section);
            var itemType = TypeOfConfigurationCollectionItem(section);

            var item = Activator.CreateInstance(section.PropertyType);
            var nameProperty = section.PropertyType.GetProperty("ItemPropertyName",
                                    BindingFlags.NonPublic | BindingFlags.Instance);
            var itemTagName = nameProperty.GetValue(item, null).ToString();

            FormatConfigSection(itemType, xml, itemTagName);
            FormatConfigSection(itemType, xml, itemTagName);
        }

        private static bool IsFlagsEnum(PropertyInfo prop)
        {
            return prop.PropertyType.GetCustomAttributes(typeof(FlagsAttribute), false).Any();
        }

        private static bool IsConfigurationElement(PropertyInfo prop)
        {
            return typeof(ConfigurationElement).IsAssignableFrom(prop.PropertyType);
        }

        private static bool IsConfigurationElementCollection(PropertyInfo prop)
        {
            return typeof(ConfigurationElementCollection).IsAssignableFrom(prop.PropertyType);
        }

        private static Type TypeOfConfigurationCollectionItem(PropertyInfo prop)
        {
            var configCollAttr = prop.PropertyType
                .GetCustomAttributes(typeof(ConfigurationCollectionAttribute), false)
                .First();
            return ((ConfigurationCollectionAttribute)configCollAttr).ItemType;
        }

        private static string ConfigurationPropertyName(PropertyInfo prop)
        {
            var configAttr = prop.GetCustomAttributes(typeof(ConfigurationPropertyAttribute), false)
                .FirstOrDefault() as ConfigurationPropertyAttribute;

            return null == configAttr ? prop.Name : configAttr.Name;
        }

        #endregion
    }
}
