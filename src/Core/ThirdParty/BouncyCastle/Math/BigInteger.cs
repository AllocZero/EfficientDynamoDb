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
using System.Diagnostics;
using System.Globalization;
using System.Text;

using ThirdParty.BouncyCastle.Utilities;

namespace ThirdParty.BouncyCastle.Math
{

    public class BigInteger
    {
        private const long IMASK = 0xffffffffL;

        private static readonly int[] ZeroMagnitude = new int[0];
        private static readonly byte[] ZeroEncoding = new byte[0];

        public static readonly BigInteger Zero = new BigInteger(0, ZeroMagnitude, false);
        public static readonly BigInteger One = createUValueOf(1);
        public static readonly BigInteger Two = createUValueOf(2);
        public static readonly BigInteger Three = createUValueOf(3);
        public static readonly BigInteger Ten = createUValueOf(10);

        private static readonly Random RandomSource = new Random();

        private const int BitsPerByte = 8;
        private const int BitsPerInt = 32;
        private const int BytesPerInt = 4;


        private int sign; // -1 means -ve; +1 means +ve; 0 means 0;
        private int[] magnitude; // array of ints with [0] being the most significant
        private int nBitLength = -1; // cache calcBitLength() value

        private static int GetByteLength(
            int nBits)
        {
            return (nBits + BitsPerByte - 1) / BitsPerByte;
        }

        private BigInteger()
        {
        }

        private BigInteger(
            int		signum,
            int[]	mag,
            bool	checkMag)
        {
            if (checkMag)
            {
                int i = 0;
                while (i < mag.Length && mag[i] == 0)
                {
                    ++i;
                }

                if (i == mag.Length)
                {
//					this.sign = 0;
                    this.magnitude = ZeroMagnitude;
                }
                else
                {
                    this.sign = signum;

                    if (i == 0)
                    {
                        this.magnitude = mag;
                    }
                    else
                    {
                        // strip leading 0 words
                        this.magnitude = new int[mag.Length - i];
                        Array.Copy(mag, i, this.magnitude, 0, this.magnitude.Length);
                    }
                }
            }
            else
            {
                this.sign = signum;
                this.magnitude = mag;
            }
        }

        public BigInteger(
            byte[] bytes)
            : this(bytes, 0, bytes.Length)
        {
        }

        public BigInteger(
            byte[]	bytes,
            int		offset,
            int		length)
        {
            if (length == 0)
                throw new FormatException("Zero length BigInteger");

            // TODO Move this processing into MakeMagnitude (provide sign argument)
            if ((sbyte)bytes[offset] < 0)
            {
                this.sign = -1;

                int end = offset + length;

                int iBval;
                // strip leading sign bytes
                for (iBval = offset; iBval < end && ((sbyte)bytes[iBval] == -1); iBval++)
                {
                }

                if (iBval >= end)
                {
                    this.magnitude = One.magnitude;
                }
                else
                {
                    int numBytes = end - iBval;
                    byte[] inverse = new byte[numBytes];

                    int index = 0;
                    while (index < numBytes)
                    {
                        inverse[index++] = (byte)~bytes[iBval++];
                    }

                    while (inverse[--index] == byte.MaxValue)
                    {
                        inverse[index] = byte.MinValue;
                    }

                    inverse[index]++;

                    this.magnitude = MakeMagnitude(inverse, 0, inverse.Length);
                }
            }
            else
            {
                // strip leading zero bytes and return magnitude bytes
                this.magnitude = MakeMagnitude(bytes, offset, length);
                this.sign = this.magnitude.Length > 0 ? 1 : 0;
            }
        }

