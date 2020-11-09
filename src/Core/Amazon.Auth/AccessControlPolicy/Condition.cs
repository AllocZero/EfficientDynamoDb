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
    /// <summary>
    /// AWS access control policy conditions are contained in <see cref="Amazon.Auth.AccessControlPolicy.Statement"/>
    /// objects, and affect when a statement is applied. For example, a statement
    /// that allows access to an Amazon SQS queue could use a condition to only apply
    /// the effect of that statement for requests that are made before a certain
    /// date, or that originate from a range of IP addresses.
    /// <para>
    /// Multiple conditions can be included in a single statement, and all conditions
    /// must evaluate to true in order for the statement to take effect.
    /// </para>
    /// <para>
    /// The set of conditions is D in the statement
    /// "A has permission to do B to C where D applies."
    /// </para>
    /// <para>
    /// A condition is composed of three parts:
    /// <list type="definition">
    ///     <item>
    ///         <term>Condition Key</term>
    ///         <description>The condition key declares which value of a
    /// request to pull in and compare against when a policy is evaluated by AWS. For
    /// example, using <see cref="Amazon.Auth.AccessControlPolicy.ConditionFactory.SOURCE_IP_CONDITION_KEY"/> will cause
    /// AWS to pull in the current request's source IP as the first value to compare
    /// against every time your policy is evaluated.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Comparison Type</term>
    ///         <description>This is a static value used as the second value
    /// in the comparison when your policy is evaluated. Depending on the comparison
    /// type, this value can optionally use wildcards. See the documentation for
    /// individual comparison types for more information.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>Comparison Value</term>
    ///         <description>This is a static value used as the second value
    /// in the comparison when your policy is evaluated. Depending on the comparison
    /// type, this value can optionally use wildcards. See the documentation for
    /// individual comparison types for more information.
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public class Condition
    {
        private string type;
        private string conditionKey;
        private string[] values;

        public Condition()
        {
        }

        public Condition(string type, string conditionKey, params string[] values)
        {
            this.type = type;
            this.conditionKey = conditionKey;
            this.values = values;
        }

        /// <summary>
        /// Gets the type of this condition.
        /// </summary>
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                this.type = value;
            }
        }

        /// <summary>
        /// Gets and Sets the name of the condition key involved in this condition.
        /// Condition keys are predefined values supported by AWS that provide input
        /// to a condition's evaluation, such as the current time, or the IP address
        /// of the incoming request.
        /// <para>
        /// Your policy is evaluated for each incoming request, and condition keys
        /// specify what information to pull out of those incoming requests and plug
        /// into the conditions in your policy.
        /// </para>
        /// </summary>
        public string ConditionKey
        {
            get
            {
                return conditionKey;
            }
            set
            {
                this.conditionKey = value;
            }
        }

        /// <summary>
        /// Gets and Sets the values specified for this access control policy condition.
        /// For example, in a condition that compares the incoming IP address of a
        /// request to a specified range of IP addresses, the range of IP addresses
        /// is the single value in the condition.
        /// <para>
        /// Most conditions accept only one value, but multiple values are possible.
        /// </para>
        /// </summary>
        public string[] Values
        {
            get
            {
                return values;
            }
            set
            {
                this.values = value;
            }
        }
    }
}

