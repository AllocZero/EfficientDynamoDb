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
using System.Linq;
using System.Text;
using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;

namespace Amazon
{

    /// <summary>
    /// Utility class to parse Amazon resource names more commonly known as an ARN.
    /// </summary>
    public class Arn
    {
        /// <summary>
        /// Gets and sets the partition associated with the ARN (e.g.: 'aws').
        /// </summary>
        public string Partition { get; set; }

        /// <summary>
        /// Gets and sets the AWS service associated with the ARN (e.g.: 's3').
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Gets and sets the region associated with the ARN (e.g.: 'us-east-1').
        /// </summary>
        public string Region { get; set; }

        private string _accountId = string.Empty;
        /// <summary>
        /// Gets and sets the account ID associated with the ARN.
        /// </summary>
        public string AccountId 
        {
            get { return this._accountId; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    this._accountId = string.Empty;
                }
                else
                {
                    if (value.Length != 12 && value.ToCharArray().Any(x => !char.IsDigit(x)))
                    {
                        throw new AmazonClientException("AccountId is invalid. The AccountId length should be 12 and only contain numeric characters with no spaces or periods.");
                    }

                    this._accountId = value;
                }
            }
        }

        /// <summary>
        /// Gets and sets the resource associated with the ARN (e.g.: 'mybucket/myobject').
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Returns true if the string appears to be an ARN by seeing if the string starts with "arn:". This method
        /// doesn't guarantee the string is a valid ARN. To validate the string call TryParse. 
        /// </summary>
        /// <param name="arn"></param>
        /// <returns></returns>
        public static bool IsArn(string arn)
        {
            return arn != null && arn.StartsWith("arn:");
        }

        /// <summary>
        /// Parses the string into an ARN object.
        /// </summary>
        /// <param name="arnString">String to parse into an ARN.</param>
        /// <param name="arn">The out parameter for the ARN object created by TryParse.</param>
        /// <returns>True if the string was parsed into an ARN object.</returns>
        public static bool TryParse(string arnString, out Arn arn)
        {
            try
            {
                if (IsArn(arnString))
                {
                    arn = Parse(arnString);
                    return true;
                }
            }
            catch (ArgumentException) { }
            arn = null;
            return false;
        }

        /// <summary>
        /// Parses the string into an ARN object.
        /// </summary>
        /// <param name="arnString">String to parse into an Arn.</param>
        /// <returns>The Arn object created from the passed in string.</returns>
        /// <exception cref="ArgumentNullException">Thrown if arnString is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the string passed in not valid ARN.</exception>
        public static Arn Parse(string arnString)
        {
            if (arnString == null)
            {
                throw new ArgumentNullException(nameof(arnString));
            }

            const string malformedErrorMessage = "ARN is in incorrect format. ARN format is: arn:<partition>:<service>:<region>:<account-id>:<resource>";

            var tokens = arnString.Split(new char[] { ':' }, 6);
            if (tokens.Length != 6)
            {
                throw new ArgumentException(malformedErrorMessage);
            }

            if (tokens[0] != "arn")
            {
                throw new ArgumentException(malformedErrorMessage);
            }

            string partition = tokens[1];
            if (string.IsNullOrEmpty(partition))
            {
                throw new ArgumentException("Malformed ARN - no partition specified");
            }
            string service = tokens[2];
            if (string.IsNullOrEmpty(service))
            {
                throw new ArgumentException("Malformed ARN - no service specified");
            }

            string region = tokens[3];
            string accountId = tokens[4];
            string resource = tokens[5];
            if (string.IsNullOrEmpty(resource))
            {
                throw new ArgumentException("Malformed ARN - no resource specified");
            }

            return new Arn
            {
                Partition = partition,
                Service = service,
                Region = region,
                AccountId = accountId,
                Resource = resource
            };
        }

        /// <summary>
        /// Turns the Arn object into a string representation of an ARN.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("arn:");
            sb.Append(this.Partition);
            sb.Append(":");
            sb.Append(this.Service);
            sb.Append(":");
            sb.Append(this.Region);
            sb.Append(":");
            sb.Append(this.AccountId);
            sb.Append(":");
            sb.Append(this.Resource);
            return sb.ToString();
        }

        public override bool Equals(object o)
        {
            if (this == o)
            {
                return true;
            }

            var arn = o as Arn;
            if(arn == null)
            {
                return false;
            }

            if (!Partition.Equals(arn.Partition))
            {
                return false;
            }
            if (!Service.Equals(arn.Service))
            {
                return false;
            }
            if (Region != arn.Region)
            {
                return false;
            }
            if (AccountId != arn.AccountId)
            {
                return false;
            }
            return Resource.Equals(arn.Resource);
        }

        public override int GetHashCode()
        {
            return Hashing.Hash(Partition, Service, Region, AccountId, Resource);
        }
    }
}