        private static int[] MakeMagnitude(
            byte[]	bytes,
            int		offset,
            int		length)
        {
            int end = offset + length;

            // strip leading zeros
            int firstSignificant;
            for (firstSignificant = offset; firstSignificant < end
                && bytes[firstSignificant] == 0; firstSignificant++)
            {
            }

            if (firstSignificant >= end)
            {
                return ZeroMagnitude;
            }

            int nInts = (end - firstSignificant + 3) / BytesPerInt;
            int bCount = (end - firstSignificant) % BytesPerInt;
            if (bCount == 0)
            {
                bCount = BytesPerInt;
            }

            if (nInts < 1)
            {
                return ZeroMagnitude;
            }

            int[] mag = new int[nInts];

            int v = 0;
            int magnitudeIndex = 0;
            for (int i = firstSignificant; i < end; ++i)
            {
                v <<= 8;
                v |= bytes[i] & 0xff;
                bCount--;
                if (bCount <= 0)
                {
                    mag[magnitudeIndex] = v;
                    magnitudeIndex++;
                    bCount = BytesPerInt;
                    v = 0;
                }
            }

            if (magnitudeIndex < mag.Length)
            {
                mag[magnitudeIndex] = v;
            }

            return mag;
        }

        private static readonly byte[] rndMask = { 255, 127, 63, 31, 15, 7, 3, 1 };



        /**
         * return a = a + b - b preserved.
         */
        private static int[] AddMagnitudes(
            int[] a,
            int[] b)
        {
            int tI = a.Length - 1;
            int vI = b.Length - 1;
            long m = 0;

            while (vI >= 0)
            {
                m += ((long)(uint)a[tI] + (long)(uint)b[vI--]);
                a[tI--] = (int)m;
                m = (long)((ulong)m >> 32);
            }

            if (m != 0)
            {
                while (tI >= 0 && ++a[tI--] == 0)
                {
                }
            }

            return a;
        }



        private BigInteger AddToMagnitude(
            int[] magToAdd)
        {
            int[] big, small;
            if (this.magnitude.Length < magToAdd.Length)
            {
                big = magToAdd;
                small = this.magnitude;
            }
            else
            {
                big = this.magnitude;
                small = magToAdd;
            }

            // Conservatively avoid over-allocation when no overflow possible
            uint limit = uint.MaxValue;
            if (big.Length == small.Length)
                limit -= (uint) small[0];

            bool possibleOverflow = (uint) big[0] >= limit;

            int[] bigCopy;
            if (possibleOverflow)
            {
                bigCopy = new int[big.Length + 1];
                big.CopyTo(bigCopy, 1);
            }
            else
            {
                bigCopy = (int[]) big.Clone();
            }

            bigCopy = AddMagnitudes(bigCopy, small);

            return new BigInteger(this.sign, bigCopy, possibleOverflow);
        }

        private int calcBitLength(
            int		indx,
            int[]	mag)
        {
            for (;;)
            {
                if (indx >= mag.Length)
                    return 0;

                if (mag[indx] != 0)
                    break;

                ++indx;
            }

            // bit length for everything after the first int
            int bitLength = 32 * ((mag.Length - indx) - 1);

            // and determine bitlength of first int
            int firstMag = mag[indx];
            bitLength += BitLen(firstMag);

            // Check for negative powers of two
            if (sign < 0 && ((firstMag & -firstMag) == firstMag))
            {
                do
                {
                    if (++indx >= mag.Length)
                    {
                        --bitLength;
                        break;
                    }
                }
                while (mag[indx] == 0);
            }

            return bitLength;
        }

        public int BitLength
        {
            get
            {
                if (nBitLength == -1)
                {
                    nBitLength = sign == 0
                        ? 0
                        : calcBitLength(0, magnitude);
                }

                return nBitLength;
            }
        }

