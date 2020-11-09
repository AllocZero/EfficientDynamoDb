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
using System.Text;

namespace Amazon.Runtime
{
    public class StreamTransferProgressArgs : EventArgs
    {
        long _incrementTransferred;
        long _total;
        long _transferred;

        /// <summary>
        /// The constructor takes the number of
        /// currently transferred bytes and the
        /// total number of bytes to be transferred
        /// </summary>
        /// <param name="incrementTransferred">The number of bytes transferred since last event</param>
        /// <param name="transferred">The number of bytes transferred</param>
        /// <param name="total">The total number of bytes to be transferred</param>
        public StreamTransferProgressArgs(long incrementTransferred, long transferred, long total)
        {
            this._incrementTransferred = incrementTransferred;
            this._transferred = transferred;
            this._total = total;
        }

        /// <summary>
        /// Gets the percentage of transfer completed
        /// </summary>
        public int PercentDone
        {
            get { return (int)((_transferred * 100) / _total); }
        }

        /// <summary>
        /// Gets the number of bytes transferred since last event
        /// </summary>
        public long IncrementTransferred
        {
            get { return this._incrementTransferred; }
        }


        /// <summary>
        /// Gets the number of bytes transferred
        /// </summary>
        public long TransferredBytes
        {
            get { return _transferred; }
        }

        /// <summary>
        /// Gets the total number of bytes to be transferred
        /// </summary>
        public long TotalBytes
        {
            get { return _total; }
        }

        /// <summary>
        /// Returns a string representation of this object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                "Transfer Statistics. Percentage completed: ",
                PercentDone,
                ", Bytes transferred: ",
                _transferred,
                ", Total bytes to transfer: ",
                _total
                );
        }    
    }
}
