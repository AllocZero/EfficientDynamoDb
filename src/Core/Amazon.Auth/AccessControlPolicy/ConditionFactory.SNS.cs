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

namespace Amazon.Auth.AccessControlPolicy
{
    public static partial class ConditionFactory
    {
        /// <summary>
        /// Condition key for The URL, e-mail address, or ARN from a Subscribe
        /// request or a previously confirmed subscription. Use with string
        /// conditions to restrict access to specific endpoints (e.g.,
        /// *@mycompany.com).
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string SNS_ENDPOINT_CONDITION_KEY = "sns:Endpoint";

        /// <summary>
        /// Condition key for the protocol value from a Subscribe request or a
        /// previously confirmed subscription. Use with string conditions to restrict
        /// publication to specific delivery protocols (e.g., HTTPS).
        /// <para>
        /// This condition key may only be used with <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.StringComparisonType"/> enum.
        /// </para>
        /// </summary>
        public const string SNS_PROTOCOL_CONDITION_KEY = "sns:Protocol";

        /// <summary>
        /// Constructs a new access policy condition that compares the requested
        /// endpoint used to subscribe to an Amazon SNS topic with the specified
        /// endpoint pattern. The endpoint pattern may optionally contain the
        /// multi-character wildcard (*) or the single-character wildcard (?).
        /// <para>
        /// For example, this condition can restrict subscriptions to a topic to
        /// email addresses in a certain domain ("*@my-company.com").
        /// </para>
        /// <code>
        /// Policy policy = new Policy("MyTopicPolicy");
        /// policy.WithStatements(new Statement(Statement.StatementEffect.Allow)
        ///        .WithPrincipals(new Principal("*")).WithActionIdentifiers(SNSActionIdentifiers.Subscribe)
        ///        .WithResources(new Resource(myTopicArn))
        ///        .WithConditions(ConditionFactory.NewEndpointCondition("*@my-company.com")));
        /// </code>
        /// </summary>
        /// <param name="endpointPattern">The endpoint pattern against which to compare the requested
        ///            endpoint for an Amazon SNS topic subscription.</param>
        /// <returns>A new access control policy condition that compares the endpoint
        ///         used in a request to subscribe to an Amazon SNS topic with the
        ///         endpoint pattern specified.</returns>
        public static Condition NewEndpointCondition(string endpointPattern) 
        {
            return ConditionFactory.NewCondition(StringComparisonType.StringLike,
                    SNS_ENDPOINT_CONDITION_KEY, endpointPattern);
        }

        /// <summary>
        /// Constructs a new AWS access control policy condition that allows an
        /// access control statement to restrict subscriptions to an Amazon SNS topic
        /// based on the protocol being used for the subscription. For example, this
        /// condition can restrict subscriptions to a topic to endpoints using HTTPS
        /// to ensure that messages are securely delivered.
        /// </summary>
        /// <param name="protocol">The protocol against which to compare the requested protocol
        ///            for an Amazon SNS topic subscription.</param>
        /// <returns>A new access control policy condition that compares the
        ///         notification protocol requested in a request to subscribe to an
        ///         Amazon SNS topic with the protocol value specified.</returns>
        public static Condition NewProtocolCondition(string protocol)
        {
            return ConditionFactory.NewCondition(StringComparisonType.StringEquals,
                    SNS_PROTOCOL_CONDITION_KEY, protocol);
        }
    }
}