        //
        // BitLen(value) is the number of bits in value.
        //
        private static int BitLen(
            int w)
        {
            // Binary search - decision tree (5 tests, rarely 6)
            return (w < 1 << 15 ? (w < 1 << 7
                ? (w < 1 << 3 ? (w < 1 << 1
                ? (w < 1 << 0 ? (w < 0 ? 32 : 0) : 1)
                : (w < 1 << 2 ? 2 : 3)) : (w < 1 << 5
                ? (w < 1 << 4 ? 4 : 5)
                : (w < 1 << 6 ? 6 : 7)))
                : (w < 1 << 11
                ? (w < 1 << 9 ? (w < 1 << 8 ? 8 : 9) : (w < 1 << 10 ? 10 : 11))
                : (w < 1 << 13 ? (w < 1 << 12 ? 12 : 13) : (w < 1 << 14 ? 14 : 15)))) : (w < 1 << 23 ? (w < 1 << 19
                ? (w < 1 << 17 ? (w < 1 << 16 ? 16 : 17) : (w < 1 << 18 ? 18 : 19))
                : (w < 1 << 21 ? (w < 1 << 20 ? 20 : 21) : (w < 1 << 22 ? 22 : 23))) : (w < 1 << 27
                ? (w < 1 << 25 ? (w < 1 << 24 ? 24 : 25) : (w < 1 << 26 ? 26 : 27))
                : (w < 1 << 29 ? (w < 1 << 28 ? 28 : 29) : (w < 1 << 30 ? 30 : 31)))));
        }

        public int CompareTo(
            object obj)
        {
            return CompareTo((BigInteger)obj);
        }

        /**
         * unsigned comparison on two arrays - note the arrays may
         * start with leading zeros.
         */
        private static int CompareTo(
            int		xIndx,
            int[]	x,
            int		yIndx,
            int[]	y)
        {
            while (xIndx != x.Length && x[xIndx] == 0)
            {
                xIndx++;
            }

            while (yIndx != y.Length && y[yIndx] == 0)
            {
                yIndx++;
            }

            return CompareNoLeadingZeroes(xIndx, x, yIndx, y);
        }

        private static int CompareNoLeadingZeroes(
            int		xIndx,
            int[]	x,
            int		yIndx,
            int[]	y)
        {
            int diff = (x.Length - y.Length) - (xIndx - yIndx);

            if (diff != 0)
            {
                return diff < 0 ? -1 : 1;
            }

            // lengths of magnitudes the same, test the magnitude values

            while (xIndx < x.Length)
            {
                uint v1 = (uint)x[xIndx++];
                uint v2 = (uint)y[yIndx++];

                if (v1 != v2)
                    return v1 < v2 ? -1 : 1;
            }

            return 0;
        }

        public int CompareTo(
            BigInteger value)
        {
            return sign < value.sign ? -1
                : sign > value.sign ? 1
                : sign == 0 ? 0
                : sign * CompareNoLeadingZeroes(0, magnitude, 0, value.magnitude);
        }



        public override bool Equals(
            object obj)
        {
            if (obj == this)
                return true;

            BigInteger biggie = obj as BigInteger;
            if (biggie == null)
                return false;

            if (biggie.sign != sign || biggie.magnitude.Length != magnitude.Length)
                return false;

