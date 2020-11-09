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

namespace Amazon.Auth.AccessControlPolicy.ActionIdentifiers
{
    /// <summary>
    /// The available AWS access control policy actions for Amazon Elastic Transcoder.
    /// </summary>
    /// <see cref="Amazon.Auth.AccessControlPolicy.Statement.Actions"/>
    [Obsolete(
            "The Amazon.Auth.AccessContolPolicy classes are out of date and are no longer being maintained.\n" +
            "Please construct policy documents using the information found here: https://docs.aws.amazon.com/IAM/latest/UserGuide/access_policies.html#access_policies-json")]
    public static class ElasticTranscoderActionIdentifiers
    {
        public static readonly ActionIdentifier AllElasticTranscoderActions = new ActionIdentifier("elastictranscoder:*");

        public static readonly ActionIdentifier CancelJob = new ActionIdentifier("elastictranscoder:CancelJob");
        public static readonly ActionIdentifier CreateJob = new ActionIdentifier("elastictranscoder:CreateJob");
        public static readonly ActionIdentifier CreatePipeline = new ActionIdentifier("elastictranscoder:CreatePipeline");
        public static readonly ActionIdentifier CreatePreset = new ActionIdentifier("elastictranscoder:CreatePreset");
        public static readonly ActionIdentifier DeletePipeline = new ActionIdentifier("elastictranscoder:DeletePipeline");
        public static readonly ActionIdentifier DeletePreset = new ActionIdentifier("elastictranscoder:DeletePreset");
        public static readonly ActionIdentifier ListJobsByPipeline = new ActionIdentifier("elastictranscoder:ListJobsByPipeline");
        public static readonly ActionIdentifier ListJobsByStatus = new ActionIdentifier("elastictranscoder:ListJobsByStatus");
        public static readonly ActionIdentifier ListPipelines = new ActionIdentifier("elastictranscoder:ListPipelines");
        public static readonly ActionIdentifier ListPresets = new ActionIdentifier("elastictranscoder:ListPresets");
        public static readonly ActionIdentifier ReadJob = new ActionIdentifier("elastictranscoder:ReadJob");
        public static readonly ActionIdentifier ReadPipeline = new ActionIdentifier("elastictranscoder:ReadPipeline");
        public static readonly ActionIdentifier ReadPreset = new ActionIdentifier("elastictranscoder:ReadPreset");
        public static readonly ActionIdentifier TestRole = new ActionIdentifier("elastictranscoder:TestRole");
        public static readonly ActionIdentifier UpdatePipeline = new ActionIdentifier("elastictranscoder:UpdatePipeline");
        public static readonly ActionIdentifier UpdatePipelineNotifications = new ActionIdentifier("elastictranscoder:UpdatePipelineNotifications");
        public static readonly ActionIdentifier UpdatePipelineStatus = new ActionIdentifier("elastictranscoder:UpdatePipelineStatus");
    }
}

