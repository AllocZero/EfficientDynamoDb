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

// Don't reference RegionEndpoints in the SDK unless absolutely necessary.
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.AssumeRoleAWSCredentials.#.ctor(Amazon.Runtime.AWSCredentials,System.String,System.String,Amazon.Runtime.AssumeRoleAWSCredentialsOptions)")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.AssumeRoleAWSCredentials.DefaultSTSClientRegion")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Justification = "<Pending>", Scope = "member", Target = "Amazon.Runtime.FederatedAWSCredentials.#.cctor()")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.FederatedAWSCredentials.DefaultSTSClientRegion")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.StoredProfileFederatedCredentials.#.ctor(System.String,System.String,System.Net.WebProxy)")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.StoredProfileFederatedCredentials.DefaultSTSClientRegion")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.Internal.Util.S3Uri.#.ctor(System.Uri)")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#DetermineRegion(System.String)")]
[module: SuppressMessage("AwsSdkRules", "CR1004:PreventRegionEndpointUseRule", Scope = "member", Target = "Amazon.Runtime.AssumeRoleWithWebIdentityCredentials._defaultSTSClientRegion")]

// This private static logger variable is used for testing.
[module: SuppressMessage("AwsSdkRules", "CR1002:PreventStaticLoggersRule", Scope="member", Target="Amazon.Runtime.Internal.HttpWebRequestFactory.#_logger")]

