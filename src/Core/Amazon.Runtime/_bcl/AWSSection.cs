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
using System.Collections.Specialized;
using System.ComponentModel;

using System.Configuration;

using Amazon.Util;
using System.Xml;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace Amazon
{
    /// <summary>
    /// Root AWS config section
    /// </summary>
    internal class AWSSection : ConfigurationSection
    {
        private const string loggingKey = "logging";
        private const string defaultsKey = "defaults";
        private const string endpointDefinitionKey = "endpointDefinition";
        private const string regionKey = "region";
        private const string proxy = "proxy";
        private const string profileNameKey = "profileName";
        private const string profilesLocationKey = "profilesLocation";
        private const string correctForClockSkewKey = "correctForClockSkew";
        private const string useSdkCacheKey = "useSdkCache";
        private const string applicationNameKey = "applicationName";
        private const string csmConfigKey = "csmConfig";

        [ConfigurationProperty(loggingKey)]
        public LoggingSection Logging
        {
            get { return (LoggingSection)this[loggingKey]; }
            set { this[loggingKey] = value; }
        }

        [ConfigurationProperty(csmConfigKey)]
        public CSMSection CSMConfig
        {
            get { return (CSMSection)this[csmConfigKey]; }
            set { this[csmConfigKey] = value; }
        }

        [ConfigurationProperty(endpointDefinitionKey)]
        public string EndpointDefinition
        {
            get { return (string)this[endpointDefinitionKey]; }
            set { this[endpointDefinitionKey] = value; }
        }

        [ConfigurationProperty(regionKey)]
        public string Region
        {
            get { return (string)this[regionKey]; }
            set { this[regionKey] = value; }
        }

        [ConfigurationProperty(profileNameKey)]
        public string ProfileName
        {
            get { return (string)this[profileNameKey]; }
            set { this[profileNameKey] = value; }
        }

        [ConfigurationProperty(profilesLocationKey)]
        public string ProfilesLocation
        {
            get { return (string)this[profilesLocationKey]; }
            set { this[profilesLocationKey] = value; }
        }

        [ConfigurationProperty(useSdkCacheKey)]
        public bool? UseSdkCache
        {
            get { return (bool?)this[useSdkCacheKey]; }
            set { this[useSdkCacheKey] = value; }
        }

        [ConfigurationProperty(correctForClockSkewKey)]
        public bool? CorrectForClockSkew
        {
            get { return (bool?)this[correctForClockSkewKey]; }
            set { this[correctForClockSkewKey] = value; }
        }

        [ConfigurationProperty(applicationNameKey)]
        public string ApplicationName
        {
            get { return (string)this[applicationNameKey]; }
            set { this[applicationNameKey] = value; }
        }

        /// <summary>
        /// Gets and sets the proxy settings for the SDK to use.
        /// </summary>
        [ConfigurationProperty(proxy)]
        public ProxySection Proxy
        {
            get { return (ProxySection)this[proxy]; }
            set { this[proxy] = value; }
        }

        private IDictionary<string, XElement> _serviceSections = null;
        public IDictionary<string, XElement> ServiceSections
        {
            get
            {
                if (_serviceSections == null)
                    _serviceSections = new Dictionary<string, XElement>(StringComparer.Ordinal);
                return _serviceSections;
            }
            set
            {
                _serviceSections = value;
            }
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            var section = (XElement)XElement.ReadFrom(reader);
            ServiceSections[elementName] = section;
            return true;
        }

        internal string Serialize(string name)
        {
            try
            {
                string basicXml = this.SerializeSection(null, name, ConfigurationSaveMode.Full);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(basicXml);
                using (var textWriter = new StringWriter(CultureInfo.InvariantCulture))
                {
                    CleanUpNodes(doc);

                    doc.Save(textWriter);
                    return textWriter.ToString();
                }
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
        internal static void CleanUpNodes(XmlNode node)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                var toRemove = child.Attributes.Cast<XmlAttribute>().Where(at => string.IsNullOrEmpty(at.Value)).ToList();
                foreach (var rem in toRemove) child.Attributes.Remove(rem);

                CleanUpNodes(child);
            }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    #region Basic sections

    /// <summary>
    /// Settings for configuring a proxy for the SDK to use.
    /// </summary>
    internal class ProxySection : WritableConfigurationElement
    {
        private const string hostSectionKey = "host";
        private const string portSectionKey = "port";
        private const string usernameSectionKey = "username";
        private const string passwordSectionKey = "password";
        private const string bypasslistSectionKey = "bypassList";
        private const string bypassOnLocalSectionKey = "bypassOnLocal";
        private const char bypasslistSectionSeparator = ';';

        /// <summary>
        /// Gets and sets the host name or IP address of the proxy server.
        /// </summary>
        [ConfigurationProperty(hostSectionKey)]
        public string Host
        {
            get { return (string)this[hostSectionKey]; }
            set { this[hostSectionKey] = value; }
        }

        /// <summary>
        /// Gets and sets the port number of the proxy.
        /// </summary>
        [ConfigurationProperty(portSectionKey)]
        public int? Port
        {
            get { return (int?)this[portSectionKey]; }
            set { this[portSectionKey] = value; }
        }

        /// <summary>
        /// Gets and sets the username to authenticate with the proxy server.
        /// </summary>
        [ConfigurationProperty(usernameSectionKey)]
        public string Username
        {
            get { return (string)this[usernameSectionKey]; }
            set { this[usernameSectionKey] = value; }
        }

        /// <summary>
        /// Gets and sets the password to authenticate with the proxy server.
        /// </summary>
        [ConfigurationProperty(passwordSectionKey)]
        public string Password
        {
            get { return (string)this[passwordSectionKey]; }
            set { this[passwordSectionKey] = value; }
        }

        /// <summary>
        /// Gets and set the proxy bypass list. A set of semi-colon
        /// delimited regular expressions denoting the addresses that
        /// should bypass the proxy.
        /// </summary>
        [ConfigurationProperty(bypasslistSectionKey)]
        public string[] BypassList
        {
            get 
            {
                var bypassList = (string)this[bypasslistSectionKey];
                if (string.IsNullOrEmpty(bypassList))
                    return new string[0];

                return bypassList.Split(new char[] { bypasslistSectionSeparator },
                                        StringSplitOptions.RemoveEmptyEntries);
            }
            set 
            {
                this[bypasslistSectionKey] = value == null ? null 
                                                        : string.Join(bypasslistSectionSeparator.ToString(), value);
            }
        }

        /// <summary>
        /// Gets and sets the proxy option to bypass local addresses.
        /// </summary>
        [ConfigurationProperty(bypassOnLocalSectionKey)]
        public bool? BypassOnLocal
        {
            get { return (bool?)this[bypassOnLocalSectionKey]; }
            set { this[bypassOnLocalSectionKey] = value; }
        }
    }

    /// <summary>
    /// Logging section
    /// </summary>
    internal class LoggingSection : WritableConfigurationElement
    {
        private const string logToKey = "logTo";
        private const string logResponsesKey = "logResponses";
        private const string logResponsesSizeLimitKey = "logResponsesSizeLimit";
        private const string logMetricsKey = "logMetrics";
        private const string logMetricsFormatKey = "logMetricsFormat";
        private const string logMetricsCustomFormatterKey = "logMetricsCustomFormatter";

        [ConfigurationProperty(logToKey)]
        public LoggingOptions LogTo
        {
            get { return (LoggingOptions)this[logToKey]; }
            set { this[logToKey] = value; }
        }

        [ConfigurationProperty(logResponsesKey)]
        public ResponseLoggingOption LogResponses
        {
            get { return (ResponseLoggingOption)this[logResponsesKey]; }
            set { this[logResponsesKey] = value; }
        }

        [ConfigurationProperty(logResponsesSizeLimitKey)]
        public int? LogResponsesSizeLimit
        {
            get { return (int?)this[logResponsesSizeLimitKey]; }
            set { this[logResponsesKey] = value; }
        }

        [ConfigurationProperty(logMetricsKey)]
        public bool? LogMetrics
        {
            get { return (bool?)this[logMetricsKey]; }
            set { this[logMetricsKey] = value; }
        }

        [ConfigurationProperty(logMetricsFormatKey)]
        public LogMetricsFormatOption LogMetricsFormat
        {
            get { return (LogMetricsFormatOption)this[logMetricsFormatKey]; }
            set { this[logMetricsFormatKey] = value; }
        }

        [TypeConverter(typeof(TypeNameConverter))]
        [ConfigurationProperty(logMetricsCustomFormatterKey)]
        public Type LogMetricsCustomFormatter
        {
            get { return (Type)this[logMetricsCustomFormatterKey]; }
            set { this[logMetricsCustomFormatterKey] = value; }
        }
    }

    internal class CSMSection : WritableConfigurationElement
    {
        private const string csmEnabledKey = "csmEnabled";
        private const string csmClientIdKey = "csmClientId";
        private const string csmPortKey = "csmPort";

        [ConfigurationProperty(csmEnabledKey)]
        public bool? CSMEnabled
        {
            get { return (bool)this[csmEnabledKey]; }
            set { this[csmEnabledKey] = value; }
        }

        [ConfigurationProperty(csmPortKey)]
        public int? CSMPort
        {
            get { return (int?)this[csmPortKey]; }
            set { this[csmPortKey] = value; }
        }

        [ConfigurationProperty(csmClientIdKey)]
        public string CSMClientId
        {
            get { return (string)this[csmClientIdKey]; }
            set { this[csmClientIdKey] = value; }
        }
    }

    #endregion

    #region Abstract helper classes

    /// <summary>
    /// Easy-to-use generic collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class WritableConfigurationElementCollection<T> : ConfigurationElementCollection
        where T : SerializableConfigurationElement, new()
    {
        protected abstract string ItemPropertyName { get; }

        protected override ConfigurationElement CreateNewElement()
        {
            return new T();
        }
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            return new T();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return element;
        }
        protected override string ElementName { get { return string.Empty; } }
        protected override bool IsElementName(string elementName)
        {
            return (string.Equals(elementName, ItemPropertyName, StringComparison.Ordinal));
        }
        public override ConfigurationElementCollectionType CollectionType { get { return ConfigurationElementCollectionType.BasicMapAlternate; } }
        public override bool IsReadOnly()
        {
            return false;
        }

        protected WritableConfigurationElementCollection() : base() { }

        public void Add(T t)
        {
            this.BaseAdd(t);
        }
        public void Add(T[] ts)
        {
            foreach (var t in ts)
            {
                Add(t);
            }
        }

        protected override bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
        {
            if (writer == null)
                return base.SerializeElement(writer, serializeCollectionKey);

            for (int i = 0; i < this.Count; i++)
            {
                var item = BaseGet(i) as SerializableConfigurationElement;
                writer.WriteStartElement(ItemPropertyName);
                item.SerializeElement(writer, serializeCollectionKey);
                writer.WriteEndElement();
            }

            return (Count > 0);
        }

        public List<T> Items { get { return this.Cast<T>().ToList(); } }
    }

    /// <summary>
    /// Configuration element that serializes properly when used with collections
    /// </summary>
    public abstract class SerializableConfigurationElement : WritableConfigurationElement
    {
        new public bool SerializeElement(XmlWriter writer, bool serializeCollectionKey)
        {
            return base.SerializeElement(writer, serializeCollectionKey);
        }
    }

    /// <summary>
    /// ConfigurationElement class which returns false for IsReadOnly
    /// </summary>
    public abstract class WritableConfigurationElement : ConfigurationElement
    {
        public override bool IsReadOnly()
        {
            return false;
        }
    }

    #endregion
}
