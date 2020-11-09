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
using System.IO;

namespace ThirdParty.BouncyCastle.Asn1
{
    public abstract class Asn1Object
		: Asn1Encodable
    {
		/// <summary>Create a base ASN.1 object from a byte array.</summary>
		/// <param name="data">The byte array to parse.</param>
		/// <returns>The base ASN.1 object represented by the byte array.</returns>
		/// <exception cref="IOException">If there is a problem parsing the data.</exception>
		public static Asn1Object FromByteArray(
			byte[] data)
		{
			try
			{
				return new Asn1InputStream(data).ReadObject();
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in stream");    
			}
		}

		/// <summary>Read a base ASN.1 object from a stream.</summary>
		/// <param name="inStr">The stream to parse.</param>
		/// <returns>The base ASN.1 object represented by the byte array.</returns>
		/// <exception cref="IOException">If there is a problem parsing the data.</exception>
		public static Asn1Object FromStream(
			Stream inStr)
		{
			try
			{
				return new Asn1InputStream(inStr).ReadObject();
			}
			catch (InvalidCastException)
			{
				throw new IOException("cannot recognise object in stream");    
			}
		}

		public sealed override Asn1Object ToAsn1Object()
        {
            return this;
        }

	}
}
