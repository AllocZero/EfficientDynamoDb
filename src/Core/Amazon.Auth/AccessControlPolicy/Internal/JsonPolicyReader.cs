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
using System.IO;
using System.Collections.Generic;
using System.Text;

using ThirdParty.Json.LitJson;

namespace Amazon.Auth.AccessControlPolicy.Internal
{
    /// <summary>
    /// Deserializes a JSON string into a AWS policy object.
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    internal static class JsonPolicyReader
    {
        public static Policy ReadJsonStringToPolicy(string jsonString)
        {
            Policy policy = new Policy();
            JsonData jPolicy = JsonMapper.ToObject(jsonString);

            if (jPolicy[JsonDocumentFields.POLICY_ID] != null && jPolicy[JsonDocumentFields.POLICY_ID].IsString)
            {
                policy.Id = (string)jPolicy[JsonDocumentFields.POLICY_ID];
            }

            JsonData jStatements = jPolicy[JsonDocumentFields.STATEMENT] as JsonData;
            if (jStatements != null && jStatements.IsArray)
            {
                foreach (JsonData jStatement in jStatements)
                {
                    Statement statement = convertStatement(jStatement);
                    if (statement != null)
                    {
                        policy.Statements.Add(statement);
                    }
                }
            }

            return policy;
        }

        private static Statement convertStatement(JsonData jStatement)
        {
            if (jStatement[JsonDocumentFields.STATEMENT_EFFECT] == null || !jStatement[JsonDocumentFields.STATEMENT_EFFECT].IsString)
                return null;


            string jEffect = (string)jStatement[JsonDocumentFields.STATEMENT_EFFECT];
            Statement.StatementEffect effect;
            if (JsonDocumentFields.EFFECT_VALUE_ALLOW.Equals(jEffect))
                effect = Statement.StatementEffect.Allow;
            else
                effect = Statement.StatementEffect.Deny;

            Statement statement = new Statement(effect);

            if (jStatement[JsonDocumentFields.STATEMENT_ID] != null && jStatement[JsonDocumentFields.STATEMENT_ID].IsString)
                statement.Id = (string)jStatement[JsonDocumentFields.STATEMENT_ID];

            convertActions(statement, jStatement);
            convertResources(statement, jStatement);
            convertCondition(statement, jStatement);
            convertPrincipals(statement, jStatement);

            return statement;
        }

        private static void convertPrincipals(Statement statement, JsonData jStatement)
        {
            JsonData jPrincipals = jStatement[JsonDocumentFields.PRINCIPAL];
            if (jPrincipals == null)
            {
                return;
            }

            if (jPrincipals.IsObject)
            {
                convertPrincipalRecord(statement, jPrincipals);
            }
            else if (jPrincipals.IsArray)
            {
                foreach (JsonData jPrincipal in jPrincipals)
                {
                    convertPrincipalRecord(statement, jPrincipal);
                }
            }
            else if (jPrincipals.IsString && jPrincipals.Equals("*"))
            {
                statement.Principals.Add(Principal.Anonymous);
            }
        }

        private static void convertPrincipalRecord(Statement statement, JsonData jPrincipal)
        {
            foreach (KeyValuePair<string, JsonData> kvp in jPrincipal)
            {
                if (kvp.Value == null)
                {
                    continue;
                }

                if (kvp.Value.IsArray)
                {
                    foreach (JsonData tok in kvp.Value)
                    {
                        if (tok.IsString)
                        {
                            // Don't strip '-' and assume the policy being deserialized is already valid.
                            Principal principal = new Principal(kvp.Key, (string)tok, false);
                            statement.Principals.Add(principal);
                        }
                    }
                }
                else if(kvp.Value.IsString)
                {
                    // Don't strip '-' and assume the policy being deserialized is already valid.
                    Principal principal = new Principal(kvp.Key, (string)kvp.Value, false);
                    statement.Principals.Add(principal);
                }
            }
        }

        private static void convertActions(Statement statement, JsonData jStatement)
        {
            JsonData jActions = jStatement[JsonDocumentFields.ACTION];
            if (jActions == null)
            {
                return;
            }

            if (jActions.IsString)
            {
                statement.Actions.Add(new ActionIdentifier((string)jActions));
            }
            else if (jActions.IsArray)
            {
                foreach (JsonData jActionValue in jActions)
                {
                    if (jActionValue.IsString)
                    {
                        statement.Actions.Add(new ActionIdentifier((string)jActionValue));
                    }
                }
            }
        }

        private static void convertResources(Statement statement, JsonData jStatement)
        {
            JsonData jResources = jStatement[JsonDocumentFields.RESOURCE];
            if (jResources == null)
            {
                return;
            }

            if (jResources.IsString)
            {
                statement.Resources.Add(new Resource((string)jResources));
            }
            else if (jResources.IsArray)
            {
                foreach (JsonData jResourceValue in jResources)
                {
                    if (jResourceValue.IsString)
                    {
                        statement.Resources.Add(new Resource((string)jResourceValue));
                    }
                }
            }
        }

        private static void convertCondition(Statement statement, JsonData jStatement)
        {
            JsonData jConditions = jStatement[JsonDocumentFields.CONDITION];
            if (jConditions == null)
            {
                return;
            }

            if (jConditions.IsObject)
            {
                convertConditionRecord(statement, jConditions);
            }
            else if (jConditions.IsArray)
            {
                foreach (JsonData jCondition in jConditions)
                {
                    convertConditionRecord(statement, jCondition);
                }
            }
        }

        private static void convertConditionRecord(Statement statement, JsonData jCondition)
        {
            foreach (KeyValuePair<string, JsonData> kvp1 in jCondition)
            {
                string type = kvp1.Key;
                JsonData comparisons = kvp1.Value;
                foreach (KeyValuePair<string, JsonData> kvp2 in comparisons)
                {
                    string key = kvp2.Key;
                    List<string> values = new List<string>();
                    if (kvp2.Value != null)
                    {
                        if (kvp2.Value.IsString)
                        {
                            values.Add((string)kvp2.Value);
                        }
                        else if (kvp2.Value.IsArray)
                        {
                            foreach (JsonData jValue in kvp2.Value)
                            {
                                if (jValue.IsString)
                                {
                                    values.Add((string)jValue);
                                }
                            }
                        }
                    }

                    Condition condition = new Condition(type, key, values.ToArray());
                    statement.Conditions.Add(condition);
                }
            }
        }
    }
}

