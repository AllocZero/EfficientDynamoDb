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

using Amazon.Util;
using System.Globalization;

namespace Amazon.Auth.AccessControlPolicy
{
    /// <summary>
    /// A factory for creating conditions to be used in the policy.
    /// </summary>
    public static partial class ConditionFactory
    {

        #region Common Keys
        /// <summary>
        /// Condition key for the current time.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.DateComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string CURRENT_TIME_CONDITION_KEY = "aws:CurrentTime";

        /// <summary>
        /// Condition key for whether or not an incoming request is using a secure
        /// transport to make the request (i.e. HTTPS instead of HTTP).
        /// <para>
        /// This condition key should only be used with the boolean overload of NewCondition.
        /// </para>
        /// </summary>
        public const string SECURE_TRANSPORT_CONDITION_KEY = "aws:SecureTransport";

        /// <summary>
        /// Condition key for the source IP from which a request originates.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.IpAddressComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string SOURCE_IP_CONDITION_KEY = "aws:SourceIp";

        /// <summary>
        /// Condition key for the user agent included in a request.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/>
        /// enum.
        /// </para>
        /// </summary>
        public const string USER_AGENT_CONDITION_KEY = "aws:UserAgent";

        /// <summary>
        /// Condition key for the current time, in epoch seconds.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.NumericComparisonType"/> enum.
        /// objects.
        /// </para>
        /// </summary>
        public const string EPOCH_TIME_CONDITION_KEY = "aws:EpochTime";

        /// <summary>
        /// Condition key for the referrer specified by a request.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/>
        /// objects.
        /// </para>
        /// </summary>
        public const string REFERRER_CONDITION_KEY = "aws:Referer";

        /// <summary>
        /// Condition key for the Amazon Resource Name (ARN) of the source specified
        /// in a request. The source ARN indicates which resource is affecting the
        /// resource listed in your policy. For example, an SNS topic is the source
        /// ARN when publishing messages from the topic to an SQS queue.
        /// <para>
        /// This condition key should only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.ArnComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string SOURCE_ARN_CONDITION_KEY = "aws:SourceArn";
        #endregion

        #region ArnComparisonType
        /// <summary>
        /// Enumeration of the supported ways an ARN comparison can be evaluated.
        /// </summary>
        public enum ArnComparisonType 
        {
            /// <summary>Exact matching</summary>
            ArnEquals,

            /// <summary>
            /// Loose case-insensitive matching of the ARN. Each of the six
            /// colon-delimited components of the ARN is checked separately and each
            /// can include a multi-character match wildcard (*) or a
            /// single-character match wildcard (?).
            /// </summary>
            ArnLike,

            /// <summary>Negated form of ArnEquals</summary>
            ArnNotEquals,

            /// <summary>Negated form of ArnLike</summary>
            ArnNotLike
        };
        #endregion

        #region DateComparisonType
        /// <summary>
        /// Enumeration of the supported ways a date comparison can be evaluated.
        /// </summary>
        public enum DateComparisonType
        {
            DateEquals,
            DateGreaterThan,
            DateGreaterThanEquals,
            DateLessThan,
            DateLessThanEquals,
            DateNotEquals
        };
        #endregion

        #region IpAddressComparisonType
        /// <summary>
        /// Enumeration of the supported ways an IP address comparison can be evaluated.
        /// </summary>
        public enum IpAddressComparisonType
        {
            /// <summary>
            /// Matches an IP address against a CIDR IP range, evaluating to true if
            /// the IP address being tested is in the condition's specified CIDR IP
            /// range.
            /// </summary>
            IpAddress,

            /// <summary>
            /// Negated form of IpAddress
            /// </summary>
            NotIpAddress
        }
        #endregion

        #region NumericComparisonType
        /// <summary>
        /// Enumeration of the supported ways a numeric comparison can be evaluated
        /// </summary>
        public enum NumericComparisonType
        {
            NumericEquals,
            NumericGreaterThan,
            NumericGreaterThanEquals,
            NumericLessThan,
            NumericLessThanEquals,
            NumericNotEquals
        };
        #endregion

