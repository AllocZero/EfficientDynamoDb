using System;
using System.Net;
#if AWS_ASYNC_API
using System.Threading.Tasks;
#endif

namespace Amazon.Runtime.SharedInterfaces
{
    /// <summary>
    /// ICoreAmazonSTS is not meant to be used directly. It defines Security Token
    /// service with basic .NET types and allows other services to be able to use the service as 
    /// a runtime dependency. This interface is implemented by the AmazonSecurityTokenServiceClient 
    /// defined in the AWSSDK.SecurityToken assembly.
    /// </summary>
    public interface ICoreAmazonSTS
    {
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon Security Token
        /// service within other service assemblies.
        /// Please use AmazonSecurityTokenServiceClient to access the Amazon Security Token
        /// service instead.
        /// </para>
        /// Use Amazon Security Token Service to assume a role.
        /// <remarks>
        /// Proxy settings that are required for the HTTPS and STS calls made during the authentication/credential
        /// generation process are supported and should have been configured on the STS ClientConfig instance
        /// associated with the STS client instance exposing this interface.
        /// </remarks>
        /// </summary>
        /// <param name="roleArn">The Amazon Resource Name (ARN) of the role to assume.</param>
        /// <param name="roleSessionName"> An identifier for the assumed role session.</param>
        /// <param name="options">Options to be used in the call to AssumeRole.</param>
        /// <returns></returns>
        AssumeRoleImmutableCredentials CredentialsFromAssumeRoleAuthentication(string roleArn, string roleSessionName, AssumeRoleAWSCredentialsOptions options);

#if NETSTANDARD20 || NETCOREAPP3_1 // In the NETSTANDARD flavors of the SDK ICoreAmazonSTS is declared without CredentialsFromSAMLAuthentication,
    }             // we cannot add a new method to the interface for backward compatibility concerns. 

    /// <summary>
    /// ICoreAmazonSTS_SAML is not meant to be used directly. It defines Security Token
    /// service with basic .NET types and allows other services to be able to use the service as 
    /// a runtime dependency. This interface is implemented by the AmazonSecurityTokenServiceClient 
    /// defined in the AWSSDK.SecurityToken assembly.
    /// </summary>
    public interface ICoreAmazonSTS_SAML
    {
#endif
#if BCL || (NETSTANDARD && !NETSTANDARD13)
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon Security Token
        /// service within other service assemblies.
        /// Please use AmazonSecurityTokenServiceClient to access the Amazon Security Token
        /// service instead.
        /// </para>
        /// Authenticates against a federated identity endpoint supporting SAML and returns 
        /// temporary AWS credentials for the supplied role. 
        /// </summary>
        /// <param name="endpoint">The endpoint for the federated identity provider</param>
        /// <param name="authenticationType">The authentication type to use (NTLM, Kerberos etc)</param>
        /// <param name="roleARN">The ARN of the role the user is to assume following authentication</param>
        /// <param name="credentialDuration">TTL duration for the generated credentials.</param>
        /// <param name="userCredential">
        /// Optional; alternate user credential for authentication. If null the identity of the
        /// current process is used.
        /// </param>
        /// <returns>Generated credential data, including SAML-related information such as subject.</returns>
        /// <remarks>
        /// Proxy settings that are required for the HTTPS and STS calls made during the authentication/credential
        /// generation process are supported and should have been configured on the STS ClientConfig instance 
        /// associated with the STS client instance exposing this interface.
        /// </remarks>
        SAMLImmutableCredentials CredentialsFromSAMLAuthentication(string endpoint,
                                                                   string authenticationType,
                                                                   string roleARN,
                                                                   TimeSpan credentialDuration,
                                                                   ICredentials userCredential);
#endif
    }

    public interface ICoreAmazonSTS_WebIdentity : IDisposable
    {
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon Security Token
        /// service within other service assemblies.
        /// Please use AmazonSecurityTokenServiceClient to access the Amazon Security Token
        /// service instead.
        /// </para>
        /// Use Amazon Security Token Service to assume a role.
        /// <remarks>
        /// Proxy settings that are required for the HTTPS and STS calls made during the authentication/credential
        /// generation process are supported and should have been configured on the STS ClientConfig instance
        /// associated with the STS client instance exposing this interface.
        /// </remarks>
        /// </summary>
        /// <param name="webIdentityToken">The web identity token used to assume the role.</param>
        /// <param name="roleArn">The Amazon Resource Name (ARN) of the role to assume.</param>
        /// <param name="roleSessionName">An identifier for the assumed role session.</param>
        /// <param name="options">Options to be used in the call to AssumeRoleWithWebIdentity</param>
        /// <returns></returns>
        AssumeRoleImmutableCredentials CredentialsFromAssumeRoleWithWebIdentityAuthentication(string webIdentityToken, string roleArn, string roleSessionName, AssumeRoleWithWebIdentityCredentialsOptions options);

#if AWS_ASYNC_API
        /// <summary>
        /// <para>
        /// This method is used internally to access the Amazon Security Token
        /// service within other service assemblies.
        /// Please use AmazonSecurityTokenServiceClient to access the Amazon Security Token
        /// service instead.
        /// </para>
        /// Use Amazon Security Token Service to assume a role.
        /// <remarks>
        /// Proxy settings that are required for the HTTPS and STS calls made during the authentication/credential
        /// generation process are supported and should have been configured on the STS ClientConfig instance
        /// associated with the STS client instance exposing this interface.
        /// </remarks>
        /// </summary>
        /// <param name="webIdentityToken">The web identity token used to assume the role.</param>
        /// <param name="roleArn">The Amazon Resource Name (ARN) of the role to assume.</param>
        /// <param name="roleSessionName">An identifier for the assumed role session.</param>
        /// <param name="options">Options to be used in the call to AssumeRoleWithWebIdentity</param>
        /// <returns></returns>
        Task<AssumeRoleImmutableCredentials> CredentialsFromAssumeRoleWithWebIdentityAuthenticationAsync(string webIdentityToken, string roleArn, string roleSessionName, AssumeRoleWithWebIdentityCredentialsOptions options);
#endif
    }
}
