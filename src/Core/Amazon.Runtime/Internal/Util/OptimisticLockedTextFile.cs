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
using System.Text;

namespace Amazon.Runtime.Internal.Util
{
    /// <summary>
    /// Provides line-based read/write access to a file.
    /// The file can be read into memory, changed, then written back to disk.
    /// When the file is persisted back to disk, an optimistic concurrency
    /// check is performed to make sure the file hasn't changed since it was
    /// originally read.
    ///
    /// This class is not threadsafe.
    /// </summary>
    public class OptimisticLockedTextFile
    {
        /// <summary>
        /// a full copy of the original file
        /// This is used for optimistic concurrency.
        /// Note that this assumes a small file and does not scale large files.
        /// </summary>
        private string OriginalContents { get; set; }

        /// <summary>
        /// path of the file
        /// </summary>
        public string FilePath { get; private set; }

        /// <summary>
        /// Read/write access to the lines that make up the file.
        /// Any changes to this List are persisted back to disk when Persist() is called.
        ///
        /// NOTE:
        /// The lines have the original line endings on them to preserve the
        /// original text as much as possible.
        /// </summary>
        public List<string> Lines { get; private set; }

        /// <summary>
        /// Construct a new OptimisticLockedTextFile.
        /// </summary>
        /// <param name="filePath">path of the file</param>
        public OptimisticLockedTextFile(string filePath)
        {
            FilePath = filePath;
            Read();
        }

        /// <summary>
        /// Persist changes to disk after an optimistic concurrency check is completed.
        /// </summary>
        public void Persist()
        {
            var newContents = ToString();
            var path = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // open the file with exclusive access
            using (var fileStream = new FileStream(FilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                // get a current copy of the file
                string currentContents = null;
                using (var streamReader = new StreamReader(fileStream))
                {
                    currentContents = streamReader.ReadToEnd();

                    // optimistic concurrency check - make sure the file hasn't changed since it was read
                    if (string.Equals(currentContents, OriginalContents, StringComparison.Ordinal))
                    {
                        // write the new contents
                        fileStream.Seek(0, SeekOrigin.Begin);
                        using (var streamWriter = new StreamWriter(fileStream))
                        {
                            streamWriter.Write(newContents);
                            streamWriter.Flush();

                            // set the length in case the new contents are shorter than the old contents
                            fileStream.Flush();
                            fileStream.SetLength(fileStream.Position);
                            OriginalContents = newContents;
                        }
                    }
                    else
                    {
                        throw new IOException(string.Format(CultureInfo.InvariantCulture,
                                                            "Cannot write to file {0}. The file has been modified since it was last read.", 
                                                            FilePath));
                    }
                }
            }
        }

        public override string ToString()
        {
            // Make sure all lines have endings, with the exception of the last line.
            var contents = new StringBuilder();
            for (int i = 0; i < Lines.Count; i++)
            {
                var line = Lines[i];
                if (i < Lines.Count - 1 && !HasEnding(line))
                {
                    contents.AppendLine(line);
                }
                else
                {
                    contents.Append(line);
                }
            }
            return contents.ToString();
        }

        private void Read()
        {
            // Store a copy of the file for checking concurrency
            OriginalContents = "";

            if (File.Exists(FilePath))
            {
                try
                {
                    OriginalContents = File.ReadAllText(FilePath);
                }
                catch (FileNotFoundException)
                {
                    // This is OK.  The Persist() method will create it if necessary.
                }
                catch (DirectoryNotFoundException)
                {
                    // This is OK.  The Persist() method will create it if necessary.
                }
            }

            // Parse the lines ourselves since we need to preserve the line endings.
            // Parsing ourselves also avoids a race condition:
            // Doing ReadAllText then ReadAllLines would leave a small gap in time where the file could be changed.
            Lines = ReadLinesWithEndings(OriginalContents);
        }

        private static bool HasEnding(string line)
        {
            var lastChar = line[line.Length - 1];
            return lastChar == '\n' || lastChar == '\r';
        }

        private static List<string> ReadLinesWithEndings(string str)
        {
            var lines = new List<string>();
            var length = str.Length;
            var i = 0;
            var currentLineStart = 0;
            while (i < length)
            {
                if (str[i] == '\r')
                {
                    i++;
                    if (i < length && str[i] == '\n')
                    {
                        i++;
                    }
                    lines.Add(str.Substring(currentLineStart, i - currentLineStart));
                    currentLineStart = i;
                }
                else if (str[i] == '\n')
                {
                    i++;
                    lines.Add(str.Substring(currentLineStart, i - currentLineStart));
                    currentLineStart = i;
                }
                else
                {
                    i++;
                }
            }

            if (currentLineStart < i)
            {
                lines.Add(str.Substring(currentLineStart, i - currentLineStart));
            }

            return lines;
        }
    }
}