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

using ThirdParty.BouncyCastle.Utilities;

namespace ThirdParty.BouncyCastle.Asn1
{
    public class Asn1EncodableVector
		: IEnumerable
    {
        private IList v = Platform.CreateArrayList();

		public static Asn1EncodableVector FromEnumerable(
			IEnumerable e)
		{
			Asn1EncodableVector v = new Asn1EncodableVector();
			foreach (Asn1Encodable obj in e)
			{
				v.Add(obj);
			}
			return v;
		}

		public Asn1EncodableVector(
			params Asn1Encodable[] v)
		{
			Add(v);
		}

		public void Add(
			params Asn1Encodable[] objs)
		{
			foreach (Asn1Encodable obj in objs)
			{
				v.Add(obj);
			}
		}

		public void AddOptional(
			params Asn1Encodable[] objs)
		{
			if (objs != null)
			{
				foreach (Asn1Encodable obj in objs)
				{
					if (obj != null)
					{
						v.Add(obj);
					}
				}
			}
		}

		public Asn1Encodable this[
			int index]
		{
			get { return (Asn1Encodable) v[index]; }
		}

		[Obsolete("Use 'object[index]' syntax instead")]
		public Asn1Encodable Get(
            int index)
        {
            return this[index];
        }

		[Obsolete("Use 'Count' property instead")]
		public int Size
		{
			get { return v.Count; }
		}

		public int Count
		{
			get { return v.Count; }
		}

		public IEnumerator GetEnumerator()
		{
			return v.GetEnumerator();
		}
	}
}
