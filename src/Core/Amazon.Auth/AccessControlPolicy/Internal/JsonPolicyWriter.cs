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
using System.Globalization;

namespace Amazon.Auth.AccessControlPolicy.Internal
{
    /// <summary>
    /// Serializes an AWS policy object to a JSON string, suitable for sending to an
    /// AWS service.
    /// </summary>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    internal static class JsonPolicyWriter
    {
        /**
         * Converts the specified AWS policy object to a JSON string, suitable for
         * passing to an AWS service.
         *
         * @param policy
         *            The AWS policy object to convert to a JSON string.
         *
         * @return The JSON string representation of the specified policy object.
         *
         * @throws IllegalArgumentException
         *             If the specified policy is null or invalid and cannot be
         *             serialized to a JSON string.
         */
        public static string WritePolicyToString(bool prettyPrint, Policy policy)
        {
            if (policy == null)
            {
                throw new ArgumentNullException("policy");
            }

            StringWriter writer = new StringWriter(CultureInfo.InvariantCulture);
            try
            {
                JsonWriter generator = new JsonWriter(writer);

                generator.IndentValue = 4;
                generator.PrettyPrint = prettyPrint;

                writePolicy(policy, generator);
                return writer.ToString().Trim();
            }
            catch (Exception e)
            {
                string message = "Unable to serialize policy to JSON string: " + e.Message;
                throw new ArgumentException(message, e);
            }
        }

        private static void writePolicy(Policy policy, JsonWriter generator)
        {
            generator.WriteObjectStart();

            writePropertyValue(generator, JsonDocumentFields.VERSION, policy.Version);

            if (policy.Id != null)
            {
                writePropertyValue(generator, JsonDocumentFields.POLICY_ID, policy.Id);
            }

            generator.WritePropertyName(JsonDocumentFields.STATEMENT);
            generator.WriteArrayStart();
            foreach (Statement statement in policy.Statements)
            {
                generator.WriteObjectStart();
                if (statement.Id != null)
                {
                    writePropertyValue(generator, JsonDocumentFields.STATEMENT_ID, statement.Id);
                }
                writePropertyValue(generator, JsonDocumentFields.STATEMENT_EFFECT, statement.Effect.ToString());

                writePrincipals(statement, generator);
                writeActions(statement, generator);
                writeResources(statement, generator);
                writeConditions(statement, generator);

                generator.WriteObjectEnd();
            }
            generator.WriteArrayEnd();

            generator.WriteObjectEnd();
        }

        /// <summary>
        /// Uses the specified generator to write the JSON data for the principals in
        /// the specified policy statement.
        /// </summary>
        private static void writePrincipals(Statement statement, JsonWriter generator)
        {
            IList<Principal> principals = statement.Principals;
            if (principals == null || principals.Count == 0) return;

            generator.WritePropertyName(JsonDocumentFields.PRINCIPAL);

            if (principals.Count == 1 && 
                principals[0] != null &&
                principals[0].Provider.Equals(Principal.ANONYMOUS_PROVIDER, StringComparison.Ordinal))
            {
                generator.Write("*");
                return;
            }

            generator.WriteObjectStart();
            Dictionary<string, List<string>> principalIdsByScheme = new Dictionary<string, List<string>>();
            foreach (Principal p in principals)
            {
                List<string> principalIds;
                if (!principalIdsByScheme.TryGetValue(p.Provider, out principalIds))
                {
                    principalIds = new List<string>();
                    principalIdsByScheme[p.Provider] = principalIds;
                }

                principalIds.Add(p.Id);
            }
            foreach (string scheme in principalIdsByScheme.Keys)
            {
                generator.WritePropertyName(scheme);

                if (principalIdsByScheme[scheme].Count > 1)
                {
                    generator.WriteArrayStart();
                }
                foreach (string principalId in principalIdsByScheme[scheme])
                {
                    generator.Write(principalId);
                }
                if (principalIdsByScheme[scheme].Count > 1)
                {
                    generator.WriteArrayEnd();
                }
            }
            generator.WriteObjectEnd();
        }

        private static void writeActions(Statement statement, JsonWriter generator)
        {
            IList<ActionIdentifier> actions = statement.Actions;
            if (actions == null || actions.Count == 0)
            {
                return;
            }

            generator.WritePropertyName(JsonDocumentFields.ACTION);
            if (actions.Count > 1)
            {
                generator.WriteArrayStart();
            }

            foreach (ActionIdentifier action in actions)
            {
                generator.Write(action.ActionName);
            }

            if (actions.Count > 1)
            {
                generator.WriteArrayEnd();
            }
        }

        private static void writeResources(Statement statement, JsonWriter generator)
        {
            IList<Resource> resources = statement.Resources;
            if (resources == null || resources.Count == 0)
            {
                return;
            }

            generator.WritePropertyName(JsonDocumentFields.RESOURCE);
            if (resources.Count > 1)
            {
                generator.WriteArrayStart();
            }

            foreach (Resource resource in resources)
            {
                generator.Write(resource.Id);
            }

            if (resources.Count > 1)
            {
                generator.WriteArrayEnd();
            }
        }

        private static void writeConditions(Statement statement, JsonWriter generator)
        {
            IList<Condition> conditions = statement.Conditions;
            if (conditions == null || conditions.Count == 0)
            {
                return;
            }

            /*
             * The condition values must be grouped by all the unique condition types and keys because
             * the values are written out as an array per type and key.
             */
            Dictionary<string, Dictionary<string, List<string>>> conditionsByTypeAndKeys = sortConditionsByTypeAndKey(conditions);

            generator.WritePropertyName(JsonDocumentFields.CONDITION);
            generator.WriteObjectStart();
            foreach (KeyValuePair<string, Dictionary<string, List<string>>> typeEntry in conditionsByTypeAndKeys)
            {
                generator.WritePropertyName(typeEntry.Key);
                generator.WriteObjectStart();
                foreach (KeyValuePair<string, List<string>> keyEntry in typeEntry.Value)
                {
                    IList<string> conditionValues = keyEntry.Value;
                    if (conditionValues.Count == 0)
                        continue;

                    generator.WritePropertyName(keyEntry.Key);

                    if (conditionValues.Count > 1)
                    {
                        generator.WriteArrayStart();
                    }

                    if (conditionValues != null && conditionValues.Count != 0)
                    {
                        foreach (string conditionValue in conditionValues)
                        {
                            generator.Write(conditionValue);
                        }
                    }

                    if (conditionValues.Count > 1)
                    {
                        generator.WriteArrayEnd();
                    }
                }
                generator.WriteObjectEnd();
            }
            generator.WriteObjectEnd();
        }

        /// <summary>
        /// This sorts the conditions by condition type and key with the list of values for that combination.
        /// </summary>
        /// <param name="conditions">The list of conditions to be sorted.</param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, List<string>>> sortConditionsByTypeAndKey(IList<Condition> conditions)
        {
            Dictionary<string, Dictionary<string, List<string>>> conditionsByTypeAndKeys = new Dictionary<string, Dictionary<string, List<string>>>();
            foreach (Condition condition in conditions)
            {
                string conditionType = condition.Type;
                string conditionKey = condition.ConditionKey;

                Dictionary<string, List<string>> keys;
                if (!conditionsByTypeAndKeys.TryGetValue(conditionType, out keys))
                {
                    keys = new Dictionary<string, List<string>>();
                    conditionsByTypeAndKeys[conditionType] = keys;
                }

                List<string> values;
                if (!keys.TryGetValue(conditionKey, out values))
                {
                    values = new List<string>();
                    keys[conditionKey] = values;
                }

                if (condition.Values != null)
                {
                    foreach (string value in condition.Values)
                    {
                        values.Add(value);
                    }
                }
            }

            return conditionsByTypeAndKeys;
        }

        private static void writePropertyValue(JsonWriter generator, string propertyName, string value)
        {
            generator.WritePropertyName(propertyName);
            generator.Write(value);
        }
    }
}

