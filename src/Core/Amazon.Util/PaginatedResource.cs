/*******************************************************************************
 *  Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 *  this file except in compliance with the License. A copy of the License is located at
 *
 *  http://aws.amazon.com/apache2.0
 *
 *  or in the "license" file accompanying this file.
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the
 *  specific language governing permissions and limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Amazon.Util
{
    internal class PaginatedResource<U> : IEnumerable<U>
    {
        internal Func<string, Marker<U>> fetcher;

        internal PaginatedResource(Func<string, Marker<U>> fetcher)
        {
            this.fetcher = fetcher;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public IEnumerator<U> GetEnumerator()
        {
            return new PaginationEnumerator<U>(this);
        }
        
    }

    internal class Marker<U>
    {
        private List<U> data;
        private string nextToken;

        internal Marker(List<U> data, string nextToken)
        {
            this.data = data;
            this.nextToken = nextToken;
        }

        internal List<U> Data
        {
            get { return this.data; }
        }

        internal string NextToken
        {
            get { return this.nextToken; }
        }
    }

    internal class PaginationEnumerator<U> : IEnumerator<U>
    {
        private PaginatedResource<U> paginatedResource;
        private int position = -1;
        private static Marker<U> blankSpot = new Marker<U>(new List<U>(), (string)null);
        private Marker<U> currentSpot = blankSpot;
        bool started = false;

        internal PaginationEnumerator(PaginatedResource<U> paginatedResource)
        {
            this.paginatedResource = paginatedResource;
        }

        public bool MoveNext()
        {
            position++;
            while (position == currentSpot.Data.Count)
            {
                if (!started || !string.IsNullOrEmpty(currentSpot.NextToken))
                {
                    currentSpot = paginatedResource.fetcher(currentSpot.NextToken);
                    position = 0;
                    started = true;
                }
                else
                {
                    currentSpot = blankSpot;
                    position = -1;
                }
            }
            return (position != -1);
        }

        public void Reset()
        {
            position = -1;
            currentSpot = new Marker<U>(new List<U>(), (string)null);
            started = false;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public U Current
        {
            get
            {
                try
                {
                    return currentSpot.Data[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
