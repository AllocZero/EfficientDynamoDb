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

using System.Diagnostics.CodeAnalysis;

// Suppressions for identifiers in base classes
[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "type", Target = "Amazon.DynamoDBv2.Select", MessageId = "Select")]
// Identifiers should not contain type names
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsInt()", MessageId = "int")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsLong()", MessageId = "long")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsShort()", MessageId = "short")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsUInt()", MessageId = "uint")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsULong()", MessageId = "ulong")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry.#AsUShort()", MessageId = "ushort")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.Model.AttributeValue.#BOOL", MessageId = "bool")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.DynamoDBv2.Model.AttributeValue.#IsBOOLSet", MessageId = "bool")]
// Array properties/fields
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBLocalSecondaryIndexRangeKeyAttribute.#IndexNames")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.QueryCondition.#Values")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.ScanCondition.#Values")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexHashKeyAttribute.#IndexNames")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexRangeKeyAttribute.#IndexNames")]
// Nested generic types
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchGetItemResponse.#Responses")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemResponse.#ItemCollectionMetrics")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemResponse.#UnprocessedItems")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.QueryResponse.#Items")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.ScanResponse.#Items")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchGetItemResult.#Responses")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemRequest.#RequestItems")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemResult.#ItemCollectionMetrics")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemResult.#UnprocessedItems")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.KeysAndAttributes.#Keys")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.QueryResult.#Items")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.ScanResult.#Items")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.AsyncSearch`1.#GetNextSetAsync(System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.AsyncSearch`1.#GetRemainingAsync(System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.IAmazonDynamoDB.#BatchWriteItemAsync(System.Collections.Generic.Dictionary`2<System.String,System.Collections.Generic.List`1<Amazon.DynamoDBv2.Model.WriteRequest>>,System.Threading.CancellationToken)")]
// Suppressions for DynamoDBContext generic methods
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginDelete`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginDelete`1(System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginDelete`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginDelete`1(System.Object,System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginLoad`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginLoad`1(System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginLoad`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#BeginLoad`1(System.Object,System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#Delete`1(System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#Delete`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#Delete`1(System.Object,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#Delete`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#GetTargetTable`1()")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#GetTargetTable`1(Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#DeleteAsync`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#DeleteAsync`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#DeleteAsync`1(System.Object,System.Object,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#DeleteAsync`1(System.Object,System.Threading.CancellationToken)")]
// Suppressions for IDynamoDBContext generic methods
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#Delete`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginLoad`1(System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginDelete`1(System.Object,System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginDelete`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#Delete`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginDelete`1(System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginLoad`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#GetTargetTable`1()")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#GetTargetTable`1(Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginLoad`1(System.Object,System.Object,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#Delete`1(System.Object,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginLoad`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#Delete`1(System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#BeginDelete`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#DeleteAsync`1(System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#DeleteAsync`1(System.Object,System.Object,Amazon.DynamoDBv2.DataModel.DynamoDBOperationConfig,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#DeleteAsync`1(System.Object,System.Object,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.IDynamoDBContext.#DeleteAsync`1(System.Object,System.Threading.CancellationToken)")]
// Sealing attributes, attributes are extended elsewhere
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBPropertyAttribute")]
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBRangeKeyAttribute")]
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexHashKeyAttribute")]
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexRangeKeyAttribute")]
[module: SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBHashKeyAttribute")]
// Identifier suffix
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Amazon.DynamoDBv2.DocumentModel.Document")]
// General exception types
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DynamoDBAsyncExecutor.#Execute(Amazon.DynamoDBv2.AsyncCall,Amazon.DynamoDBv2.DynamoDBAsyncResult)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#TryLoadTable(Amazon.DynamoDBv2.IAmazonDynamoDB,System.String,Amazon.DynamoDBv2.DocumentModel.Table&)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#TryLoadTable(Amazon.DynamoDBv2.IAmazonDynamoDB,System.String,Amazon.DynamoDBv2.DocumentModel.Table+DynamoDBConsumer,Amazon.DynamoDBv2.DynamoDBEntryConversion,Amazon.DynamoDBv2.DocumentModel.Table&)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#TryLoadTable(Amazon.DynamoDBv2.IAmazonDynamoDB,System.String,Amazon.DynamoDBv2.DynamoDBEntryConversion,Amazon.DynamoDBv2.DocumentModel.Table&)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBContext.#TryToScalar(System.Object,System.Type,Amazon.DynamoDBv2.DataModel.DynamoDBFlatConfig,Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry&)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.JsonUtils.#TryDecodeBase64(System.String,System.Byte[]&)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.DynamoDBv2.EnumConverterV1.#ConvertEnum(System.String,System.Type)")]
// Overflow operations
[module: SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", Scope = "member", Target = "Amazon.DynamoDBv2.AmazonDynamoDBClient.#pauseExponentially(System.Int32)", MessageId = "retries-1")]
// Atrribute arguments, breaking change
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBLocalSecondaryIndexRangeKeyAttribute")]
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBPropertyAttribute")]
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBTableAttribute")]
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexHashKeyAttribute")]
[module: SuppressMessage("Microsoft.Design", "CA1019:DefineAccessorsForAttributeArguments", Scope = "type", Target = "Amazon.DynamoDBv2.DataModel.DynamoDBGlobalSecondaryIndexRangeKeyAttribute")]
// Uri properties should not be strings
[module: SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.S3Link.#GetPreSignedURL(System.DateTime)")]
// Use properties, breaking changes
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Document.#GetAttributeNames()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#GetNextSet()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#GetRemaining()")]
// Flag enums should have plural names, breaking changes
[module: SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Scope = "type", Target = "Amazon.DynamoDBv2.DocumentModel.ReturnValues")]
[module: SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Scope = "type", Target = "Amazon.DynamoDBv2.DocumentModel.SelectValues")]
[module: SuppressMessage("Microsoft.Naming", "CA1717:OnlyFlagsEnumsShouldHavePluralNames", Scope = "type", Target = "Amazon.DynamoDBv2.DocumentModel.ConditionalOperatorValues")]
// Unused parameters
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#UserAgentRequestEventHandler(System.Object,Amazon.Runtime.RequestEventArgs,System.Boolean)", MessageId = "sender")]
// Mark members as static, breaking changes
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.MultiTableBatchWrite.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.BatchWrite.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.MultiTableBatchGet.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DataModel.BatchGet.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.MultiTableDocumentBatchGet.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#EndGetRemaining(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#EndGetNextSet(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#EndGetItem(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#EndUpdateItem(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#EndDeleteItem(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Table.#EndPutItem(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.MultiTableDocumentBatchWrite.#EndExecute(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.#EndExecute(System.IAsyncResult)")]
// Suppressions for Nested Generic types
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.IAmazonDynamoDB.#BatchWriteItem(System.Collections.Generic.Dictionary`2<System.String,System.Collections.Generic.List`1<Amazon.DynamoDBv2.Model.WriteRequest>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.Model.BatchWriteItemRequest.#.ctor(System.Collections.Generic.Dictionary`2<System.String,System.Collections.Generic.List`1<Amazon.DynamoDBv2.Model.WriteRequest>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#GetNextSetAsync(System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.Search.#GetRemainingAsync(System.Threading.CancellationToken)")]
// Suppression for base and subclass conversions
[module: SuppressMessage("Microsoft.Design", "CA1061:DoNotHideBaseClassMethods", Scope = "member", Target = "Amazon.DynamoDBv2.DocumentModel.DynamoDBList.#op_Implicit(Amazon.DynamoDBv2.DocumentModel.DynamoDBEntry[]):Amazon.DynamoDBv2.DocumentModel.DynamoDBList")]
// Do not call overridable methods in constructors
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Util.DynamoDBSectionRoot.#.ctor(System.Xml.Linq.XElement)")]