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

namespace Amazon.Auth.AccessControlPolicy.Internal
{
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    internal static class JsonDocumentFields
    {
        internal const string VERSION = "Version";
        internal const string POLICY_ID = "Id";
        internal const string STATEMENT = "Statement";
        internal const string STATEMENT_EFFECT = "Effect";
        internal const string EFFECT_VALUE_ALLOW = "Allow";
        internal const string STATEMENT_ID = "Sid";
        internal const string PRINCIPAL = "Principal";
        internal const string ACTION = "Action";
        internal const string RESOURCE = "Resource";
        internal const string CONDITION = "Condition";

    }
}

