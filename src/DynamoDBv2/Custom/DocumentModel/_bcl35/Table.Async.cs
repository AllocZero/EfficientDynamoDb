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

using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Util;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Amazon.DynamoDBv2.DocumentModel
{
    public partial class Table
    {
        #region PutItemAsync

        /// <summary>
        /// Initiates the asynchronous execution of the PutItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.PutItem"/>
        /// </summary>
        /// <param name="doc">Document to save.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndPutItem
        ///         operation.</returns>
        public IAsyncResult BeginPutItem(Document doc, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => PutItemHelper(doc, null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the PutItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.PutItem"/>
        /// </summary>
        /// <param name="doc">Document to save.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndPutItem
        ///         operation.</returns>
        public IAsyncResult BeginPutItem(Document doc, PutItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => PutItemHelper(doc, config), callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the PutItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.PutItem"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginPutItem.</param>
        /// <returns>Null or updated attributes, depending on config.</returns>
        public Document EndPutItem(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<Document>(asyncResult);
        }

        #endregion

        #region GetItemAsync

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(Primitive hashKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(hashKey, null), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(Primitive hashKey, Primitive rangeKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(hashKey, rangeKey), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(Primitive hashKey, GetItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(hashKey, null), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(hashKey, rangeKey), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="key">Key of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(IDictionary<string, DynamoDBEntry> key, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(key), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="key">Ley of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndGetItem
        ///         operation.</returns>
        public IAsyncResult BeginGetItem(IDictionary<string, DynamoDBEntry> key, GetItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => GetItemHelper(MakeKey(key), config), callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.GetItem"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginGetItem.</param>
        /// <returns>Document from DynamoDB.</returns>
        public Document EndGetItem(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<Document>(asyncResult);
        }

        #endregion

        #region UpdateItemAsync

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Document to update.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(doc), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, Primitive hashKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(hashKey, null), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, Primitive hashKey, Primitive rangeKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(hashKey, rangeKey), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="key">Key of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, IDictionary<string, DynamoDBEntry> key, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(key), null), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Document to update.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, UpdateItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(doc), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, Primitive hashKey, UpdateItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(hashKey, null), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, Primitive hashKey, Primitive rangeKey, UpdateItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(hashKey, rangeKey), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="doc">Attributes to update.</param>
        /// <param name="key">Key of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndUpdateItem
        ///         operation.</returns>
        public IAsyncResult BeginUpdateItem(Document doc, IDictionary<string, DynamoDBEntry> key, UpdateItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => UpdateHelper(doc, MakeKey(key), config), callback, state);
        }

        /// <summary>
        /// Finishes the asynchronous execution of the GetItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginUpdateItem.</param>
        /// <returns>Null or updated attributes, depending on config.</returns>
        public Document EndUpdateItem(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<Document>(asyncResult);
        }

        #endregion

        #region DeleteItemAsync

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="document">Document to delete.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Document document, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => { DeleteHelper(MakeKey(document), null); return null; }, callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Primitive hashKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => { DeleteHelper(MakeKey(hashKey, null), null); return null; }, callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Primitive hashKey, Primitive rangeKey, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => { DeleteHelper(MakeKey(hashKey, rangeKey), null); return null; }, callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="key">Key of the document.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(IDictionary<string, DynamoDBEntry> key, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => { DeleteHelper(MakeKey(key), null); return null; }, callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="document">Document to delete.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Document document, DeleteItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => DeleteHelper(MakeKey(document), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Primitive hashKey, DeleteItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => DeleteHelper(MakeKey(hashKey, null), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="hashKey">Hash key element of the document.</param>
        /// <param name="rangeKey">Range key element of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(Primitive hashKey, Primitive rangeKey, DeleteItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => DeleteHelper(MakeKey(hashKey, rangeKey), config), callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="key">Key of the document.</param>
        /// <param name="config">Configuration to use.</param>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the operation completes.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback
        ///          procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndDeleteItem
        ///         operation.</returns>
        public IAsyncResult BeginDeleteItem(IDictionary<string, DynamoDBEntry> key, DeleteItemOperationConfig config, AsyncCallback callback, object state)
        {
            return DynamoDBAsyncExecutor.BeginOperation(() => DeleteHelper(MakeKey(key), config), callback, state);
        }
        
        /// <summary>
        /// Finishes the asynchronous execution of the DeleteItem operation.
        /// <seealso cref="Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem"/>
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginDeleteItem.</param>
        /// <returns>Null or old attributes, depending on config.</returns>
        public Document EndDeleteItem(IAsyncResult asyncResult)
        {
            return DynamoDBAsyncExecutor.EndOperation<Document>(asyncResult);
        }

        #endregion

    }
}
