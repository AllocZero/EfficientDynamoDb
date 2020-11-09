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

using ThirdParty.BouncyCastle.Utilities;

namespace ThirdParty.BouncyCastle.Asn1
{
    public abstract class Asn1Sequence
		: Asn1Object, IEnumerable
    {
        private readonly IList seq;




		protected internal Asn1Sequence(
			int capacity)
		{
            seq = Platform.CreateArrayList(capacity);
		}

		public virtual IEnumerator GetEnumerator()
		{
			return seq.GetEnumerator();
		}

		[Obsolete("Use GetEnumerator() instead")]
		public IEnumerator GetObjects()
        {
            return GetEnumerator();
        }



        /**
         * return the object at the sequence position indicated by index.
         *
         * @param index the sequence number (starting at zero) of the object
         * @return the object at the sequence position indicated by index.
         */
		public virtual Asn1Encodable this[int index]
		{
			get { return (Asn1Encodable) seq[index]; }
		}

		[Obsolete("Use 'object[index]' syntax instead")]
        public Asn1Encodable GetObjectAt(
            int index)
        {
             return this[index];
        }

		[Obsolete("Use 'Count' property instead")]
		public int Size
        {
			get { return Count; }
        }

		public virtual int Count
		{
			get { return seq.Count; }
		}


		private Asn1Encodable GetCurrent(IEnumerator e)
		{
			Asn1Encodable encObj = (Asn1Encodable)e.Current;
			return encObj;
		}

		protected internal void AddObject(
            Asn1Encodable obj)
        {
            seq.Add(obj);
        }
    }
}