// Avoid empty interfaces
[module: SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces", Scope="type", Target="Amazon.Runtime.IAsyncResponseContext")]

// Suppressions for identifiers in base classes
[module: SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", Scope = "member", Target = "Amazon.Runtime.Internal.Util.ILogger.#Error(System.Exception,System.String,System.Object[])", MessageId = "Error")]

// Identifiers should have correct prefix
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceClient.#Invoke`2(!!1,System.AsyncCallback,System.Object,System.Boolean,Amazon.Runtime.Internal.Transform.IMarshaller`2<!!0,!!1>,Amazon.Runtime.Internal.Transform.ResponseUnmarshaller,Amazon.Runtime.Internal.Auth.AbstractAWSSigner)", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.IResponseUnmarshaller`2", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.IUnmarshaller`2", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.DictionaryUnmarshaller`4", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.IMarshaller`2", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.ListUnmarshaller`2", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.KeyValueUnmarshaller`4", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.IRequestMarshaller`2", MessageId = "T")]
[module: SuppressMessage("Microsoft.Naming", "CA1715:IdentifiersShouldHaveCorrectPrefix", Scope = "type", Target = "Amazon.Runtime.Internal.AutoConstructedDictionary`2", MessageId = "T")]

// Identifiers should not contain type names
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4PreSignedUrlSigner.#MaxAWS4PreSignedUrlExpiry", MessageId = "signed")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#FormatDateTime(System.DateTime,System.String)", MessageId = "string")]
[module: SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#IsPropertySet(System.Object,System.String)", MessageId = "object")]

// Array properties/fields
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Util.HashStream.#CalculatedHash")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Util.HashStream.#ExpectedHash")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Auth.AccessControlPolicy.Condition.#Values")]
[module: SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#TerminatorBytes")]
[module: SuppressMessage("Microsoft.Security", "CA2105:ArrayFieldsShouldNotBeReadOnly", Scope = "member", Target = "Amazon.Util.Internal.TypeFactory.#EmptyTypes")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.IRequest.#Content")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Util.LogMessage.#Args")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4SigningResult.#SignatureBytes")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4SigningResult.#SigningKey")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.Internal.Util.EncryptUploadPartStream.#InitializationVector")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope="member", Target="Amazon.Runtime.ILogMessage.#Args")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.SharedInterfaces.GenerateDataKeyResult.#KeyPlaintext")]
[module: SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope = "member", Target = "Amazon.Runtime.SharedInterfaces.GenerateDataKeyResult.#KeyCiphertext")]

// Nested generic types
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.IRequestMetrics.#Properties")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.IRequestMetrics.#Timings")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.SettingsCollection.#.ctor(System.Collections.Generic.Dictionary`2<System.String,System.Collections.Generic.Dictionary`2<System.String,System.Object>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.SharedInterfaces.ICoreAmazonSQS.#GetAttributesAsync(System.String)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.SharedInterfaces.ICoreAmazonS3.#GetAllObjectKeysAsync(System.String,System.String,System.Collections.Generic.IDictionary`2<System.String,System.Object>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Util.IniFile.#EditSection(System.String,System.Collections.Generic.List`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope="member", Target="Amazon.Runtime.Internal.Auth.AWS4Signer.#SortAndPruneHeaders(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.ParameterCollection.#GetSortedParametersList()")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#GetParametersToCanonicalize(Amazon.Runtime.Internal.IRequest)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeQueryParameters(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>,System.Boolean)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeQueryParameters(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Util.AWSHttpClient.#GetResponseHeadersAsync(System.String,System.String)")]


// Suppressions for exceptions in setters
[module: SuppressMessage("Microsoft.Usage", "CA2219:DoNotRaiseExceptionsInExceptionClauses", Scope = "member", Target = "Amazon.Runtime.Internal.HttpRequest.#WriteToRequestBody(System.IO.Stream,System.IO.Stream,System.Collections.Generic.IDictionary`2<System.String,System.String>,Amazon.Runtime.IRequestContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2219:Do not raise exceptions in finally clauses", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpRequest.WriteToRequestBodyAsync(System.IO.Stream,System.IO.Stream,System.Collections.Generic.IDictionary{System.String,System.String},Amazon.Runtime.IRequestContext)~System.Threading.Tasks.Task")]

// Suppressions for DisposableFieldsShouldBeDisposed
#if BCL35
// In this case the semaphore is being disposed. The semaphore.Close() has been renamed to semaphore.Dispose()
[module: SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", Scope = "member", Target = "Amazon.Runtime.RefreshingAWSCredentials.#Dispose(System.Boolean)")]
#endif

// Serialization-only classes
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityCredentials")]
[module: SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Scope = "type", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityInfo")]

// Passing base types
[module: SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Amazon.S3.Model.S3AccessControlList.#RemoveGrant(Amazon.S3.Model.S3Grantee,Amazon.S3.S3Permission)")]
[module: SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Amazon.S3.Model.S3BucketLoggingConfig.#RemoveGrant(Amazon.S3.Model.S3Grantee,Amazon.S3.S3Permission)")]
[module: SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory.#NewCannedACLCondition(Amazon.S3.S3CannedACL)")]

// Casting
[module: SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Amazon.Runtime.Internal.Util.WrapperStream.#SearchWrappedStream(System.Func`2<System.IO.Stream,System.Boolean>)")]
[module: SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.JsonUnmarshallerContext.#ReadText()")]

// Identifier suffix
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Amazon.Runtime.StreamTransferProgressArgs")]
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Amazon.Runtime.Internal.AutoConstructedList`1")]
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Amazon.Runtime.Internal.ParametersDictionaryFacade")]
[module: SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix", Scope = "type", Target = "Amazon.Runtime.Internal.ParameterCollection")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Not a collection", Scope = "type", Target = "~T:Amazon.Runtime.PaginatedResultKeyResponse`2")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Not a collection", Scope = "type", Target = "~T:Amazon.Runtime.IPaginatedEnumerable`1")]
[assembly: SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "Not a collection", Scope = "type", Target = "~T:Amazon.Runtime.PaginatedResponse`1")]


// Interface methods not callable by child types
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IRequestEvents.FireBeforeRequestEvent(System.Object,Amazon.Runtime.RequestEventArgs)")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IRequestEvents.AddBeforeRequestHandler(Amazon.Runtime.RequestEventHandler)")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.RemoveBeforeRequestHandler(Amazon.Runtime.RequestEventHandler)")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.get_RequestState()")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.set_StreamUploadProgressCallback(System.EventHandler`1<Amazon.Runtime.StreamTransferProgressArgs>)")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.get_StreamUploadProgressCallback()")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.get_UseSigV4()")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.set_UseSigV4(System.Boolean)")]
[module: SuppressMessage("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceRequest.#Amazon.Runtime.Internal.IAmazonWebServiceRequest.AddBeforeRequestHandler(Amazon.Runtime.RequestEventHandler)")]

// General exception types
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.InternalLog4netLogger.#loadStatics()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.BackgroundDispatcher`1.#HandleInvoked()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.FallbackCredentialsFactory.#GetCredentials(System.Boolean)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.MetricError.#.ctor(Amazon.Runtime.Internal.Util.Metric,System.Exception,System.String,System.Object[])")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceClient.#getRequestStreamCallback(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceClient.#getResponseCallback(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials.#GenerateNewCredentials()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Util.CryptoUtilFactory+CryptoUtil.#get_SHA256HashAlgorithmInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#PreserveStackTrace(System.Exception)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#ForceCanonicalPathAndQuery(System.Uri)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#DetermineFramework()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.EventStream.#BeginRead(System.Byte[],System.Int32,System.Int32,System.AsyncCallback,System.Object)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.AWSSection.#Serialize(System.String)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#DontUnescapePathDotsAndSlashes(System.Uri)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.RegionEndpoint.#TryLoadEndpointDefinitionsFromAssemblyDir()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.HttpHandler`1.#DontUnescapePathDotsAndSlashes(System.Uri)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.HttpHandler`1.#GetResponseCallback(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.HttpHandler`1.#GetRequestStreamCallback(System.IAsyncResult)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.PipelineHandler.#AsyncCallback(Amazon.Runtime.IAsyncExecutionContext)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.HttpErrorResponseExceptionHandler.#HandleSuppressed404(Amazon.Runtime.IExecutionContext,Amazon.Runtime.Internal.Transform.IWebResponseData)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.ErrorHandler.#InvokeAsyncCallback(Amazon.Runtime.IAsyncExecutionContext)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.AmazonServiceClient.#DontUnescapePathDotsAndSlashes(System.Uri)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.MetricError.#.ctor(Amazon.Runtime.Metric,System.Exception,System.String,System.Object[])")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Util.RequestMetrics.#ToString()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.HttpHandler`1.#CompleteFailedRequest(Amazon.Runtime.IHttpRequest`1<!0>)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.PersistenceManager.#loadSettingsType(System.String)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Util.Internal.InternalSDKUtils.#DetermineFramework()")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "Amazon.Runtime.Internal.AsyncRunner.#Run`1(System.Func`1<!!0>,System.Threading.CancellationToken)")]
[module: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope="member", Target="Amazon.Runtime.Internal.Settings.UserCrypto.#get_IsUserCryptAvailable()")]

// Overflow operations
[module: SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", Scope = "member", Target = "Amazon.Runtime.Internal.Util.EncryptStream.#Read(System.Byte[],System.Int32,System.Int32)", MessageId = "offset+16")]
[module: SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", Scope = "member", Target = "Amazon.Runtime.Internal.Util.EncryptUploadPartStream.#Read(System.Byte[],System.Int32,System.Int32)", MessageId = "offset+16")]

// Nested types, breaking change
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory+ArnComparisonType")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory+DateComparisonType")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory+IpAddressComparisonType")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory+NumericComparisonType")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory+StringComparisonType")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.Statement+StatementEffect")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.RegionEndpoint+Endpoint")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Internal.RegionEndpointProviderV2+RegionEndpoint")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.Internal.AsyncResult+AsyncRequestState")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.Internal.Settings.SettingsCollection+ObjectSettings")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.Internal.Settings.SettingsCollection+ObjectSettings")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.FallbackCredentialsFactory+CredentialsGenerator")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.RefreshingAWSCredentials+CredentialsRefreshState")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.StoredProfileFederatedCredentials+RequestUserCredential")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Util.AWSPublicIpAddressRange+AddressFormat")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.Internal.CSMFallbackConfigChain+ConfigurationSource")]
[module: SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope = "type", Target = "Amazon.Runtime.Internal.Auth.S3Signer+RegionDetectionUpdater")]

