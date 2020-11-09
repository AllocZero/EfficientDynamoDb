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
    /// Container for the parameters to the DescribeLimits operation.
    /// Returns the current provisioned-capacity quotas for your AWS account in a Region,
    /// both for the Region as a whole and for any one DynamoDB table that you create there.
    /// 
    ///  
    /// <para>
    /// When you establish an AWS account, the account has initial quotas on the maximum read
    /// capacity units and write capacity units that you can provision across all of your
    /// DynamoDB tables in a given Region. Also, there are per-table quotas that apply when
    /// you create a table there. For more information, see <a href="https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/Limits.html">Service,
    /// Account, and Table Quotas</a> page in the <i>Amazon DynamoDB Developer Guide</i>.
    /// </para>
    ///  
    /// <para>
    /// Although you can increase these quotas by filing a case at <a href="https://console.aws.amazon.com/support/home#/">AWS
    /// Support Center</a>, obtaining the increase is not instantaneous. The <code>DescribeLimits</code>
    /// action lets you write code to compare the capacity you are currently using to those
    /// quotas imposed by your account so that you have enough time to apply for an increase
    /// before you hit a quota.
    /// </para>
    ///  
    /// <para>
    /// For example, you could use one of the AWS SDKs to do the following:
    /// </para>
    ///  <ol> <li> 
    /// <para>
    /// Call <code>DescribeLimits</code> for a particular Region to obtain your current account
    /// quotas on provisioned capacity there.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Create a variable to hold the aggregate read capacity units provisioned for all your
    /// tables in that Region, and one to hold the aggregate write capacity units. Zero them
    /// both.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Call <code>ListTables</code> to obtain a list of all your DynamoDB tables.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// For each table name listed by <code>ListTables</code>, do the following:
    /// </para>
    ///  <ul> <li> 
    /// <para>
    /// Call <code>DescribeTable</code> with the table name.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// Use the data returned by <code>DescribeTable</code> to add the read capacity units
    /// and write capacity units provisioned for the table itself to your variables.
    /// </para>
    ///  </li> <li> 
    /// <para>
    /// If the table has one or more global secondary indexes (GSIs), loop over these GSIs
    /// and add their provisioned capacity values to your variables as well.
    /// </para>
    ///  </li> </ul> </li> <li> 
    /// <para>
    /// Report the account quotas for that Region returned by <code>DescribeLimits</code>,
    /// along with the total current provisioned capacity levels you have calculated.
    /// </para>
    ///  </li> </ol> 
    /// <para>
    /// This will let you see whether you are getting close to your account-level quotas.
    /// </para>
    ///  
    /// <para>
    /// The per-table quotas apply only when you are creating a new table. They restrict the
    /// sum of the provisioned capacity of the new table itself and all its global secondary
    /// indexes.
    /// </para>
    ///  
    /// <para>
    /// For existing tables and their GSIs, DynamoDB doesn't let you increase provisioned
    /// capacity extremely rapidly, but the only quota that applies is that the aggregate
    /// provisioned capacity over all your tables and GSIs cannot exceed either of the per-account
    /// quotas.
    /// </para>
    ///  <note> 
    /// <para>
    ///  <code>DescribeLimits</code> should only be called periodically. You can expect throttling
    /// errors if you call it more than once in a minute.
    /// </para>
    ///  </note> 
    /// <para>
    /// The <code>DescribeLimits</code> Request element has no content.
    /// </para>
    /// </summary>
    public partial class DescribeLimitsRequest : AmazonDynamoDBRequest
    {

    }
}