        #region StringComparisonType
        /// <summary>
        /// Enumeration of the supported ways a string comparison can be evaluated.
        /// </summary>
        public enum StringComparisonType 
        {
            /// <summary>
            /// Case-sensitive exact string matching
            /// </summary>
            StringEquals,

            /// <summary>
            /// Case-insensitive string matching
            /// </summary>
            StringEqualsIgnoreCase,

            /// <summary>
            /// Loose case-insensitive matching. The values can include a
            /// multi-character match wildcard (*) or a single-character match
            /// wildcard (?) anywhere in the string.
            /// </summary>
            StringLike,

            /// <summary>
            /// Negated form of StringEquals.
            /// </summary>
            StringNotEquals,

            /// <summary>
            /// Negated form of StringEqualsIgnorecase.
            /// </summary>
            StringNotEqualsIgnoreCase,

            /// <summary>
            /// Negated form of StringLike.
            /// </summary>
            StringNotLike
        }
        #endregion

        /// <summary>
        /// Constructs a new access control policy condition that compares ARNs (Amazon Resource Names).
        /// </summary>
        /// <param name="key"> The access policy condition key specifying where to get the first ARN for the comparison</param>
        /// <param name="type">The type of comparison to perform.</param>
        /// <param name="value">The second ARN to compare against. When using ArnLike or ArnNotLike this may contain the
        ///     multi-character wildcard (*) or the single-character wildcard</param>
        public static Condition NewCondition(ArnComparisonType type, string key, string value) 
        {
            return new Condition(type.ToString(), key, value);
        }

        /// <summary>
        /// Constructs a new access policy condition that performs a boolean
        /// comparison.
        /// </summary>
        /// <param name="key">The access policy condition key specifying where to get the
        ///            first boolean value for the comparison (ex: aws:SecureTransport).</param>
        /// <param name="value">The boolean to compare against.</param>
        public static Condition NewCondition(string key, bool value)
        {
            return new Condition("Bool", key, value.ToString().ToLowerInvariant());
        }