// Normalize strings to uppercase, breaking change
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Auth.AccessControlPolicy.ConditionFactory.#NewCondition(System.String,System.Boolean)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS3Signer.#GetCanonicalizedHeadersForStringToSign(Amazon.Runtime.Internal.IRequest)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS3Signer.#GetHeadersForStringToSign(Amazon.Runtime.Internal.IRequest)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CalculateSignature(System.Collections.Generic.IDictionary`2<System.String,System.String>,System.Collections.Generic.IDictionary`2<System.String,System.String>,System.String,System.String,System.String,System.String,Amazon.Runtime.ImmutableCredentials)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#DetermineRegion(Amazon.Runtime.IClientConfig)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#DetermineService(Amazon.Runtime.IClientConfig)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#GetCanonicalizedHeaders(System.Collections.Generic.List`1<System.String>,System.Collections.Generic.IDictionary`2<System.String,System.String>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#GetSignedHeaders(System.Collections.Generic.List`1<System.String>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.S3Signer.#buildCanonicalizedHeaders(System.Collections.Generic.IDictionary`2<System.String,System.String>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Util.StringUtils.#FromBool(System.Boolean)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaderNames(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaders(System.Collections.Generic.ICollection`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.S3Signer.#BuildCanonicalizedHeaders(System.Collections.Generic.IDictionary`2<System.String,System.String>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#DetermineSigningRegion(Amazon.Runtime.IClientConfig,System.String,Amazon.RegionEndpoint,Amazon.Runtime.Internal.IRequest)")]

