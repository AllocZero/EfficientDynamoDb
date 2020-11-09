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

using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// A strongly-typed object for retrieving search results (Query or Scan)
    /// from DynamoDB.
    /// </summary>
    public partial class AsyncSearch<T>
    {
        #region Constructor

        internal AsyncSearch(DynamoDBContext source, DynamoDBContext.ContextSearch contextSearch)
        {
            SourceContext = source;
            DocumentSearch = contextSearch.Search;
            Config = contextSearch.FlatConfig;
        }

        #endregion

        #region Private members

        private Search DocumentSearch { get; set; }
        private DynamoDBContext SourceContext { get; set; }
        private DynamoDBFlatConfig Config { get; set; }

        #endregion

        #region Public properties

        /// <summary>
        /// Flag that, if true, indicates that the search is done
        /// </summary>
        public bool IsDone
        {
            get
            {
                return DocumentSearch.IsDone;
            }
        }

        #endregion
    }
}