            for (int i = 0; i < magnitude.Length; i++)
            {
                if (biggie.magnitude[i] != magnitude[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hc = magnitude.Length;
            if (magnitude.Length > 0)
            {
                hc ^= magnitude[0];

                if (magnitude.Length > 1)
                {
                    hc ^= magnitude[magnitude.Length - 1];
                }
            }

            return sign < 0 ? ~hc : hc;
        }

        // TODO Make public?
        private BigInteger Inc()
        {
            if (this.sign == 0)
                return One;

            if (this.sign < 0)
                return new BigInteger(-1, doSubBigLil(this.magnitude, One.magnitude), true);

            return AddToMagnitude(One.magnitude);
        }


        public BigInteger Negate()
        {
            if (sign == 0)
                return this;

            return new BigInteger(-sign, magnitude, false);
        }

        public BigInteger Not()
        {
            return Inc().Negate();
        }


        /**
         * returns x = x - y - we assume x is >= y
         */
        private static int[] Subtract(
            int		xStart,
            int[]	x,
            int		yStart,
            int[]	y)
        {
            int iT = x.Length;
            int iV = y.Length;
            long m;
            int borrow = 0;

            do
            {
                m = (x[--iT] & IMASK) - (y[--iV] & IMASK) + borrow;
                x[iT] = (int) m;

//				borrow = (m < 0) ? -1 : 0;
                borrow = (int)(m >> 63);
            }
            while (iV > yStart);

            if (borrow != 0)
            {
                while (--x[--iT] == -1)
                {
                }
            }

            return x;
        }

        private static int[] doSubBigLil(
            int[]	bigMag,
            int[]	lilMag)
        {
            int[] res = (int[]) bigMag.Clone();

            return Subtract(0, res, 0, lilMag);
        }

        public byte[] ToByteArray()
        {
            return ToByteArray(false);
        }

        public byte[] ToByteArrayUnsigned()
        {
            return ToByteArray(true);
        }

        private byte[] ToByteArray(
            bool unsigned)
        {
            unchecked { 

            if (sign == 0)
                return unsigned ? ZeroEncoding : new byte[1];

            int nBits = (unsigned && sign > 0)
                ?	BitLength
                :	BitLength + 1;

            int nBytes = GetByteLength(nBits);
            byte[] bytes = new byte[nBytes];

            int magIndex = magnitude.Length;
            int bytesIndex = bytes.Length;

            if (sign > 0)
            {
                while (magIndex > 1)
                {
                    uint mag = (uint) magnitude[--magIndex];
                    bytes[--bytesIndex] = (byte) mag;
                    bytes[--bytesIndex] = (byte)(mag >> 8);
                    bytes[--bytesIndex] = (byte)(mag >> 16);
                    bytes[--bytesIndex] = (byte)(mag >> 24);
                }

                uint lastMag = (uint) magnitude[0];
                while (lastMag > byte.MaxValue)
                {
                    bytes[--bytesIndex] = (byte) lastMag;
                    lastMag >>= 8;
                }

                bytes[--bytesIndex] = (byte) lastMag;
            }
            else // sign < 0
            {
                bool carry = true;

                while (magIndex > 1)
                {
                    uint mag = ~((uint) magnitude[--magIndex]);

                    if (carry)
                    {
                        carry = (++mag == uint.MinValue);
                    }

                    bytes[--bytesIndex] = (byte) mag;
                    bytes[--bytesIndex] = (byte)(mag >> 8);
                    bytes[--bytesIndex] = (byte)(mag >> 16);
                    bytes[--bytesIndex] = (byte)(mag >> 24);
                }

                uint lastMag = (uint) magnitude[0];

                if (carry)
                {
                    // Never wraps because magnitude[0] != 0
                    --lastMag;
                }

                while (lastMag > byte.MaxValue)
                {
                    bytes[--bytesIndex] = (byte) ~lastMag;
                    lastMag >>= 8;
                }

                bytes[--bytesIndex] = (byte) ~lastMag;

                if (bytesIndex > 0)
                {
                    bytes[--bytesIndex] = byte.MaxValue;
                }
            }

            return bytes;
            }
        }

        private static BigInteger createUValueOf(
            ulong value)
        {
            int msw = (int)(value >> 32);
            int lsw = (int)value;

            if (msw != 0)
                return new BigInteger(1, new int[] { msw, lsw }, false);

            if (lsw != 0)
            {
                BigInteger n = new BigInteger(1, new int[] { lsw }, false);
                return n;
            }

            return Zero;
        }

        private static BigInteger createValueOf(
            long value)
        {
            if (value < 0)
            {
                if (value == long.MinValue)
                    return createValueOf(~value).Not();

                return createValueOf(-value).Negate();
            }

            return createUValueOf((ulong)value);
        }

        public static BigInteger ValueOf(
            long value)
        {
            switch (value)
            {
                case 0:
                    return Zero;
                case 1:
                    return One;
                case 2:
                    return Two;
                case 3:
                    return Three;
                case 10:
                    return Ten;
            }

            return createValueOf(value);
        }
     }
}
