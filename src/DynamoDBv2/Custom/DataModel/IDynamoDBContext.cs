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

using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// Context interface for using the DataModel mode of DynamoDB.
    /// Used to interact with the service, save/load objects, etc.
    /// </summary>
    public partial interface IDynamoDBContext : IDisposable
    {
        #region Save/serialize

        /// <summary>
        /// Serializes an object to a Document.
        /// </summary>
        /// <typeparam name="T">Type to serialize as.</typeparam>
        /// <param name="value">Object to serialize.</param>
        /// <returns>Document with attributes populated from object.</returns>
        Document ToDocument<T>(T value);

        /// <summary>
        /// Serializes an object to a Document.
        /// </summary>
        /// <typeparam name="T">Type to serialize as.</typeparam>
        /// <param name="value">Object to serialize.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>Document with attributes populated from object.</returns>
        Document ToDocument<T>(T value, DynamoDBOperationConfig operationConfig);

        #endregion

        #region Load/deserialize

        /// <summary>
        /// Deserializes a document to an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="document">Document with properties to use.</param>
        /// <returns>
        /// Object of type T, populated with properties from the document.
        /// </returns>
        T FromDocument<T>(Document document);

        /// <summary>
        /// Deserializes a document to an instance of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="document">Document with properties to use.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>
        /// Object of type T, populated with properties from the document.
        /// </returns>
        T FromDocument<T>(Document document, DynamoDBOperationConfig operationConfig);


        /// <summary>
        /// Deserializes a collections of documents to a collection of instances of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="documents">Documents to deserialize.</param>
        /// <returns>
        /// Collection of items of type T, each populated with properties from a corresponding document.
        /// </returns>
        IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents);

        /// <summary>
        /// Deserializes a collections of documents to a collection of instances of type T.
        /// </summary>
        /// <typeparam name="T">Type to populate.</typeparam>
        /// <param name="documents">Documents to deserialize.</param>
        /// <param name="operationConfig">Config object which can be used to override the table used.</param>
        /// <returns>
        /// Collection of items of type T, each populated with properties from a corresponding document.
        /// </returns>
        IEnumerable<T> FromDocuments<T>(IEnumerable<Document> documents, DynamoDBOperationConfig operationConfig);

        #endregion

        #region BatchGet
#if BCL35
        /// <summary>
        /// Creates a strongly-typed BatchGet object, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to get</typeparam>
        /// <returns>Empty strongly-typed BatchGet object</returns>
        BatchGet<T> CreateBatchGet<T>();

        /// <summary>
        /// Creates a strongly-typed BatchGet object, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to get</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchGet object</returns>
        BatchGet<T> CreateBatchGet<T>(DynamoDBOperationConfig operationConfig);
#else
        /// <summary>
        /// Creates a strongly-typed BatchGet object, allowing
        /// a batch-get operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to get</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchGet object</returns>
        BatchGet<T> CreateBatchGet<T>(DynamoDBOperationConfig operationConfig = null);
#endif

        /// <summary>
        /// Creates a MultiTableBatchGet object, composed of multiple
        /// individual BatchGet objects.
        /// </summary>
        /// <param name="batches">Individual BatchGet objects</param>
        /// <returns>Composite MultiTableBatchGet object</returns>
        MultiTableBatchGet CreateMultiTableBatchGet(params BatchGet[] batches);

        #endregion

        #region Batch Write

#if BCL35
        /// <summary>
        /// Creates a strongly-typed BatchWrite object, allowing
        /// a batch-write operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to write</typeparam>
        /// <returns>Empty strongly-typed BatchWrite object</returns>
        BatchWrite<T> CreateBatchWrite<T>();

        /// <summary>
        /// Creates a strongly-typed BatchWrite object, allowing
        /// a batch-write operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to write</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchWrite object</returns>
        BatchWrite<T> CreateBatchWrite<T>(DynamoDBOperationConfig operationConfig);
#else
        /// <summary>
        /// Creates a strongly-typed BatchWrite object, allowing
        /// a batch-write operation against DynamoDB.
        /// </summary>
        /// <typeparam name="T">Type of objects to write</typeparam>
        /// <param name="operationConfig">Config object which can be used to override that table used.</param>
        /// <returns>Empty strongly-typed BatchWrite object</returns>
        BatchWrite<T> CreateBatchWrite<T>(DynamoDBOperationConfig operationConfig = null);
#endif

        /// <summary>
        /// Creates a MultiTableBatchWrite object, composed of multiple
        /// individual BatchWrite objects.
        /// </summary>
        /// <param name="batches">Individual BatchWrite objects</param>
        /// <returns>Composite MultiTableBatchWrite object</returns>
        MultiTableBatchWrite CreateMultiTableBatchWrite(params BatchWrite[] batches);

        #endregion
    }
}
