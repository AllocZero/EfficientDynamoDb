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
using System.Diagnostics;
using System.IO;

using ThirdParty.BouncyCastle.Asn1.Utilities;
using ThirdParty.BouncyCastle.Utilities.IO;

namespace ThirdParty.BouncyCastle.Asn1
{
	/**
	 * a general purpose ASN.1 decoder - note: this class differs from the
	 * others in that it returns null after it has read the last object in
	 * the stream. If an ASN.1 Null is encountered a Der/BER Null object is
	 * returned.
	 */
	public class Asn1InputStream
		: FilterStream
	{
        public const int Constructed = 0x20;
        public const int Integer = 0x02;

        private readonly int limit;

		internal static int FindLimit(Stream input)
		{
			if (input is MemoryStream)
			{
				MemoryStream mem = (MemoryStream)input;
				return (int)(mem.Length - mem.Position);
			}

			return int.MaxValue;
		}

		public Asn1InputStream(
			Stream inputStream)
			: this(inputStream, FindLimit(inputStream))
		{
		}

		/**
		 * Create an ASN1InputStream where no DER object will be longer than limit.
		 *
		 * @param input stream containing ASN.1 encoded data.
		 * @param limit maximum size of a DER encoded object.
		 */
		public Asn1InputStream(
			Stream	inputStream,
			int		limit)
			: base(inputStream)
		{
			this.limit = limit;
		}

		/**
		 * Create an ASN1InputStream based on the input byte array. The length of DER objects in
		 * the stream is automatically limited to the length of the input array.
		 *
		 * @param input array containing ASN.1 encoded data.
		 */
		public Asn1InputStream(
			byte[] input)
			: this(new MemoryStream(input, false), input.Length)
		{
		}

		/**
		* build an object given its tag and the number of bytes to construct it from.
		*/
		private Asn1Object BuildObject(
			int	tag,
			int	tagNo,
			int	length)
		{
			bool isConstructed = (tag & Constructed) != 0;

            if (isConstructed)
            {
                return CreateDerSequence(this);
            }
            else
            {
                byte[] bytes = new byte[length];
                this.Read(bytes, 0, bytes.Length);
                return CreatePrimitiveDerObject(tagNo, bytes);
            }
		}

		internal Asn1EncodableVector BuildEncodableVector()
		{
			Asn1EncodableVector v = new Asn1EncodableVector();

			Asn1Object o;
			while ((o = ReadObject()) != null)
			{
				v.Add(o);
			}

			return v;
		}

		internal virtual Asn1EncodableVector BuildDerEncodableVector(
			Stream dIn)
		{
			return new Asn1InputStream(dIn).BuildEncodableVector();
		}

		internal virtual DerSequence CreateDerSequence(
            Stream dIn)
		{
			return DerSequence.FromVector(BuildDerEncodableVector(dIn));
		}


		public Asn1Object ReadObject()
		{
			int tag = ReadByte();
			if (tag <= 0)
			{
				if (tag == 0)
					throw new IOException("unexpected end-of-contents marker");

				return null;
			}

			//
			// calculate tag number
			//
			int tagNo = ReadTagNumber(this, tag);

			bool isConstructed = (tag & Constructed) != 0;

			//
			// calculate length
			//
			int length = ReadLength(this, limit);


			try
			{
				return BuildObject(tag, tagNo, length);
			}
			catch (ArgumentException e)
			{
				throw new Exception("corrupted stream detected", e);
			}
		}

		internal static int ReadTagNumber(
			Stream	s,
			int		tag)
		{
			int tagNo = tag & 0x1f;

			//
			// with tagged object tag number is bottom 5 bits, or stored at the start of the content
			//
			if (tagNo == 0x1f)
			{
				tagNo = 0;

				int b = s.ReadByte();

				// X.690-0207 8.1.2.4.2
				// "c) bits 7 to 1 of the first subsequent octet shall not all be zero."
				if ((b & 0x7f) == 0) // Note: -1 will pass
				{
					throw new IOException("Corrupted stream - invalid high tag number found");
				}

				while ((b >= 0) && ((b & 0x80) != 0))
				{
					tagNo |= (b & 0x7f);
					tagNo <<= 7;
					b = s.ReadByte();
				}

				if (b < 0)
					throw new EndOfStreamException("EOF found inside tag value.");

				tagNo |= (b & 0x7f);
			}

			return tagNo;
		}

		internal static int ReadLength(
			Stream	s,
			int		limit)
		{
			int length = s.ReadByte();
			if (length < 0)
				throw new EndOfStreamException("EOF found when length expected");

			if (length == 0x80)
				return -1;      // indefinite-length encoding

			if (length > 127)
			{
				int size = length & 0x7f;

				// Note: The invalid long form "0xff" (see X.690 8.1.3.5c) will be caught here
				if (size > 4)
					throw new IOException("DER length more than 4 bytes: " + size);

				length = 0;
				for (int i = 0; i < size; i++)
				{
					int next = s.ReadByte();

					if (next < 0)
						throw new EndOfStreamException("EOF found reading length");

					length = (length << 8) + next;
				}

				if (length < 0)
					throw new IOException("Corrupted stream - negative length found");

				if (length >= limit)   // after all we must have read at least 1 byte
					throw new IOException("Corrupted stream - out of bounds length found");
			}

			return length;
		}

		internal static Asn1Object CreatePrimitiveDerObject(
			int		tagNo,
			byte[]	bytes)
		{

			switch (tagNo)
			{
				case Integer:
					return new DerInteger(bytes);
				default:
					throw new Exception("Unknown primitive tag");
			}
		}
	}
}
