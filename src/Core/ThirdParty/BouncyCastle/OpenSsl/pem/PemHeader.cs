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

namespace ThirdParty.BouncyCastle.Utilities.IO.Pem
{
	public class PemHeader
	{
		private string name;
		private string val;

		public PemHeader(string name, string val)
		{
			this.name = name;
			this.val = val;
		}

		public virtual string Name
		{
			get { return name; }
		}

		public virtual string Value
		{
			get { return val; }
		}

		public override int GetHashCode()
		{
			return GetHashCode(this.name) + 31 * GetHashCode(this.val);
		}

		public override bool Equals(object obj)
		{
			if (obj == this)
				return true;

			if (!(obj is PemHeader))
				return false;

			PemHeader other = (PemHeader)obj;

			return string.Equals(this.name, other.name)
                && string.Equals(this.val, other.val);
		}

		private int GetHashCode(string s)
		{
			if (s == null)
			{
				return 1;
			}

			return s.GetHashCode();
		}
	}
}
