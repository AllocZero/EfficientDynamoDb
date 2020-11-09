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
using System.Collections.Generic;
using System.Text;

using Amazon.Auth.AccessControlPolicy.Internal;

namespace Amazon.Auth.AccessControlPolicy
{
    /// <summary>
    /// An AWS access control policy is a object that acts as a container for one or
    /// more statements, which specify fine grained rules for allowing or denying
    /// various types of actions from being performed on your AWS resources.
    /// <para>
    /// By default, all requests to use your resource coming from anyone but you are
    /// denied. Access control polices can override that by allowing different types
    /// of access to your resources, or by explicitly denying different types of
    /// access.
    /// </para>
    /// <para>
    /// Each statement in an AWS access control policy takes the form:
    /// "A has permission to do B to C where D applies".
    /// <list type="definition">
    ///     <item>
    ///         <term>A is the prinicpal</term>
    ///         <description>The AWS account that is making a request to
    ///             access or modify one of your AWS resources.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>B is the action</term>
    ///         <description>the way in which your AWS resource is being accessed or modified, such
    ///             as sending a message to an Amazon SQS queue, or storing an object in an Amazon S3 bucket.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>C is the resource</term>
    ///         <description>your AWS entity that the principal wants to access, such
    ///             as an Amazon SQS queue, or an object stored in Amazon S3.
    ///         </description>
    ///     </item>
    ///     <item>
    ///         <term>D is the set of conditions</term>
    ///         <description>optional constraints that specify when to allow or deny
    ///             access for the principal to access your resource.  Many expressive conditions are available,
    ///             some specific to each service.  For example you can use date conditions to allow access to
    ///             your resources only after or before a specific time.
    ///         </description>
    ///     </item>
    /// </list>
    /// </para>
    /// <para>
    /// Note that an AWS access control policy should not be confused with the
    /// similarly named "POST form policy" concept used in Amazon S3.
    /// </para>
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public class Policy
    {

        /// <summary>
        /// The default policy version
        /// </summary>
        private const string DEFAULT_POLICY_VERSION = "2012-10-17";

        private string id;
        private string version = DEFAULT_POLICY_VERSION;
        private IList<Statement> statements = new List<Statement>();

        /// <summary>
        /// Constructs an empty AWS access control policy ready to be populated with
        /// statements.
        /// </summary>
        public Policy()
        {
        }

        /// <summary>
        /// Constructs a new AWS access control policy with the specified policy ID.
        /// The policy ID is a user specified string that serves to help developers
        /// keep track of multiple polices. Policy IDs are often used as a human
        /// readable name for a policy.
        /// </summary>
        /// <param name="id">The policy ID for the new policy object. Policy IDs serve to
        ///            help developers keep track of multiple policies, and are often
        ///            used to give the policy a meaningful, human readable name.</param>
        public Policy(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Constructs a new AWS access control policy with the specified policy ID
        /// and collection of statements. The policy ID is a user specified string
        /// that serves to help developers keep track of multiple polices. Policy IDs
        /// are often used as a human readable name for a policy.
        /// </summary>
        /// <param name="id">The policy ID for the new policy object. Policy IDs serve to
        ///            help developers keep track of multiple policies, and are often
        ///            used to give the policy a meaningful, human readable name.</param>
        /// <param name="statements">The statements to include in the new policy.</param>
        public Policy(string id, IList<Statement> statements)
        {
            this.id = id;
            this.statements = statements;
        }

        /// <summary>
        /// Gets and Sets the policy ID for this policy. Policy IDs serve to help
        /// developers keep track of multiple policies, and are often used as human
        /// readable name for a policy.
        /// </summary>
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// Sets the policy ID for this policy and returns the updated policy so that
        /// multiple calls can be chained together.
        /// <para>
        /// Policy IDs serve to help developers keep track of multiple policies, and
        /// are often used as human readable name for a policy.
        /// </para>
        /// </summary>
        /// <paraparam name="id">The polich ID for this policy</paraparam>
        /// <returns>this instance</returns>
        public Policy WithId(string id)
        {
            Id = id;
            return this;
        }

        /// <summary>
        /// Gets and sets the version of this AWS policy.
        /// </summary>
        public string Version
        {
            get
            {
                return version;
            }
            set
            {
                this.version = value;
            }
        }

        /// <summary>
        /// Gets and Sets the collection of statements contained by this policy. Individual
        /// statements in a policy are what specify the rules that enable or disable
        /// access to your AWS resources.
        /// </summary>
        public IList<Statement> Statements
        {
            get
            {
                return statements;
            }
            set
            {
                this.statements = value;
            }
        }

        /// <summary>
        /// Checks to see if the permissions set in the statement are already set by another
        /// statement in the policy.
        /// </summary>
        /// <param name="statement">The statement to verify</param>
        /// <returns>True if the statement's permissions are already allowed by the statement</returns>
        public bool CheckIfStatementExists(Statement statement)
        {
            if (this.Statements == null)
                return false;

            
            foreach (var existingStatement in this.Statements)
            {
                if (existingStatement.Effect != statement.Effect)
                    continue;
                if(!StatementContainsResources(existingStatement, statement.Resources))
                    continue;
                if (!StatementContainsActions(existingStatement, statement.Actions))
                    continue;
                if (!StatementContainsConditions(existingStatement, statement.Conditions))
                    continue;
                if (!StatementContainsPrincipals(existingStatement, statement.Principals))
                    continue;

                return true;
            }


            return false;
        }

        private static bool StatementContainsResources(Statement statement, IList<Resource> resources)
        {
            foreach(var resource in resources)
            {
                if(statement.Resources.FirstOrDefault(x => string.Equals(x.Id, resource.Id)) == null)
                    return false;
            }

            return true;
        }

        private static bool StatementContainsActions(Statement statement, IList<ActionIdentifier> actions)
        {
            foreach (var action in actions)
            {
                if (statement.Actions.FirstOrDefault(x => string.Equals(x.ActionName, action.ActionName)) == null)
                    return false;
            }

            return true;
        }

        private static bool StatementContainsConditions(Statement statement, IList<Condition> conditions)
        {
            foreach (var condition in conditions)
            {
                if (statement.Conditions.FirstOrDefault(x => 
                    string.Equals(x.Type, condition.Type) && 
                    string.Equals(x.ConditionKey, condition.ConditionKey) &&
                    x.Values.Intersect(condition.Values).Count() == condition.Values.Count()) == null)
                    return false;
            }

            return true;
        }

        private static bool StatementContainsPrincipals(Statement statement, IList<Principal> principals)
        {
            foreach (var principal in principals)
            {
                if (statement.Principals.FirstOrDefault(x =>
                    string.Equals(x.Id, principal.Id) &&
                    string.Equals(x.Provider, principal.Provider)) == null)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Sets the collection of statements contained by this policy and returns
        /// this policy object so that additional method calls can be chained
        /// together.
        /// <para>
        /// Individual statements in a policy are what specify the rules that enable
        /// or disable access to your AWS resources.
        /// </para>
        /// </summary>
        /// <param name="statements">The collection of statements included in this policy.</param>
        /// <returns>this instance</returns>
        public Policy WithStatements(params Statement[] statements)
        {
            if (this.Statements == null)
            {
                this.Statements = new List<Statement>();
            }
            foreach (Statement element in statements)
            {
                this.Statements.Add(element);
            }

            return this;
        }

        /// <summary>
        /// Returns a JSON string representation of this AWS access control policy,
        /// suitable to be sent to an AWS service as part of a request to set an
        /// access control policy.
        /// </summary>
        /// <returns>A JSON string representation of this AWS access control policy.</returns>
        public string ToJson()
        {
            return ToJson(true);
        }

        /// <summary>
        /// Returns a JSON string representation of this AWS access control policy,
        /// suitable to be sent to an AWS service as part of a request to set an
        /// access control policy.
        /// </summary>
        /// <param name="prettyPrint">Toggle pretty print for the generated JSON document</param>
        /// <returns>A JSON string representation of this AWS access control policy.</returns>
        public string ToJson(bool prettyPrint)
        {
            return JsonPolicyWriter.WritePolicyToString(prettyPrint, this);
        }

        /// <summary>
        /// Parses a JSON document of a policy and creates a Policy object.
        /// </summary>
        /// <param name="json">JSON document of a policy.</param>
        /// <returns></returns>
        public static Policy FromJson(string json)
        {
            return JsonPolicyReader.ReadJsonStringToPolicy(json);
        }
    }
}