[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaders(System.Collections.Generic.IDictionary`2<System.String,System.String>)")]
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaderNames(System.Collections.Generic.IDictionary`2<System.String,System.String>)")]

// Types names matching namespaces
[module: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.Policy")]
[module: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Amazon.Auth.AccessControlPolicy.Principal")]
[module: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Amazon.Runtime.Internal.Auth.CredentialProfile.Profile")]

// Uri should not be string
[module: SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#DetermineRegion(System.String)", MessageId = "0#")]
[module: SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#DetermineService(System.String)", MessageId = "0#")]

[module: SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope = "member", Target = "Amazon.Runtime.ClientConfig.#ServiceURL")]
[module: SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope = "member", Target = "Amazon.Runtime.IClientConfig.#ServiceURL")]

[module: SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Scope = "member", Target = "Amazon.Util.AWSSDKUtils.#UrlEncode(System.String,System.Boolean)")]

// Use properties, breaking changes
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.AWSCredentials.#GetCredentials()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials.#GetAvailableRoles()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.RequestMetrics.#GetErrors()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.AWSCredentials.#GetCredentialsAsync()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.PersistenceManager.#GetSettingsStoreFolder()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.SettingsWatcher.#GetSettings()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Util.RequestMetrics.#GetErrors()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.IHttpRequest`1.#GetRequestContentAsync()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.IClientConfig.#GetWebProxy()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Util.SAMLRoleProfile.#GetCurrentSession()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.FallbackRegionFactory.#GetRegionEndpoint()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.AWSRegion.#GetRegionEndpoint()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.SharedCredentialsFile.#GetSections()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.FallbackEndpointDiscoveryEnabledFactory.#GetEnabled()")]


// Use properties, huge change
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.ResponseMetadataUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.MemoryStreamUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.StringUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.LongUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.IntUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.ErrorResponseUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.FloatUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.JsonErrorResponseUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.DateTimeUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.ByteUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.DoubleUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.BoolUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.DateTimeEpochLongMillisecondsUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.NullableIntUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.NullableDateTimeUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Util.Internal.ITypeInfo.#GetElementType()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Util.Internal.ITypeInfo.#GetGenericTypeDefinition()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Util.Internal.ITypeInfo.#GetFields()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Util.Internal.ITypeInfo.#GetProperties()")]

// Use properties, design decision to suppress
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.Util.WrapperStream.#GetSeekableBaseStream()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope="member", Target="Amazon.Runtime.Internal.Transform.DecimalUnmarshaller.#GetInstance()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.ParameterCollection.#GetSortedParametersList()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope="member", Target="Amazon.Runtime.Internal.CSMFallbackConfigChain.#GetCSMConfig()")]

// Unused parameters
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Amazon.Runtime.AmazonWebServiceClient.#LogResponse(Amazon.Runtime.Internal.Util.RequestMetrics,Amazon.Runtime.Internal.IRequest,System.Net.HttpStatusCode)", MessageId = "request")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Amazon.Runtime.ConstantClass.#ToString(System.IFormatProvider)", MessageId = "provider")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.HttpWebRequestResponseData.#GetObjectData(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)", MessageId = "context")]
[module: SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.HttpWebRequestResponseData.#.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)", MessageId = "context")]

// Mark members as static, breaking changes
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#SignRequest(Amazon.Runtime.Internal.IRequest,Amazon.Runtime.IClientConfig,Amazon.Runtime.Internal.Util.RequestMetrics,System.String,System.String)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4PreSignedUrlSigner.#SignRequest(Amazon.Runtime.Internal.IRequest,Amazon.Runtime.IClientConfig,Amazon.Runtime.Internal.Util.RequestMetrics,System.String,System.String)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.S3Signer.#SignRequest(Amazon.Runtime.Internal.IRequest,Amazon.Runtime.IClientConfig,Amazon.Runtime.Internal.Util.RequestMetrics,System.String,System.String)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.S3Signer.#SignRequest(Amazon.Runtime.Internal.IRequest,Amazon.Runtime.Internal.Util.RequestMetrics,System.String,System.String)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope="member", Target="Amazon.Runtime.Internal.CSMHandler.#CSMMetricsCapture(Amazon.Runtime.IRequestContext,Amazon.Runtime.IResponseContext)")]
[module: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope="member", Target="Amazon.Runtime.Internal.CSMHandler.#PreInvoke(Amazon.Runtime.IExecutionContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpRequest.WriteToRequestBodyAsync(System.IO.Stream,System.Byte[],System.Collections.Generic.IDictionary{System.String,System.String})~System.Threading.Tasks.Task")]

// Link demand
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Util.TraceSourceUtil.#GetTraceSourceWithListeners(System.String,System.Diagnostics.SourceLevels)")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.UserCrypto.#Encrypt(System.String)")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.UserCrypto.#Decrypt(System.String)")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.UserCrypto.#ConvertData(System.Byte[])")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.SettingsWatcher.#set_Enable(System.Boolean)")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.SettingsWatcher.#get_Enable()")]
[module: SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.SettingsWatcher.#.ctor(System.String,System.String)")]



// Initialize reference type static fields inline

[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.SharedCredentialsFile.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.FallbackRegionFactory.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.PersistenceManager.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.FallbackCredentialsFactory.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.Internal.HttpWebRequestFactory.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope="member", Target="Amazon.Runtime.MonitoringListener.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope="member", Target="Amazon.Runtime.Internal.DeterminedCSMConfiguration.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope="member", Target="Amazon.Util.Internal.EnvironmentVariableSource.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Util.Internal.EnvironmentVariableSource.#.cctor()")]
[module: SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "Amazon.Runtime.FallbackEndpointDiscoveryEnabledFactory.#.cctor()")]

// Abstract types should not have constructors
[module: SuppressMessage("Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors", Scope = "type", Target = "Amazon.Runtime.ClientConfig")]

// Avoid uncalled private code, properties used in serialization
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityBase.#set_Code(System.String)")]
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityBase.#get_LastUpdated()")]
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityBase.#set_LastUpdated(System.DateTime)")]
[module: SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "Amazon.Runtime.InstanceProfileAWSCredentials+SecurityBase.#set_Message(System.String)")]

// Do not call overridable methods in constructors
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#.ctor(Amazon.Runtime.AWSCredentials,Amazon.Runtime.ClientConfig,Amazon.Runtime.AbstractWebServiceClient+AuthenticationTypes)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.ClientConfig.#.ctor()")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.AmazonServiceClient.#.ctor(Amazon.Runtime.AWSCredentials,Amazon.Runtime.ClientConfig)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.Internal.ErrorHandler.#.ctor(Amazon.Runtime.Internal.Util.ILogger)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "ThirdParty.MD5.MD5Managed.#.ctor()")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.V4ClientSection.#.ctor(System.Xml.Linq.XElement)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.V4ClientSectionRoot.#.ctor(System.Xml.Linq.XElement)")]

// Events should have before or after prefix
[module: SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#BeforeRequestEvent")]
[module: SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#AfterResponseEvent")]
[module: SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Amazon.Runtime.AmazonServiceClient.#BeforeRequestEvent")]
[module: SuppressMessage("Microsoft.Naming", "CA1713:EventsShouldNotHaveBeforeOrAfterPrefix", Scope = "member", Target = "Amazon.Runtime.AmazonServiceClient.#AfterResponseEvent")]

// Use generic event handler instances, breaking change
[module: SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "type", Target = "Amazon.Runtime.RequestEventHandler")]
[module: SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "type", Target = "Amazon.Runtime.ExceptionEventHandler")]
[module: SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "type", Target = "Amazon.Runtime.ResponseEventHandler")]
[module: SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope = "type", Target = "Amazon.Runtime.PreRequestEventHandler")]

