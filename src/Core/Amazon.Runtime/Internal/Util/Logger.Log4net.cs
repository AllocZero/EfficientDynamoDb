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
using System.Collections.Specialized;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.ComponentModel;

using Amazon.Util.Internal;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Logger wrapper for reflected log4net logging methods.
    /// </summary>
    internal class InternalLog4netLogger : InternalLogger
    {
        enum LoadState { Uninitialized, Failed, Loading, Success };

        #region Reflected Types and methods

        static LoadState loadState = LoadState.Uninitialized;
        static readonly object LOCK = new object();

        static Type logMangerType;
        static ITypeInfo logMangerTypeInfo;
        static MethodInfo getLoggerWithTypeMethod;

        static Type logType;
        static ITypeInfo logTypeInfo;
        static MethodInfo logMethod;

        static Type levelType;
        static ITypeInfo levelTypeInfo;
        static object debugLevelPropertyValue;
        static object infoLevelPropertyValue;
        static object errorLevelPropertyValue;

        static MethodInfo isEnabledForMethod;
        
        static Type systemStringFormatType;
        static Type loggerType;

        #endregion

        private object internalLogger;
        private bool? isErrorEnabled;
        private bool? isDebugEnabled;
        private bool? isInfoEnabled;

        /// <summary>
        /// This should be a one time call to use reflection to find all the types and methods
        /// needed for the logging API.
        /// </summary>
        private static void loadStatics()
        {
            lock (InternalLog4netLogger.LOCK)
            {
                if (loadState != LoadState.Uninitialized)
                    return;

                loadState = LoadState.Loading;
                try
                {
                    loggerType = Type.GetType("Amazon.Runtime.Internal.Util.Logger");

                    // The LogManager and its methods
                    logMangerType = Type.GetType("log4net.Core.LoggerManager, log4net");
                    logMangerTypeInfo = TypeFactory.GetTypeInfo(logMangerType);
                    if (logMangerType == null)
                    {
                        loadState = LoadState.Failed;
                        return;
                    }

                    getLoggerWithTypeMethod = logMangerTypeInfo.GetMethod("GetLogger", new ITypeInfo[] { TypeFactory.GetTypeInfo(typeof(Assembly)), TypeFactory.GetTypeInfo(typeof(Type)) });

                    // The ILog and its methdods
                    logType = Type.GetType("log4net.Core.ILogger, log4net");
                    logTypeInfo = TypeFactory.GetTypeInfo(logType);

                    levelType = Type.GetType("log4net.Core.Level, log4net");
                    levelTypeInfo = TypeFactory.GetTypeInfo(levelType);

                    debugLevelPropertyValue = levelTypeInfo.GetField("Debug").GetValue(null);
                    infoLevelPropertyValue = levelTypeInfo.GetField("Info").GetValue(null);
                    errorLevelPropertyValue = levelTypeInfo.GetField("Error").GetValue(null);

                    systemStringFormatType = Type.GetType("log4net.Util.SystemStringFormat, log4net");

                    logMethod = logTypeInfo.GetMethod("Log", new ITypeInfo[] { TypeFactory.GetTypeInfo(typeof(Type)), levelTypeInfo, TypeFactory.GetTypeInfo(typeof(object)), TypeFactory.GetTypeInfo(typeof(Exception)) });
                    isEnabledForMethod = logTypeInfo.GetMethod("IsEnabledFor", new ITypeInfo[] { levelTypeInfo });

                    if (getLoggerWithTypeMethod == null ||
                        isEnabledForMethod == null ||
                        logType == null ||
                        levelType == null ||
                        logMethod == null)
                    {
                        loadState = LoadState.Failed;
                        return;
                    }

                    var log4netSectionPresent = AWSConfigs.XmlSectionExists("log4net");

                    // If log4net section exists and log4net logging is enabled, we attempt to activate
                    // log4net by calling XmlConfigurator.Configure()
                    if (log4netSectionPresent &&
                        (AWSConfigs.LoggingConfig.LogTo & LoggingOptions.Log4Net) == LoggingOptions.Log4Net)
                    {
                        ITypeInfo xmlConfiguratorType = TypeFactory.GetTypeInfo(Type.GetType("log4net.Config.XmlConfigurator, log4net"));
                        if (xmlConfiguratorType != null)
                        {
                            MethodInfo configureMethod = xmlConfiguratorType.GetMethod("Configure", new ITypeInfo[0]);
                            if (configureMethod != null)
                            {
                                configureMethod.Invoke(null, null);
                            }
                        }
                    }

                    loadState = LoadState.Success;
                }
                catch
                {
                    // Mark as failed so no attempted will be made on the logging methods.
                    loadState = LoadState.Failed;
                }
            }
        }

        public InternalLog4netLogger(Type declaringType)
            : base(declaringType)
        {
            if (loadState == LoadState.Uninitialized)
            {
                loadStatics();
            }

            if (logMangerType == null)
                return;

            this.internalLogger = getLoggerWithTypeMethod.Invoke(null, new object[] { TypeFactory.GetTypeInfo(declaringType).Assembly, declaringType }); //Assembly.GetCallingAssembly()
        }

        #region Overrides

        public override void Flush()
        {
        }

        /// <summary>
        /// Simple wrapper around the log4net IsErrorEnabled property.
        /// </summary>
        public override bool IsErrorEnabled
        {
            get
            {
                if (!isErrorEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || errorLevelPropertyValue == null)
                        isErrorEnabled = false;
                    else
                        isErrorEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { errorLevelPropertyValue }), CultureInfo.InvariantCulture);
                }
                return isErrorEnabled.Value;
            }
        }

        /// <summary>
        /// Simple wrapper around the log4net Error method.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public override void Error(Exception exception, string messageFormat, params object[] args)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, errorLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, messageFormat, args),
                    exception
                });
        }

        /// <summary>
        /// Simple wrapper around the log4net IsDebugEnabled property.
        /// </summary>
        public override bool IsDebugEnabled
        {
            get
            {
                if (!isDebugEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || debugLevelPropertyValue == null)
                        isDebugEnabled = false;
                    else
                        isDebugEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { debugLevelPropertyValue }), CultureInfo.InvariantCulture);
                }
                return isDebugEnabled.Value;
            }
        }

        /// <summary>
        /// Simple wrapper around the log4net Debug method.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="messageFormat"></param>
        /// <param name="args"></param>
        public override void Debug(Exception exception, string messageFormat, params object[] args)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, debugLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, messageFormat, args),
                    exception
                });
        }

        /// <summary>
        /// Simple wrapper around the log4net DebugFormat method.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public override void DebugFormat(string message, params object[] arguments)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, debugLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, message, arguments),
                    null
                });

        }

        /// <summary>
        /// Simple wrapper around the log4net IsInfoEnabled property.
        /// </summary>
        public override bool IsInfoEnabled
        {
            get
            {
                if (!isInfoEnabled.HasValue)
                {
                    if (loadState != LoadState.Success || this.internalLogger == null || loggerType == null || systemStringFormatType == null || infoLevelPropertyValue == null)
                        isInfoEnabled = false;
                    else
                        isInfoEnabled = Convert.ToBoolean(isEnabledForMethod.Invoke(this.internalLogger, new object[] { infoLevelPropertyValue }), CultureInfo.InvariantCulture);
                }
                return isInfoEnabled.Value;
            }
        }

        /// <summary>
        /// Simple wrapper around the log4net InfoFormat method.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="arguments"></param>
        public override void InfoFormat(string message, params object[] arguments)
        {
            logMethod.Invoke(
                this.internalLogger,
                new object[]
                {
                    loggerType, infoLevelPropertyValue,
                    new LogMessage(CultureInfo.InvariantCulture, message, arguments),
                    null
                });
        }

        #endregion
    }

}
