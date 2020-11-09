/*
 * Copyright (c) 2000 - 2011 The Legion Of The Bouncy Castle (http://www.bouncycastle.org)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this 
 * software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, 
 * copy, modify, merge, publish, distribute, sublicense, and/or sell copies of 
 * the Software, and to permit persons to whom the Software is furnished to do so, 
 * subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all copies 
 * or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
 * PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE 
 * FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR 
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
 * DEALINGS IN THE SOFTWARE.
 * 
 */
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace ThirdParty.BouncyCastle.Utilities.IO.Pem
{
	public class PemReader
	{
		private const string BeginString = "-----BEGIN ";
		private const string EndString = "-----END ";

		private readonly TextReader reader;

		public PemReader(TextReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			this.reader = reader;
		}

		public TextReader Reader
		{
			get { return reader; }
		}

		/// <returns>
		/// A <see cref="PemObject"/>
		/// </returns>
		/// <exception cref="IOException"></exception>
		public PemObject ReadPemObject()
		{
			string line = reader.ReadLine();

			if (line != null && line.StartsWith(BeginString))
			{
				line = line.Substring(BeginString.Length);
				int index = line.IndexOf('-');
				string type = line.Substring(0, index);

				if (index > 0)
					return LoadObject(type);
			}

			return null;
		}

		private PemObject LoadObject(string type)
		{
			string endMarker = EndString + type;
			IList headers = Platform.CreateArrayList();
			StringBuilder buf = new StringBuilder();

			string line;
			while ((line = reader.ReadLine()) != null
				&& line.IndexOf(endMarker) == -1)
			{
				int colonPos = line.IndexOf(':');

				if (colonPos == -1)
				{
					buf.Append(line.Trim());
				}
				else
				{
					// Process field
					string fieldName = line.Substring(0, colonPos).Trim();

					if (fieldName.StartsWith("X-"))
						fieldName = fieldName.Substring(2);

					string fieldValue = line.Substring(colonPos + 1).Trim();

					headers.Add(new PemHeader(fieldName, fieldValue));
				}
			}

			if (line == null)
			{
				throw new IOException(endMarker + " not found");
			}

			if (buf.Length % 4 != 0)
			{
				throw new IOException("base64 data appears to be truncated");
			}

			return new PemObject(type, headers, Convert.FromBase64String(buf.ToString()));
		}
	}
}