// Declare event handlers correctly, breaking change
[module: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#BeforeRequestEvent")]
[module: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#ExceptionEvent")]
[module: SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly", Scope = "member", Target = "Amazon.Runtime.AbstractWebServiceClient.#AfterResponseEvent")]

// Use events where appropriate
[module: SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.Internal.IRequestEvents.#FireBeforeRequestEvent(System.Object,Amazon.Runtime.RequestEventArgs)")]

// Do not call overridable methods in constructors
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.Internal.Util.EncryptionWrapper.#.ctor()")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "Amazon.Runtime.Internal.Util.DecryptionWrapper.#.ctor()")]

// Suppressions for Runtime generic methods
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.RuntimePipeline.#RemoveHandler`1()")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.RuntimePipeline.#AddHandlerAfter`1(Amazon.Runtime.IPipelineHandler)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.RuntimePipeline.#ReplaceHandler`1(Amazon.Runtime.IPipelineHandler)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.RuntimePipeline.#AddHandlerBefore`1(Amazon.Runtime.IPipelineHandler)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.DefaultRetryPolicy.#IsInnerException`1(System.Exception)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Util.PaginatedResourceFactory.#Create`3(Amazon.Util.PaginatedResourceInfo)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.ServiceClientHelpers.#CreateServiceFromAnother`2(Amazon.Runtime.AmazonServiceClient)")]
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Util.Internal.PlatformServices.ServiceFactory.#RegisterService`1(System.Type)")]

[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.Internal.DefaultRetryPolicy.#IsInnerException`1(System.Exception,!!0&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Util.ICache`2.#GetValue(!0,System.Func`2<!0,!1>,System.Boolean&)", MessageId = "2#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Util.IniFile.#TryGetSection(System.Text.RegularExpressions.Regex,System.String&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileTypeDetector.#DetectProfileTypes(Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileOptions,System.Nullable`1<Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileType>&,System.Collections.Generic.HashSet`1<Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileType>&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileTypeDetector.#DetectProfileTypes(Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileOptions,System.Nullable`1<Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileType>&,System.Collections.Generic.HashSet`1<Amazon.Runtime.Internal.Auth.CredentialProfile.ProfileType>&)", MessageId = "2#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Util.Internal.SettingsManager.#TryGetObjectByProperty(System.String,System.String,System.String&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "2#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Util.Internal.NamedSettingsManager.#TryGetObject(System.String,System.String&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.Internal.CredentialProfilePropertyMapping.#ExtractProfileParts(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.Collections.Generic.HashSet`1<System.String>,Amazon.Runtime.CredentialManagement.CredentialProfileOptions&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "2#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.Internal.CredentialProfilePropertyMapping.#ExtractProfileParts(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.Collections.Generic.HashSet`1<System.String>,Amazon.Runtime.CredentialManagement.CredentialProfileOptions&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "3#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.Internal.CredentialProfilePropertyMapping.#ExtractProfileParts(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.Collections.Generic.HashSet`1<System.String>,Amazon.Runtime.CredentialManagement.CredentialProfileOptions&,System.Collections.Generic.Dictionary`2<System.String,System.String>&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "2#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.Internal.CredentialProfilePropertyMapping.#ExtractProfileParts(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.Collections.Generic.HashSet`1<System.String>,Amazon.Runtime.CredentialManagement.CredentialProfileOptions&,System.Collections.Generic.Dictionary`2<System.String,System.String>&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "3#")]
[module: SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Scope = "member", Target = "Amazon.Runtime.CredentialManagement.Internal.CredentialProfilePropertyMapping.#ExtractProfileParts(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.Collections.Generic.HashSet`1<System.String>,Amazon.Runtime.CredentialManagement.CredentialProfileOptions&,System.Collections.Generic.Dictionary`2<System.String,System.String>&,System.Collections.Generic.Dictionary`2<System.String,System.String>&)", MessageId = "4#")]

// Suppression for methods on IHttpRequest
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.IHttpRequest`1.#GetRequestContent()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.IHttpRequest`1.#GetResponse()")]
[module: SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "Amazon.Runtime.CredentialProfile.#GetAWSCredentials()")]

// Suppression for HttpWebRequestResponseData, HttpWebRequestResponseData.ResponseBody is disposed on its own.
[module: SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "Amazon.Runtime.Internal.Transform.HttpWebRequestResponseData")]

// Suppression for UserCrypto DATA_BLOB, native resources cleaned up in the code
[module: SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable", Scope = "type", Target = "Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB")]

// Passing Uri instead of string (in these cases, the uri overload is called by the string overload)
[module: SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Scope = "member", Target = "Amazon.S3.Internal.AmazonS3RedirectHandler.#FinalizeForRedirect(Amazon.Runtime.IExecutionContext,System.String)")]
[module: SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Scope = "member", Target = "Amazon.Util.AWSHttpClient.#GetResponseHeadersAsync(System.String,System.String)")]
[module: SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Scope = "member", Target = "Amazon.Util.AWSHttpClient.#GetStreamAsync(System.String)")]
[module: SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings", Scope="member", Target="Amazon.Util.AWSHttpClient.#PutRequestUriAsync(System.String,Amazon.Util.AWSStreamContent,System.Collections.Generic.IDictionary`2<System.String,System.String>)")]

// Move PInvoke to separate file
[module: SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.UserCrypto.#CryptUnprotectData(Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&,System.String,Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&,System.IntPtr,Amazon.Runtime.Internal.Settings.UserCrypto+CRYPTPROTECT_PROMPTSTRUCT&,Amazon.Runtime.Internal.Settings.UserCrypto+CryptProtectFlags,Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&)")]
[module: SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Scope = "member", Target = "Amazon.Runtime.Internal.Settings.UserCrypto.#CryptProtectData(Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&,System.String,Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&,System.IntPtr,Amazon.Runtime.Internal.Settings.UserCrypto+CRYPTPROTECT_PROMPTSTRUCT&,Amazon.Runtime.Internal.Settings.UserCrypto+CryptProtectFlags,Amazon.Runtime.Internal.Settings.UserCrypto+DATA_BLOB&)")]

// Pass types by reference
[module: SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Scope = "member", Target = "Amazon.Util.Internal.InternalSDKUtils.#SetIsSet`2(System.Boolean,System.Collections.Generic.Dictionary`2<!!0,!!1>&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Scope = "member", Target = "Amazon.Util.Internal.InternalSDKUtils.#SetIsSet`1(System.Boolean,System.Collections.Generic.List`1<!!0>&)", MessageId = "1#")]
[module: SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Scope = "member", Target = "Amazon.Util.Internal.InternalSDKUtils.#SetIsSet`1(System.Boolean,System.Nullable`1<!!0>&)", MessageId = "1#")]

// Collections that do not require ICollection<T> interface
[module: SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface", Scope = "type", Target = "Amazon.WritableConfigurationElementCollection`1")]

// Empty Logger instance can be cached
[module: SuppressMessage("AwsSdkRules", "CR1002:PreventStaticLoggersRule", Scope = "member", Target = "Amazon.Runtime.Internal.Util.Logger.#emptyLogger")]

// Call HashAlgorithm.Create is for "SHA-256"
[module: SuppressMessage("AwsSdkRules", "CR1001:PreventHashAlgorithmCreateRule", Scope = "member", Target = "Amazon.Util.CryptoUtilFactory+CryptoUtil.#get_SHA256HashAlgorithmInstance()")]

// Visible instance fields
[module: SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Scope = "member", Target = "Amazon.Runtime.RefreshingAWSCredentials.#currentState")]

// Supression due to IL2CPP error
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#SortHeaders(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaderNames(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaders(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]
[module: SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.CredentialProfile.ProfilePropertyMapping.#Convert(Amazon.Runtime.Internal.Auth.CredentialProfile.ImmutableProfileOptions)")]

// ignore as we are not converting back string to upper case.
[module: SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope = "member", Target = "Amazon.Runtime.Internal.Auth.AWS4Signer.#CanonicalizeHeaders(System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>)")]

// ignore as this is used to create a default instance of the exception object
[module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.ResponseUnmarshaller.#GetDefaultErrorMessage`1()")]

// ignore as this is used to create an instance of the object in the CSM fall back chain
[module: SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Scope = "member", Target = "Amazon.Runtime.Internal.CSMFallbackConfigChain.#.ctor()", MessageId = "Amazon.Runtime.Internal.AppConfigCSMConfigs")]
[module: SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Scope = "member", Target = "Amazon.Runtime.Internal.CSMFallbackConfigChain.#.ctor()", MessageId = "Amazon.Runtime.Internal.ProfileCSMConfigs")]
[module: SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", Scope = "member", Target = "Amazon.Runtime.Internal.CSMFallbackConfigChain.#.ctor()", MessageId = "Amazon.Runtime.Internal.EnvironmentVariableCSMConfigs")]

//Suppression is subject to discussion
[module: SuppressMessage("AwsSdkRules", "CR1003:PreventDateTimeNowUseRule", Scope = "member", Target = "Amazon.Runtime.Internal.MonitoringAPICall.#.ctor()")][module: SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "Amazon.Runtime.Internal.Transform.ResponseUnmarshaller.#GetDefaultErrorMessage`1()")]