        /// <summary>
        /// This method is deprecated. Invoking this method results in non-UTC DateTimes
        /// not being marshalled correctly. Use NewConditionUtc instead.
        /// Constructs a new access policy condition that compares the current time
        /// (on the AWS servers) to the specified date.
        /// </summary>
        /// <param name="type">The type of comparison to perform. For example,
        ///            DateComparisonType.DateLessThan will cause this policy
        ///            condition to evaluate to true if the current date is less than
        ///            the date specified in the second argument.</param>
        /// <param name="date">The date to compare against.</param>
        [Obsolete("Invoking this method results in non-UTC DateTimes not being marshalled correctly. Use NewConditionUtc instead.", false)]
        public static Condition NewCondition(DateComparisonType type, DateTime date)
        {
            return new Condition(type.ToString(), CURRENT_TIME_CONDITION_KEY, date.ToString(AWSSDKUtils.ISO8601DateFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Constructs a new access policy condition that compares the current time
        /// (on the AWS servers) to the specified date.
        /// </summary>
        /// <param name="type">The type of comparison to perform. For example,
        ///            DateComparisonType.DateLessThan will cause this policy
        ///            condition to evaluate to true if the current date is less than
        ///            the date specified in the second argument.</param>
        /// <param name="date">The date to compare against.</param>
        public static Condition NewConditionUtc(DateComparisonType type, DateTime date)
        {
            return new Condition(type.ToString(), CURRENT_TIME_CONDITION_KEY, date.ToUniversalTime().ToString(AWSSDKUtils.ISO8601DateFormat, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Constructs a new access policy condition that compares the source IP
        /// address of the incoming request to an AWS service against the specified
        /// CIDR range. The condition evaluates to true (meaning the policy statement
        /// containing it will be applied) if the incoming source IP address is
        /// within that range.
        /// <para>
        /// To achieve the opposite effect (i.e. cause the condition to evaluate to
        /// true when the incoming source IP is <b>not</b> in the specified CIDR
        /// range) use the alternate constructor form and specify
        /// IpAddressComparisonType.NotIpAddress.
        /// </para>
        /// </summary>
        /// <param name="ipAddressRange">The CIDR IP range involved in the policy condition.</param>
        public static Condition NewIpAddressCondition(string ipAddressRange)
        {
            return NewCondition(IpAddressComparisonType.IpAddress, ipAddressRange);
        }

        /// <summary>
        /// Constructs a new access policy condition that compares the source IP
        /// address of the incoming request to an AWS service against the specified
        /// CIDR range. When the condition evaluates to true (i.e. when the incoming
        /// source IP address is within the CIDR range or not) depends on the
        /// specified IpAddressComparisonType.
        /// </summary>
        /// <param name="type">The type of comparison to to perform.</param>
        /// <param name="ipAddressRange">The CIDR IP range involved in the policy condition.</param>
        public static Condition NewCondition(IpAddressComparisonType type, string ipAddressRange)
        {
            return new Condition(type.ToString(), SOURCE_IP_CONDITION_KEY, ipAddressRange);
        }

        /// <summary>
        /// Constructs a new access policy condition that compares two numbers.
        /// </summary>
        /// <param name="type">The type of comparison to perform.</param>
        /// <param name="key">The access policy condition key specifying where to get the
        ///           first number for the comparison.</param>
        /// <param name="value">The second number to compare against.</param>
        public static Condition NewCondition(NumericComparisonType type, string key, string value)
        {
            return new Condition(type.ToString(), key, value);
        }

        /// <summary>
        /// Constructs a new access control policy condition that compares two
        /// strings.
        /// </summary>
        /// <param name="type">The type of comparison to perform</param>
        /// <param name="key">The access policy condition key specifying where to get the
        ///            first string for the comparison (ex: aws:UserAgent). 
        /// </param>
        /// <param name="value">The second string to compare against. When using
        ///            StringComparisonType.StringLike or
        ///            StringComparisonType.StringNotLike this may contain
        ///            the multi-character wildcard (*) or the single-character
        ///            wildcard (?).
        /// </param>
        public static Condition NewCondition(StringComparisonType type, string key, string value)
        {
            return new Condition(type.ToString(), key, value);
        }

        /// <summary>
        /// Constructs a new access policy condition that compares the Amazon
        /// Resource Name (ARN) of the source of an AWS resource that is modifying
        /// another AWS resource with the specified pattern.
        /// <para>
        /// For example, the source ARN could be an Amazon SNS topic ARN that is
        /// sending messages to an Amazon SQS queue. In that case, the SNS topic ARN
        /// would be compared the ARN pattern specified here.
        /// </para>
        /// <para>
        /// The endpoint pattern may optionally contain the multi-character wildcard
        ///* (*) or the single-character wildcard (?). Each of the six colon-delimited
        /// components of the ARN is checked separately and each can include a
        /// wildcard.
        /// </para>
        /// <code>
        /// Policy policy = new Policy("MyQueuePolicy");
        /// policy.WithStatements(new Statement(Statement.StatementEffect.Allow)
        ///     .WithPrincipals(new Principal("*")).WithActionIdentifiers(SQSActionIdentifiers.SendMessage)
        ///     .WithResources(new Resource(myQueueArn))
        ///     .WithConditions(ConditionFactory.NewSourceArnCondition(myTopicArn)));
        /// </code>
        /// </summary>
        /// <param name="arnPattern">The ARN pattern against which the source ARN will be compared.
        ///     Each of the six colon-delimited components of the ARN is
        ///     checked separately and each can include a wildcard.</param>
        /// <returns>A new access control policy condition that compares the ARN of
        ///        the source specified in an incoming request with the ARN pattern
        ///        specified here.</returns>
        public static Condition NewSourceArnCondition(string arnPattern)
        {
            return NewCondition(ArnComparisonType.ArnLike, SOURCE_ARN_CONDITION_KEY, arnPattern);
        }

        /// <summary>
        /// Constructs a new access control policy condition that tests if the
        /// incoming request was sent over a secure transport (HTTPS).
        /// </summary>
        /// <returns>A new access control policy condition that tests if the incoming
        ///                 request was sent over a secure transport (HTTPS).</returns>
        public static Condition NewSecureTransportCondition()
        {
            return NewCondition(SECURE_TRANSPORT_CONDITION_KEY, true);
        }
    }
}
