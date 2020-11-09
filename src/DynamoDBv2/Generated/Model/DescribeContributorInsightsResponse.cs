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

/*
 * Do not modify this file. This file is generated from the dynamodb-2012-08-10.normal.json service model.
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Net;

using Amazon.Runtime;
using Amazon.Runtime.Internal;

namespace Amazon.DynamoDBv2.Model
{
    /// <summary>
    /// This is the response object from the DescribeContributorInsights operation.
    /// </summary>
    public partial class DescribeContributorInsightsResponse : AmazonWebServiceResponse
    {
        private List<string> _contributorInsightsRuleList = new List<string>();
        private ContributorInsightsStatus _contributorInsightsStatus;
        private FailureException _failureException;
        private string _indexName;
        private DateTime? _lastUpdateDateTime;
        private string _tableName;

        /// <summary>
        /// Gets and sets the property ContributorInsightsRuleList. 
        /// <para>
        /// List of names of the associated Alpine rules.
        /// </para>
        /// </summary>
        public List<string> ContributorInsightsRuleList
        {
            get { return this._contributorInsightsRuleList; }
            set { this._contributorInsightsRuleList = value; }
        }

        // Check to see if ContributorInsightsRuleList property is set
        internal bool IsSetContributorInsightsRuleList()
        {
            return this._contributorInsightsRuleList != null && this._contributorInsightsRuleList.Count > 0; 
        }

        /// <summary>
        /// Gets and sets the property ContributorInsightsStatus. 
        /// <para>
        /// Current Status contributor insights.
        /// </para>
        /// </summary>
        public ContributorInsightsStatus ContributorInsightsStatus
        {
            get { return this._contributorInsightsStatus; }
            set { this._contributorInsightsStatus = value; }
        }

        // Check to see if ContributorInsightsStatus property is set
        internal bool IsSetContributorInsightsStatus()
        {
            return this._contributorInsightsStatus != null;
        }

        /// <summary>
        /// Gets and sets the property FailureException. 
        /// <para>
        /// Returns information about the last failure that encountered.
        /// </para>
        ///  
        /// <para>
        /// The most common exceptions for a FAILED status are:
        /// </para>
        ///  <ul> <li> 
        /// <para>
        /// LimitExceededException - Per-account Amazon CloudWatch Contributor Insights rule limit
        /// reached. Please disable Contributor Insights for other tables/indexes OR disable Contributor
        /// Insights rules before retrying.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// AccessDeniedException - Amazon CloudWatch Contributor Insights rules cannot be modified
        /// due to insufficient permissions.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// AccessDeniedException - Failed to create service-linked role for Contributor Insights
        /// due to insufficient permissions.
        /// </para>
        ///  </li> <li> 
        /// <para>
        /// InternalServerError - Failed to create Amazon CloudWatch Contributor Insights rules.
        /// Please retry request.
        /// </para>
        ///  </li> </ul>
        /// </summary>
        public FailureException FailureException
        {
            get { return this._failureException; }
            set { this._failureException = value; }
        }

        // Check to see if FailureException property is set
        internal bool IsSetFailureException()
        {
            return this._failureException != null;
        }

        /// <summary>
        /// Gets and sets the property IndexName. 
        /// <para>
        /// The name of the global secondary index being described.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string IndexName
        {
            get { return this._indexName; }
            set { this._indexName = value; }
        }

        // Check to see if IndexName property is set
        internal bool IsSetIndexName()
        {
            return this._indexName != null;
        }

        /// <summary>
        /// Gets and sets the property LastUpdateDateTime. 
        /// <para>
        /// Timestamp of the last time the status was changed.
        /// </para>
        /// </summary>
        public DateTime LastUpdateDateTime
        {
            get { return this._lastUpdateDateTime.GetValueOrDefault(); }
            set { this._lastUpdateDateTime = value; }
        }

        // Check to see if LastUpdateDateTime property is set
        internal bool IsSetLastUpdateDateTime()
        {
            return this._lastUpdateDateTime.HasValue; 
        }

        /// <summary>
        /// Gets and sets the property TableName. 
        /// <para>
        /// The name of the table being described.
        /// </para>
        /// </summary>
        [AWSProperty(Min=3, Max=255)]
        public string TableName
        {
            get { return this._tableName; }
            set { this._tableName = value; }
        }

        // Check to see if TableName property is set
        internal bool IsSetTableName()
        {
            return this._tableName != null;
        }

    }
}