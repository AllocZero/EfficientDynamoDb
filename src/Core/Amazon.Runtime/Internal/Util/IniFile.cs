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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Provides read/write access to a file in the INI format.
    ///
    /// This class is not threadsafe.
    /// </summary>
    public class IniFile
    {
        private const string sectionNamePrefix = "[";
        private const string sectionNameSuffix = "]";
        private const string keyValueSeparator = "=";
        private const string semiColonComment = ";";
        private const string hashComment = "#";

        private OptimisticLockedTextFile textFile;

        private Logger logger;

        /// <summary>
        /// Construct a new IniFile.
        /// </summary>
        /// <param name="filePath">path of the IniFile</param>
        public IniFile(string filePath)
        {
            logger = Logger.GetLogger(GetType());
            textFile = new OptimisticLockedTextFile(filePath);
            Validate();
        }

        /// <summary>
        /// the path of the file
        /// </summary>
        public String FilePath
        {
            get
            {
                return textFile.FilePath;
            }
        }

        /// <summary>
        /// helper to access the lines of the file
        /// </summary>
        private List<string> Lines
        {
            get
            {
                return textFile.Lines;
            }
        }

        /// <summary>
        /// Persist the changes to this INI file to disk.
        /// </summary>
        public void Persist()
        {
            Validate();
            textFile.Persist();
        }

        /// <summary>
        /// Rename the section fromSectionName to toSectionName
        /// </summary>
        /// <param name="oldSectionName"></param>
        /// <param name="newSectionName"></param>
        public void RenameSection(string oldSectionName, string newSectionName)
        {
            RenameSection(oldSectionName, newSectionName, false);
        }

        /// <summary>
        /// Rename the section fromSectionName to toSectionName
        /// </summary>
        /// <param name="oldSectionName"></param>
        /// <param name="newSectionName"></param>
        /// <param name="force">if true and destination section already exists overwrite it</param>
        public void RenameSection(string oldSectionName, string newSectionName, bool force)
        {
            int sectionLineNumber = 0;
            if (TrySeekSection(oldSectionName, ref sectionLineNumber))
            {
                int lineNumber = 0;
                if (TrySeekSection(newSectionName, ref lineNumber))
                {
                    //  if oldSectionName == newSectionName it's a no op
                    if (!string.Equals(oldSectionName, newSectionName, StringComparison.Ordinal))
                    {
                        if (force)
                        {
                            DeleteSection(newSectionName);
                            // recursive call with force == false now that the destination section is gone
                            RenameSection(oldSectionName, newSectionName, false);
                        }
                        else
                            throw new ArgumentException("Cannot rename section. The destination section " + newSectionName +
                                " already exists." + GetLineMessage(lineNumber));
                    }
                }
                else
                    Lines[sectionLineNumber] = sectionNamePrefix + newSectionName + sectionNameSuffix;
            }
            else
                throw new ArgumentException("Cannot rename section. The source section " + oldSectionName + " does not exist.");
        }
        /// <summary>
        /// Copy the section fromSectionName to toSectionName
        /// </summary>
        /// <param name="fromSectionName"></param>
        /// <param name="toSectionName"></param>
        /// <param name="replaceProperties">Any properties in the original section that are also in this dictionary will
        /// be replaced by the value from this dictionary.</param>
        public void CopySection(string fromSectionName, string toSectionName, Dictionary<string, string> replaceProperties)
        {
            CopySection(fromSectionName, toSectionName, replaceProperties, false);
        }

        /// <summary>
        /// Copy the section fromSectionName to toSectionName
        /// </summary>
        /// <param name="fromSectionName"></param>
        /// <param name="toSectionName"></param>
        /// <param name="replaceProperties">Any properties in the original section that are also in this dictionary will
        /// be replaced by the value from this dictionary.</param>
        /// <param name="force">if true and destination section already exists overwrite it</param>
        public void CopySection(string fromSectionName, string toSectionName, Dictionary<string, string> replaceProperties, bool force)
        {
            int currentLineNumber = 0;
            if (TrySeekSection(fromSectionName, ref currentLineNumber))
            {
                int lineNumber = 0;
                if (TrySeekSection(toSectionName, ref lineNumber))
                {
                    //  if fromSectionName == toSectionName it's a no op
                    if (!string.Equals(fromSectionName, toSectionName, StringComparison.Ordinal))
                    {
                        if (force)
                        {
                            DeleteSection(toSectionName);
                            // recursive call with force == false now that the destination section is gone
                            CopySection(fromSectionName, toSectionName, replaceProperties, false);
                        }
                        else
                            throw new ArgumentException("Cannot copy section. The destination section " + toSectionName +
                                " already exists." + GetLineMessage(lineNumber));
                    }
                }
                else
                {
                    // keep the first line number
                    var firstLineNumber = currentLineNumber;

                    // find the last line number (exclusive)
                    // could be end of file or beginning of next section
                    string dummySectionName;
                    currentLineNumber++;
                    while (currentLineNumber < Lines.Count
                        && !TryParseSection(Lines[currentLineNumber], out dummySectionName))
                    {
                        currentLineNumber++;
                    }

                    // add the new section header to the end of the file
                    Lines.Add(sectionNamePrefix + toSectionName + sectionNameSuffix);

                    // copy the contents of the section to the end of the file
                    for (int line = firstLineNumber + 1; line < currentLineNumber; line++)
                    {
                        // If the key is in replaceProperties use the value from there
                        // otherwise just copy the line.
                        string propertyName;
                        string unused;
                        if (TryParseProperty(Lines[line], out propertyName, out unused) &&
                            replaceProperties.ContainsKey(propertyName))
                            Lines.Add(GetPropertyLine(propertyName, replaceProperties[propertyName]));
                        else
                            Lines.Add(Lines[line]);
                    }
                }
            }
            else
                throw new ArgumentException("Cannot copy section. The source section " + fromSectionName + " does not exist.");
        }

        /// <summary>
        /// Update the section with the properties given.
        /// If the section doesn't exist, it will be appended to the file.
        ///
        /// Notes:
        /// 1. Any properties that do exist in the section will be updated.
        /// 2. A null value for a property denotes that it should be deleted from the section
        /// 3. If any properties don't exist they will be appended to the end of the section
        /// in the same order they appear in the SortedDictionary.
        /// </summary>
        /// <param name="sectionName">name of the section to operate on</param>
        /// <param name="properties">properties to add/update/delete</param>
        public void EditSection(string sectionName, SortedDictionary<string, string> properties)
        {
            EnsureSectionExists(sectionName);

            // build dictionary from list
            // ensure the keys will be looked up case-insensitive
            var propertiesLookup = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var pair in properties)
            {
                propertiesLookup.Add(pair.Key, pair.Value);
            }

            var lineNumber = 0;
            if (TrySeekSection(sectionName, ref lineNumber))
            {
                lineNumber++;
                string propertyName;
                string propertyValue;
                while (SeekProperty(ref lineNumber, out propertyName, out propertyValue))
                {
                    var propertyDeleted = false;
                    if (propertiesLookup.ContainsKey(propertyName))
                    {
                        if (!string.Equals(propertiesLookup[propertyName], propertyValue))
                        {
                            if (propertiesLookup[propertyName] == null)
                            {
                                // delete the line
                                Lines.RemoveAt(lineNumber);
                                propertyDeleted = true;
                            }
                            else
                            {
                                // update the line
                                Lines[lineNumber] = GetPropertyLine(propertyName, propertiesLookup[propertyName]);
                            }
                        }
                        propertiesLookup.Remove(propertyName);
                    }

                    if (!propertyDeleted)
                    {
                        lineNumber++;
                    }
                }
                foreach (var pair in properties)
                {
                    if (propertiesLookup.ContainsKey(pair.Key) && propertiesLookup[pair.Key] != null)
                    {
                        Lines.Insert(lineNumber++, pair.Key + keyValueSeparator + pair.Value);
                    }
                }
            }
        }

        /// <summary>
        /// Check if the section exists.  If not, append it to the end of the file.
        /// </summary>
        /// <param name="sectionName">section to ensure exists</param>
        public void EnsureSectionExists(string sectionName)
        {
            var lineNumber = 0;
            if (!TrySeekSection(sectionName, ref lineNumber))
            {
                Lines.Add(sectionNamePrefix + sectionName + sectionNameSuffix);
            }
        }

        /// <summary>
        /// If the section exists, delete it from the INI file.
        /// </summary>
        /// <param name="sectionName">section to delete</param>
        public void DeleteSection(string sectionName)
        {
            var lineNumber = 0;
            if (TrySeekSection(sectionName, ref lineNumber))
            {
                Lines.RemoveAt(lineNumber);
                while (lineNumber < Lines.Count &&
                    !IsSection(Lines[lineNumber]))
                {
                    Lines.RemoveAt(lineNumber);
                }
            }
        }

        public virtual HashSet<string> ListSectionNames()
        {
            var sectionNames = new HashSet<string>();
            int lineNumber = 0;
            string sectionName = null;
            while (SeekSection(ref lineNumber, out sectionName))
            {
                sectionNames.Add(sectionName);
                lineNumber++;
            }
            return sectionNames;
        }

        /// <summary>
        /// Determine if a section exists in the INI file.
        /// </summary>
        /// <param name="sectionName">name of section to look for</param>
        /// <returns>true if the section exists, false otherwise</returns>
        public bool SectionExists(string sectionName)
        {
            var lineNumber = 0;
            return TrySeekSection(sectionName, ref lineNumber);
        }

        /// <summary>
        /// Return the properties for the section if it exists.
        /// </summary>
        /// <param name="sectionName">name of section to get</param>
        /// <param name="properties">properties contained in the section</param>
        /// <returns>True if the section was found, false otherwise</returns>
        public virtual bool TryGetSection(string sectionName, out Dictionary<string, string> properties)
        {
            var lineNumber = 0;
            properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (TrySeekSection(sectionName, ref lineNumber))
            {
                lineNumber++;
                string propertyName;
                string propertyValue;
                while (SeekProperty(ref lineNumber, out propertyName, out propertyValue))
                {
                    if (IsDuplicateProperty(properties, propertyName, sectionName, lineNumber))
                    {
                        properties.Clear();
                        return false;
                    }

                    properties.Add(propertyName, propertyValue);
                    lineNumber++;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Return the properties for the section if it exists.
        /// </summary>
        /// <param name="sectionNameRegex">Regex to match name of section to get</param>
        /// <param name="properties">properties contained in the section</param>
        /// <returns>True if the section was found, false otherwise</returns>
        public bool TryGetSection(Regex sectionNameRegex, out Dictionary<string, string> properties)
        {
            string dummy = null;
            return TryGetSection(sectionNameRegex, out dummy, out properties);
        }

        /// <summary>
        /// Return the properties for the section if it exists.
        /// </summary>
        /// <param name="sectionNameRegex">Regex to match name of section to get</param>
        /// <param name="sectionName">name of section if regex matches</param>
        /// <param name="properties">properties contained in the section</param>
        /// <returns>True if the section was found, false otherwise</returns>
        public bool TryGetSection(Regex sectionNameRegex, out string sectionName,
            out Dictionary<string, string> properties)
        {
            var lineNumber = 0;
            properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (TrySeekSection(sectionNameRegex, ref lineNumber, out sectionName))
            {
                lineNumber++;
                string propertyName;
                string propertyValue;
                while (SeekProperty(ref lineNumber, out propertyName, out propertyValue))
                {
                    if (IsDuplicateProperty(properties, propertyName, sectionName, lineNumber))
                    {
                        sectionName = null;
                        properties.Clear();
                        return false;
                    }

                    properties.Add(propertyName, propertyValue);
                    lineNumber++;
                }
                return true;
            }
            return false;
        }

        override public string ToString()
        {
            return textFile.ToString();
        }

        private bool IsDuplicateProperty(Dictionary<string, string> properties, string propertyName, string sectionName, int lineNumber)
        {
            var result = properties.ContainsKey(propertyName);
            if (result)
                logger.InfoFormat("Skipping section {0} because of duplicate property {1}.  {2}", sectionName, propertyName, GetLineMessage(lineNumber));
            return result;
        }

        private void Validate()
        {
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                if (!IsProperty(line) && !IsSection(line) && !IsCommentOrBlank(line))
                {
                    throw new InvalidDataException(GetErrorMessage(i));
                }
            }
        }

        private bool TrySeekSection(Regex sectionNameRegex, ref int lineNumber, out string sectionName)
        {
            string currentSectionName = null;
            while (SeekSection(ref lineNumber, out currentSectionName) &&
                !sectionNameRegex.IsMatch(currentSectionName))
            {
                lineNumber++;
            }
            sectionName = currentSectionName;
            return currentSectionName != null && sectionNameRegex.IsMatch(currentSectionName);
        }

        private bool TrySeekSection(String sectionName, ref int lineNumber)
        {
            string currentSectionName = null;
            while (SeekSection(ref lineNumber, out currentSectionName) &&
                !string.Equals(sectionName, currentSectionName, StringComparison.Ordinal))
            {
                lineNumber++;
            }
            return string.Equals(sectionName, currentSectionName, StringComparison.Ordinal);
        }

        private bool SeekSection(ref int lineNumber, out string sectionName)
        {
            while (lineNumber < Lines.Count)
            {
                if (TryParseSection(Lines[lineNumber], out sectionName))
                {
                    return true;
                }
                lineNumber++;
            }
            sectionName = null;
            return false;
        }

        private bool SeekProperty(ref int lineNumber, out string propertyName, out string propertyValue)
        {
            while (lineNumber < Lines.Count)
            {
                if (TryParseProperty(Lines[lineNumber], out propertyName, out propertyValue))
                {
                    return true;
                }
                else if (IsSection(Lines[lineNumber]))
                {
                    return false;
                }
                else if (IsCommentOrBlank(Lines[lineNumber]))
                {
                    lineNumber++;
                }
                else
                {
                    throw new InvalidDataException(GetErrorMessage(lineNumber));
                }
            }
            propertyName = null;
            propertyValue = null;
            return false;
        }

        private string GetErrorMessage(int lineNumber)
        {
            return string.Format(CultureInfo.InvariantCulture,
                                 "Line {0}:<{1}> in file {2} does not contain a section, property or comment.", 
                                 lineNumber + 1, 
                                 Lines[lineNumber], 
                                 FilePath);
        }

        private static bool IsCommentOrBlank(string line)
        {
            if (line == null)
            {
                return true;
            }
            else
            {
                line = line.Trim();
                return string.IsNullOrEmpty(line) 
                    || line.StartsWith(semiColonComment, StringComparison.Ordinal) 
                    || line.StartsWith(hashComment, StringComparison.Ordinal);
            }
        }

        private static bool IsSection(string line)
        {
            string dummy;
            return TryParseSection(line, out dummy);
        }

        private static bool TryParseSection(string line, out string sectionName)
        {
            if (line != null)
            {
                line = line.Trim();
                if (line.StartsWith(sectionNamePrefix, StringComparison.Ordinal) 
                    && line.EndsWith(sectionNameSuffix, StringComparison.Ordinal))
                {
                    sectionName = line.Substring(1, line.Length - 2).Trim();
                    return true;
                }
            }
            sectionName = null;
            return false;
        }

        private static bool IsProperty(string line)
        {
            string dummyName;
            string dummyValue;
            return TryParseProperty(line, out dummyName, out dummyValue);
        }

        private static bool TryParseProperty(string line, out string propertyName, out string propertyValue)
        {
            if (line != null && !IsCommentOrBlank(line))
            {
                line = line.Trim();
                var separatorIndex = line.IndexOf(keyValueSeparator, StringComparison.Ordinal);
                if (separatorIndex >= 0)
                {
                    propertyName = line.Substring(0, separatorIndex).Trim();
                    var valueStartIndex = separatorIndex + keyValueSeparator.Length;
                    propertyValue = line.Substring(valueStartIndex, line.Length - valueStartIndex).Trim();
                    return true;
                }
            }
            propertyName = null;
            propertyValue = null;
            return false;
        }

        private static string GetPropertyLine(string propertyName, string propertyValue)
        {
            return string.Concat(propertyName, keyValueSeparator, propertyValue);
        }

        private string GetLineMessage(int lineNumber)
        {
            return string.Concat("(", this.FilePath, ":line ", lineNumber + 1, ")");
        }
    }
}