// Amazon.Runtime.Internal.Signer type conflicts with Amazon.Signer service namespace. 
[module: SuppressMessage("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Scope = "type", Target = "Amazon.Runtime.Internal.Signer")]


[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:Amazon.MissingTypes.IOrderedDictionary")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.Asn1EncodableVector")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.Asn1EncodableVector")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.Asn1Sequence")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.Asn1Sequence")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.DerSequence")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Asn1.DerSequence")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.IJsonWrapper")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.IJsonWrapper")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1010:Collections should implement generic interface", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonWriter")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Internal.RegionEndpointProviderV2.RegionEndpoint.TryLoadEndpointDefinitionsFromAssemblyDir~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.CredentialManagement.AWSCredentialsFactory.GetCredentialSourceAWSCredentials(System.String,System.Boolean)~Amazon.Runtime.AWSCredentials")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.CredentialManagement.Internal.SAMLRoleSessionManager.TryGetRoleSession(System.String,Amazon.Runtime.SAMLImmutableCredentials@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.CredentialManagement.SharedCredentialsFile.TryGetProfile(System.String,System.Boolean,Amazon.Runtime.CredentialManagement.CredentialProfile@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.CredentialManagement.NetSDKCredentialsFile.TryGetProfile(System.String,Amazon.Runtime.CredentialManagement.CredentialProfile@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.DefaultInstanceProfileAWSCredentials.RenewCredentials(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.EventStreams.Internal.EventStream`2.ProcessLoop(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.FallbackEndpointDiscoveryEnabledFactory.GetEnabled~System.Nullable{System.Boolean}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.FallbackRegionFactory.GetRegionEndpoint(System.Boolean)~Amazon.RegionEndpoint")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.CSMFallbackConfigChain.GetCSMConfig~Amazon.Runtime.Internal.CSMConfiguration")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.EndpointDiscoveryHandler.InvokeAsyncCallback(Amazon.Runtime.IAsyncExecutionContext)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.EndpointDiscoveryResolverBase.ProcessInvokeEndpointOperation(System.String,System.Func{System.Collections.Generic.IList{Amazon.Runtime.Internal.DiscoveryEndpointBase}},System.Boolean)~System.Collections.Generic.IEnumerable{Amazon.Runtime.Internal.DiscoveryEndpointBase}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpHandler`1.GetRequestStreamCallbackHelper(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpHandler`1.GetResponseCallbackHelper(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpWebRequestFactory.#ctor(Amazon.Runtime.Internal.IAmazonSecurityProtocolManager,Amazon.Runtime.Internal.Util.ILogger)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.RuntimeAsyncResult.InvokeCallback")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.Util.GuidUtils.TryParseGuid(System.String,System.Guid@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.MonitoringListener.BeginPostMessagesOverUDPInvoke(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.MonitoringListener.EndSendMessagesOverUDPInvoke(System.IAsyncResult)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.MonitoringListener.PostMessagesOverUDP(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.SAMLImmutableCredentials.FromJson(System.String)~Amazon.Runtime.SAMLImmutableCredentials")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.EC2InstanceMetadata.FetchData(System.String,System.Boolean)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.ProfileManager.ListProfiles~System.Collections.Generic.IEnumerable{Amazon.Util.ProfileSettingsBase}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.ProfileManager.TryGetAWSCredentials(System.String,Amazon.Runtime.AWSCredentials@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.ProfileManager.TryGetProfile``1(System.String,``0@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.ProfileManager.TryGetSAMLEndpoint(System.String,Amazon.Util.SAMLEndpointSettings@)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~P:Amazon.Util.EC2InstanceMetadata.IAMInstanceProfileInfo")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~P:Amazon.Util.EC2InstanceMetadata.IAMSecurityCredentials")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~P:Amazon.Util.EC2InstanceMetadata.IsIMDSEnabled")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~P:Amazon.Util.EC2InstanceMetadata.Region")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:Use properties where appropriate", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.GetJsonType~ThirdParty.Json.LitJson.JsonType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:Amazon.Runtime.AmazonWebServiceRequest.Amazon#Runtime#Internal#IAmazonWebServiceRequest#AddBeforeRequestHandler(Amazon.Runtime.RequestEventHandler)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:Amazon.Runtime.AmazonWebServiceRequest.Amazon#Runtime#Internal#IAmazonWebServiceRequest#RemoveBeforeRequestHandler(Amazon.Runtime.RequestEventHandler)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#ICollection#CopyTo(System.Array,System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#Contains(System.Object)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#GetEnumerator~System.Collections.IDictionaryEnumerator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#Remove(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IEnumerable#GetEnumerator~System.Collections.IEnumerator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#Contains(System.Object)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#IndexOf(System.Object)~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#Insert(System.Int32,System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#Remove(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#RemoveAt(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#Specialized#IOrderedDictionary#GetEnumerator~System.Collections.IDictionaryEnumerator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#Specialized#IOrderedDictionary#Insert(System.Int32,System.Object,System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.System#Collections#Specialized#IOrderedDictionary#RemoveAt(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetBoolean~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetDouble~System.Double")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetInt~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetLong~System.Int64")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetString~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetUInt~System.UInt32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#GetULong~System.UInt64")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetBoolean(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetDouble(System.Double)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetInt(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetLong(System.Int64)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetString(System.String)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetUInt(System.UInt32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ThirdParty#Json#LitJson#IJsonWrapper#SetULong(System.UInt64)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:Amazon.Runtime.AmazonWebServiceRequest.Amazon#Runtime#Internal#IAmazonWebServiceRequest#RequestState")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#ICollection#IsSynchronized")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#ICollection#SyncRoot")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#IsFixedSize")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#IsReadOnly")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#Keys")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IDictionary#Values")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#IsFixedSize")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~P:ThirdParty.Json.LitJson.JsonData.System#Collections#IList#IsReadOnly")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1051:Do not declare visible instance fields", Scope = "member", Target = "~F:ThirdParty.BouncyCastle.Asn1.Utilities.FilterStream.s")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonMapper")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Scope = "member", Target = "~M:Amazon.Runtime.ClientConfig.DetermineServiceURL~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Scope = "member", Target = "~M:Amazon.Runtime.IClientConfig.DetermineServiceURL~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Scope = "member", Target = "~M:Amazon.Runtime.SharedInterfaces.ICoreAmazonS3.GeneratePreSignedURL(System.String,System.String,System.DateTime,System.Collections.Generic.IDictionary{System.String,System.Object})~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Scope = "member", Target = "~M:Amazon.Util.AWSSDKUtils.ProtectEncodedSlashUrlEncode(System.String,System.Boolean)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1055:Uri return values should not be strings", Scope = "member", Target = "~M:Amazon.Util.AWSSDKUtils.UrlEncode(System.Int32,System.String,System.Boolean)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1060:Move pinvokes to native methods class", Scope = "type", Target = "~T:Amazon.Runtime.Internal.Settings.UserCrypto")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Scope = "member", Target = "~P:ThirdParty.Ionic.Zlib.CrcCalculatorStream.Length")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.AssumeRoleAWSCredentialsOptions.MfaTokenCode")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.DecryptStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.ReadOnlyWrapperStream.Length")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.ReadOnlyWrapperStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.HashStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "<Pending>", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.CachingWrapperStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Utilities.IO.Pem.PemObjectGenerator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1715:Identifiers should have correct prefix", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Utilities.IO.Pem.PemObjectParser")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~F:ThirdParty.BouncyCastle.Asn1.Asn1InputStream.Integer")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:Amazon.Runtime.ConstantClass.Equals(Amazon.Runtime.ConstantClass)~System.Boolean")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Asn1.Asn1Sequence.AddObject(ThirdParty.BouncyCastle.Asn1.Asn1Encodable)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Asn1.DerSequence.#ctor(ThirdParty.BouncyCastle.Asn1.Asn1Encodable)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Math.BigInteger.CompareTo(System.Object)~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Utilities.IO.Pem.PemObjectParser.ParseObject(ThirdParty.BouncyCastle.Utilities.IO.Pem.PemObject)~System.Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.#ctor(System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonMapper.ToJson(System.Object)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonMapper.ToJson(System.Object,ThirdParty.Json.LitJson.JsonWriter)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "type", Target = "~T:Amazon.Runtime.EventStreams.EventStreamHeaderType")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1720:Identifier contains type name", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonToken")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Scope = "member", Target = "~M:ThirdParty.Ionic.Zlib.CRC32.#cctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonMapper.#cctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonWriter.#cctor")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1813:Avoid unsealed attributes", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonPropertyAttribute")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Scope = "member", Target = "~P:Amazon.Runtime.EventStreams.EventStreamMessage.Payload")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Scope = "member", Target = "~P:Amazon.Runtime.Internal.DefaultRequest.Content")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Scope = "member", Target = "~P:ThirdParty.BouncyCastle.Asn1.DerInteger.Bytes")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Scope = "member", Target = "~P:ThirdParty.BouncyCastle.Utilities.IO.Pem.PemObject.Content")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Asn1.Asn1InputStream.CreatePrimitiveDerObject(System.Int32,System.Byte[])~ThirdParty.BouncyCastle.Asn1.Asn1Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.Asn1.Asn1InputStream.ReadObject~ThirdParty.BouncyCastle.Asn1.Asn1Object")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.OpenSsl.PemReader.ReadPrivatekey~System.Security.Cryptography.RSAParameters")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2229:Implement serialization constructors", Scope = "type", Target = "~T:ThirdParty.BouncyCastle.Utilities.IO.Pem.PemGenerationException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2229:Implement serialization constructors", Scope = "type", Target = "~T:ThirdParty.Json.LitJson.JsonException")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.EndpointDiscoveryHandler.InvokeAsync``1(Amazon.Runtime.IExecutionContext)~System.Threading.Tasks.Task{``0}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpHandler`1.CompleteFailedRequest(Amazon.Runtime.IExecutionContext,Amazon.Runtime.IHttpRequest{`0})~System.Threading.Tasks.Task")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.HttpHandler`1.InvokeAsync``1(Amazon.Runtime.IExecutionContext)~System.Threading.Tasks.Task{``0}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.Internal.RetryHandler.InvokeAsync``1(Amazon.Runtime.IExecutionContext)~System.Threading.Tasks.Task{``0}")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Runtime.MonitoringListener.PostMessagesOverUDPAsync(System.String)~System.Threading.Tasks.Task")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Amazon.Runtime.Internal.EnvironmentVariableInternalConfiguration.GetEnvironmentVariable``1(System.String)~System.Nullable{``0}")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Scope = "type", Target = "~T:Amazon.Runtime.Internal.Transform.HttpClientResponseData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Scope = "type", Target = "~T:Amazon.Runtime.Internal.Util.AsyncHelpers.ExclusiveSynchronizationContext")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1024:Use properties where appropriate", Scope = "member", Target = "~M:Amazon.Runtime.ClientConfig.GetWebProxy~System.Net.IWebProxy")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("AwsSdkRules", "CR1002:Do not store ILogger instances in static members.", Justification = "<Pending>", Scope = "member", Target = "~F:Amazon.Runtime.Internal.Settings.PersistenceManager._logger")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:Amazon.Runtime.Internal.Settings.PersistenceManager.DecryptAnyEncryptedValues(System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.Dictionary{System.String,System.Object}})")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.Internal.InternalSDKUtils.DetermineOS~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.Internal.InternalSDKUtils.DetermineOSVersion~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.Internal.InternalSDKUtils.DetermineRuntime~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Scope = "member", Target = "~M:Amazon.Util.Internal.InternalSDKUtils.PlatformUserAgent~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.Amazon#MissingTypes#IOrderedDictionary#GetEnumerator~System.Collections.IDictionaryEnumerator")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.Amazon#MissingTypes#IOrderedDictionary#Insert(System.Int32,System.Object,System.Object)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1033:Interface methods should be callable by child types", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.Amazon#MissingTypes#IOrderedDictionary#RemoveAt(System.Int32)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1802:Use literals where appropriate", Scope = "member", Target = "~F:Amazon.Util.Internal.InternalSDKUtils._unknown")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Scope = "member", Target = "~M:Amazon.Util.CryptoUtilFactory.CryptoUtil.CreateKeyedHashAlgorithm(Amazon.Runtime.SigningAlgorithm)~System.Security.Cryptography.KeyedHashAlgorithm")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:Amazon.Runtime.ClientConfig.SetUseNagleIfAvailable(System.Boolean)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:Amazon.Runtime.Internal.Transform.HttpClientResponseData.GetFirstHeaderValue(System.Net.Http.Headers.HttpHeaders,System.String)~System.String")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:ThirdParty.BouncyCastle.OpenSsl.PemReader.GetAlignmentValue(System.Byte[])~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:ThirdParty.Ionic.Zlib.CRC32._InternalComputeCrc32(System.UInt32,System.Byte)~System.Int32")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Scope = "member", Target = "~M:ThirdParty.Json.LitJson.JsonData.ToJsonData(System.Object)~ThirdParty.Json.LitJson.JsonData")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("AwsSdkRules", "CR1003:Do not use DateTime.Now or DateTime.UtcNow, use AWSSDKUtils.CorrectedNow or AWSSDKUtils.CorrectedUtcNow", Justification = "<Pending>", Scope = "member", Target = "~M:Amazon.Runtime.Internal.TokenBucket.GetTimeInSeconds~System.Double")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2214:Do not call overridable methods in constructors", Justification = "<Pending>", Scope = "member", Target = "~M:Amazon.Runtime.Internal.TokenBucket.#ctor(System.Double,System.Double,System.Double,System.Double,System.Double)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.HashStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.ReadOnlyWrapperStream.Length")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.AssumeRoleAWSCredentialsOptions.MfaTokenCode")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.CachingWrapperStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.DecryptStream.Position")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations", Justification = "Stream does not support property gained through inheritance.", Scope = "member", Target = "~P:Amazon.Runtime.Internal.Util.ReadOnlyWrapperStream.Position")]
