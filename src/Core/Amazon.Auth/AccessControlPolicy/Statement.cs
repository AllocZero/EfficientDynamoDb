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
    /// A statement is the formal description of a single permission, and is always
    /// contained within a policy object.
    /// <para>
    /// A statement describes a rule for allowing or denying access to a specific AWS
    /// resource based on how the resource is being accessed, and who is attempting
    /// to access the resource. Statements can also optionally contain a list of
    /// conditions that specify when a statement is to be honored.
    /// </para>
    /// <para>
    /// For example, consider a statement that:
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
    /// There are many resources and conditions available for use in statements, and
    /// you can combine them to form fine grained custom access control polices.
    /// </para>
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public class Statement 
    {
        private string id;
        private StatementEffect effect;
        private IList<Principal> principals = new List<Principal>();
        private IList<ActionIdentifier> actions = new List<ActionIdentifier>();
        private IList<Resource> resources = new List<Resource>();
        private IList<Condition> conditions = new List<Condition>();

        /// <summary>
        /// The effect is the result that you want a policy statement to return at
        /// evaluation time. A policy statement can either allow access or explicitly
        /// deny access.
        /// </summary>
        public enum StatementEffect { Allow, Deny }

        /// <summary>
        /// Constructs a new access control policy statement with the specified
        /// effect.
        /// <para>
        /// Before a statement is valid and can be sent to AWS, callers must set the
        /// principals, resources, and actions (as well as any optional conditions)
        /// involved in the statement.
        /// </para>
        /// </summary>
        /// <param name="effect">The effect this statement has (allowing access or denying
        ///            access) when all conditions, resources, principals, and
        ///            actions are matched.</param>
        public Statement(StatementEffect effect) 
        {
            this.effect = effect;
            this.id = Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// Gets and Sets the ID for this statement. Statement IDs serve to help keep track
        /// of multiple statements, and are often used to give the statement a
        /// meaningful, human readable name.
        /// <para>
        /// Developers should be careful to not use the same statement ID for
        /// multiple statements in the same policy. Reusing the same statement ID in
        /// different policies is not a problem.
        /// </para>
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
        /// Sets the ID for this statement and returns the updated statement so
        /// multiple calls can be chained together.
        /// <para>
        /// Statement IDs serve to help keep track of multiple statements, and are
        /// often used to give the statement a meaningful, human readable name.
        /// </para>
        /// <para>
        /// Developers should be careful to not use the same statement ID for
        /// multiple statements in the same policy. Reusing the same statement ID in
        /// different policies is not a problem.
        /// </para>
        /// </summary>
        /// <param name="id">The new statement ID for this statement.</param>
        /// <returns>this instance</returns>
        public Statement WithId(string id) 
        {
            this.Id = id;
            return this;
        }

        /// <summary>
        /// Gets and Sets the result effect of this policy statement when it is evaluated.
        /// A policy statement can either allow access or explicitly
        /// </summary>
        public StatementEffect Effect 
        {
            get
            {
                return effect;
            }
            set
            {
                this.effect = value;
            }
        }

        /// <summary>
        /// Gets and Sets the list of actions to which this policy statement applies.
        /// Actions limit a policy statement to specific service operations that are
        /// being allowed or denied by the policy statement. For example, you might
        /// want to allow any AWS user to post messages to your SQS queue using the
        /// SendMessage action, but you don't want to allow those users other actions
        /// such as ReceiveMessage or DeleteQueue.
        /// </summary>
        public IList<ActionIdentifier> Actions 
        {
            get
            {
                return actions;
            }
            set
            {
                this.actions = value;
            }
        }

       /// <summary>
        /// Sets the list of actions to which this policy statement applies and
        /// returns this updated Statement object so that additional method calls can
        /// be chained together.
        /// <para>
        /// Actions limit a policy statement to specific service operations that are
        /// being allowed or denied by the policy statement. For example, you might
        /// want to allow any AWS user to post messages to your SQS queue using the
        /// SendMessage action, but you don't want to allow those users other actions
        /// such as ReceiveMessage or DeleteQueue.
        /// </para>
        /// </summary>
        /// <param name="actions">The list of actions to which this statement applies.</param>
        /// <returns>this instance</returns>
        public Statement WithActionIdentifiers(params ActionIdentifier[] actions) 
        {
            if (this.actions == null)
            {
                this.actions = new List<ActionIdentifier>();
            }
            foreach (ActionIdentifier element in actions)
            {
                this.actions.Add(element);
            }

            return this;
        }

        /// <summary>
        /// Gets and Sets the resources associated with this policy statement. Resources
        /// are what a policy statement is allowing or denying access to, such as an
        /// Amazon SQS queue or an Amazon SNS topic.
        /// <para>
        /// Note that some services allow only one resource to be specified per
        /// policy statement.
        /// </para>
        /// </summary>
        public IList<Resource> Resources
        {
            get
            {
                return resources;
            }
            set
            {
                this.resources = value;
            }
        }

        /// <summary>
        /// Sets the resources associated with this policy statement and returns this
        /// updated Statement object so that additional method calls can be chained
        /// together.
        /// <para>
        /// Resources are what a policy statement is allowing or denying access to,
        /// such as an Amazon SQS queue or an Amazon SNS topic.
        /// </para>
        /// <para>
        /// Note that some services allow only one resource to be specified per
        /// policy statement.
        /// </para>
        /// </summary>
        /// <param name="resources">The resources associated with this policy statement.</param>
        /// <returns>this instance</returns>
        public Statement WithResources(params Resource[] resources) 
        {
            if (this.resources == null)
            {
                this.resources = new List<Resource>();
            }
            foreach(Resource element in resources)
            {
                this.resources.Add(element);
            }

            return this;
        }

        /// <summary>
        /// Gets and Sets the conditions associated with this policy statement. Conditions
        /// allow policy statements to be conditionally evaluated based on the many
        /// available condition types.
        /// <para>
        /// For example, a statement that allows access to an Amazon SQS queue could
        /// use a condition to only apply the effect of that statement for requests
        /// that are made before a certain date, or that originate from a range of IP
        /// addresses.
        /// </para>
        /// <para>
        /// When multiple conditions are included in a single statement, all
        /// conditions must evaluate to true in order for the statement to take
        /// effect.
        /// </para>
        /// </summary>
        public IList<Condition> Conditions
        {
            get
            {
                return conditions;
            }
            set
            {
                this.conditions = value;
            }
        }

        /// <summary>
        /// Sets the conditions associated with this policy statement, and returns
        /// this updated Statement object so that additional method calls can be
        /// chained together.
        /// <para>
        /// Conditions allow policy statements to be conditionally evaluated based on
        /// the many available condition types.
        /// </para>
        /// <para>
        /// For example, a statement that allows access to an Amazon SQS queue could
        /// use a condition to only apply the effect of that statement for requests
        /// that are made before a certain date, or that originate from a range of IP
        /// addresses.
        /// </para>
        /// <para>
        /// Multiple conditions can be included in a single statement, and all
        /// conditions must evaluate to true in order for the statement to take
        /// effect.
        /// </para>
        /// </summary>
        /// <param name="conditions">The conditions associated with this policy statement.</param>
        /// <returns>this instance</returns>
        public Statement WithConditions(params Condition[] conditions) 
        {
            if (this.Conditions == null)
            {
                this.Conditions = new List<Condition>();
            }
            foreach(Condition element in conditions)
            {
                this.Conditions.Add(element);
            }

            return this;
        }

        /// <summary>
        /// Gets and Sets the principals associated with this policy statement, indicating
        /// which AWS accounts are affected by this policy statement.
        /// </summary>
        public IList<Principal> Principals
        {
            get
            {
                return principals;
            }
            set
            {
                this.principals = value;
            }
        }

        /// <summary>
        /// Sets the principals associated with this policy statement, and returns
        /// this updated Statement object. Principals control which AWS accounts are
        /// affected by this policy statement.
        /// <para>
        /// If you don't want to restrict your policy to specific users, you can use
        /// <see cref="Amazon.Auth.AccessControlPolicy.Principal.AllUsers"/> to apply the policy to any user trying to
        /// access your resource.
        /// </para>
        /// </summary>
        /// <param name="principals">The list of principals associated with this policy statement.</param>
        /// <returns>this instance</returns>
        public Statement WithPrincipals(params Principal[] principals) 
        {
            if (this.principals == null)
            {
                this.principals = new List<Principal>();
            }
            foreach (Principal element in principals)
            {
                this.principals.Add(element);
            }

            return this;
        }

    }
}

