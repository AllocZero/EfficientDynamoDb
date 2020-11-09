/*******************************************************************************
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not use
 * this file except in compliance with the License. A copy of the License is located at
 *
 * http://aws.amazon.com/apache2.0
 *
 * or in the "license" file accompanying this file. This file is distributed on
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions and
 * limitations under the License.
 * *****************************************************************************
 *    __  _    _  ___
 *   (  )( \/\/ )/ __)
 *   /__\ \    / \__ \
 *  (_)(_) \/\/  (___/
 *
 *  AWS SDK for .NET
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using Amazon.Runtime;
using ThirdParty.Json.LitJson;
using System.Globalization;
using Amazon.Runtime.Internal.Util;
using AWSSDK.Runtime.Internal.Util;

namespace Amazon.Util
{
    /// <summary>
    /// Provides access to EC2 instance metadata when running on an EC2 instance.
    /// If this class is used on a non-EC2 instance, the properties in this class
    /// will return null.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Amazon EC2 instances can access instance-specific metadata, as well as data supplied when launching the instances, using a specific URI.
    /// </para>
    /// <para>
    /// You can use this data to build more generic AMIs that can be modified by configuration files supplied at launch time. 
    /// For example, if you run web servers for various small businesses, they can all use the same AMI and retrieve their content from the 
    /// Amazon S3 bucket you specify at launch. To add a new customer at any time, simply create a bucket for the customer, add their content, 
    /// and launch your AMI.
    /// </para>
    /// <para>
    /// More information about EC2 Metadata <see href="http://docs.aws.amazon.com/AWSEC2/latest/UserGuide/AESDG-chapter-instancedata.html"/>
    /// </para>
    /// </remarks>
    public static class EC2InstanceMetadata
    {
        public static readonly string
            EC2_METADATA_SVC = "http://169.254.169.254",
            LATEST = "/latest",
            EC2_METADATA_ROOT = EC2_METADATA_SVC + LATEST + "/meta-data",
            EC2_USERDATA_ROOT = EC2_METADATA_SVC + LATEST + "/user-data",
            EC2_DYNAMICDATA_ROOT = EC2_METADATA_SVC + LATEST + "/dynamic",
            AWS_EC2_METADATA_DISABLED = "AWS_EC2_METADATA_DISABLED",
            EC2_APITOKEN_URL = EC2_METADATA_SVC + LATEST + "/api/token";

        private static int
            DEFAULT_RETRIES = 3,
            MIN_PAUSE_MS = 250,
            MAX_RETRIES = 3,
            DEFAULT_APITOKEN_TTL = 21600;

        private static Dictionary<string, string> _cache = new Dictionary<string, string>();

        private static bool useNullToken = false; 

        /// <summary>
        /// </summary>
        public static bool IsIMDSEnabled
        {
            get
            {
                const string True = "true";
                string value = string.Empty;
                try
                {
                    value = System.Environment.GetEnvironmentVariable(AWS_EC2_METADATA_DISABLED);
                } catch { };
                return !True.Equals(value, StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Allows to configure the proxy used for HTTP requests. The default value is null.
        /// </summary>
        public static IWebProxy Proxy
        {
            get; set;
        }

        /// <summary>
        /// The AMI ID used to launch the instance.
        /// </summary>
        public static string AmiId
        {
            get { return FetchData("/ami-id"); }
        }

        /// <summary>
        /// The index of this instance in the reservation.
        /// </summary>
        public static string AmiLaunchIndex
        {
            get { return FetchData("/ami-launch-index"); }
        }

        /// <summary>
        /// The manifest path of the AMI with which the instance was launched.
        /// </summary>
        public static string AmiManifestPath
        {
            get { return FetchData("/ami-manifest-path"); }
        }

        /// <summary>
        /// The AMI IDs of any instances that were rebundled to create this AMI. 
        /// Will only exist if the AMI manifest file contained an ancestor-amis key.
        /// </summary>
        public static IEnumerable<string> AncestorAmiIds
        {
            get { return GetItems("/ancestor-ami-ids"); }
        }

        /// <summary>
        /// The private hostname of the instance.
        /// In cases where multiple network interfaces are present,
        /// this refers to the eth0 device (the device for which the device number is 0).
        /// </summary>
        public static string Hostname
        {
            get { return FetchData("/hostname"); }
        }

        /// <summary>
        /// Notifies the instance that it should reboot in preparation for bundling. 
        /// Valid values: none | shutdown | bundle-pending.
        /// </summary>
        public static string InstanceAction
        {
            get { return FetchData("/instance-action"); }
        }

        /// <summary>
        /// The ID of this instance.
        /// </summary>
        public static string InstanceId
        {
            get { return FetchData("/instance-id"); }
        }

        /// <summary>
        /// The type of instance. 
        /// </summary>
        public static string InstanceType
        {
            get { return FetchData("/instance-type"); }
        }

        /// <summary>
        /// The ID of the kernel launched with this instance, if applicable.
        /// </summary>
        public static string KernelId
        {
            get { return GetData("kernel-id"); }
        }

        /// <summary>
        /// The local hostname of the instance. In cases where multiple network interfaces are present, 
        /// this refers to the eth0 device (the device for which device-number is 0).
        /// </summary>
        public static string LocalHostname
        {
            get { return FetchData("/local-hostname"); }
        }

        /// <summary>
        /// The instance's MAC address. In cases where multiple network interfaces are present, 
        /// this refers to the eth0 device (the device for which device-number is 0).
        /// </summary>
        public static string MacAddress
        {
            get { return FetchData("/mac"); }
        }

        /// <summary>
        ///  The private IP address of the instance. In cases where multiple network interfaces are present, 
        ///  this refers to the eth0 device (the device for which device-number is 0).
        /// </summary>
        public static string PrivateIpAddress
        {
            get { return FetchData("/local-ipv4"); }
        }

        /// <summary>
        /// The Availability Zone in which the instance launched.
        /// </summary>
        public static string AvailabilityZone
        {
            get { return FetchData("/placement/availability-zone"); }
        }

        /// <summary>
        /// Product codes associated with the instance, if any. 
        /// </summary>
        public static IEnumerable<string> ProductCodes
        {
            get { return GetItems("/product-codes"); }
        }

        /// <summary>
        /// Public key. Only available if supplied at instance launch time.
        /// </summary>
        public static string PublicKey
        {
            get { return FetchData("/public-keys/0/openssh-key"); }
        }

        /// <summary>
        /// The ID of the RAM disk specified at launch time, if applicable.
        /// </summary>
        public static string RamdiskId
        {
            get { return FetchData("/ramdisk-id"); }
        }
                
        /// <summary>
        /// The region in which the instance is running, extracted from the identity
        /// document data.
        /// </summary>
        public static RegionEndpoint Region
        {
            get
            {
                var identityDocument = IdentityDocument;
                if (!string.IsNullOrEmpty(identityDocument))
                {
                    try
                    {
                        var jsonDocument = JsonMapper.ToObject(identityDocument.ToString());
                        var regionName = jsonDocument["region"];
                        if (regionName != null)
                            return RegionEndpoint.GetBySystemName(regionName.ToString());
                    }
                    catch (Exception e)
                    {
                        var logger = Logger.GetLogger(typeof(EC2InstanceMetadata));
                        logger.Error(e, "Error attempting to read region from instance metadata identity document");
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// ID of the reservation.
        /// </summary>
        public static string ReservationId
        {
            get { return FetchData("/reservation-id"); }
        }

        /// <summary>
        /// The names of the security groups applied to the instance. 
        /// </summary>
        public static IEnumerable<string> SecurityGroups
        {
            get { return GetItems("/security-groups"); }
        }

        /// <summary>
        /// Returns information about the last time the instance profile was updated, 
        /// including the instance's LastUpdated date, InstanceProfileArn, and InstanceProfileId.
        /// </summary>
        public static IAMInstanceProfileMetadata IAMInstanceProfileInfo
        {
            get
            {
                var json = GetData("/iam/info");
                if (null == json)
                    return null;
                IAMInstanceProfileMetadata info;
                try
                {
                    info = JsonMapper.ToObject<IAMInstanceProfileMetadata>(json);
                }
                catch
                {
                    info = new IAMInstanceProfileMetadata
                    {
                        Code = "Failed",
                        Message = "Could not parse response from metadata service."
                    };
                }
                return info;
            }
        }

        /// <summary>
        /// Returns the temporary security credentials (AccessKeyId, SecretAccessKey, SessionToken, and Expiration) 
        /// associated with the IAM roles on the instance.
        /// </summary>
        public static IDictionary<string, IAMSecurityCredentialMetadata> IAMSecurityCredentials
        {
            get
            {
                var list = GetItems("/iam/security-credentials");
                if (list == null)
                    return null;

                var creds = new Dictionary<string, IAMSecurityCredentialMetadata>();
                foreach (var item in list)
                {
                    var json = GetData("/iam/security-credentials/" + item);
                    try
                    {
                        var cred = JsonMapper.ToObject<IAMSecurityCredentialMetadata>(json);
                        creds[item] = cred;
                    }
                    catch
                    {
                        creds[item] = new IAMSecurityCredentialMetadata
                        {
                            Code = "Failed",
                            Message = "Could not parse response from metadata service."
                        };
                    }
                }

                return creds;
            }
        }

        /// <summary>
        /// The virtual devices associated with the ami, root, ebs, and swap.
        /// </summary>
        public static IDictionary<string, string> BlockDeviceMapping
        {
            get
            {
                var keys = GetItems("/block-device-mapping");
                if (keys == null)
                    return null;

                var mapping = new Dictionary<string, string>();
                foreach (var key in keys)
                {
                    mapping[key] = GetData("/block-device-mapping/" + key);
                }

                return mapping;
            }
        }

        /// <summary>
        /// The network interfaces on the instance.
        /// </summary>
        public static IEnumerable<NetworkInterfaceMetadata> NetworkInterfaces
        {
            get
            {
                var macs = GetItems("/network/interfaces/macs/");
                if (macs == null)
                    return null;

                var interfaces = new List<NetworkInterfaceMetadata>();
                foreach (var mac in macs)
                {
                    interfaces.Add(new NetworkInterfaceMetadata(mac.Trim('/')));
                }
                return interfaces;
            }
        }

        /// <summary>
        /// The metadata sent to the instance.
        /// </summary>
        public static string UserData
        {
            get
            {
                return GetData(EC2_USERDATA_ROOT);
            }
        }

        /// <summary>
        /// Value showing whether the customer has enabled detailed 
        /// one-minute monitoring in CloudWatch.
        /// </summary>
        public static string InstanceMonitoring
        {
            get
            {
                return GetData(EC2_DYNAMICDATA_ROOT + "/fws/instance-monitoring");
            }
        }

        /// <summary>
        /// JSON containing instance attributes, such as instance-id, private IP 
        /// address, etc
        /// </summary>
        public static string IdentityDocument
        {
            get
            {
                return GetData(EC2_DYNAMICDATA_ROOT + "/instance-identity/document");
            }
        }

        /// <summary>
        /// Data that can be used by other parties to verify its origin and authenticity.
        /// </summary>
        public static string IdentitySignature
        {
            get
            {
                return GetData(EC2_DYNAMICDATA_ROOT + "/instance-identity/signature");
            }
        }

        /// <summary>
        /// Used to verify the document's authenticity and content against the signature.
        /// </summary>
        public static string IdentityPkcs7
        {
            get
            {
                return GetData(EC2_DYNAMICDATA_ROOT + "/instance-identity/pkcs7");
            }
        }

        /// <summary>
        /// Return the list of items in the metadata at path.
        /// </summary>
        /// <param name="path">Path at which to query the metadata; may be relative or absolute.</param>
        /// <returns>List of items returned by the metadata service</returns>
        public static IEnumerable<string> GetItems(string path)
        {
            return GetItems(path, DEFAULT_RETRIES, false);
        }

        /// <summary>
        /// Return the metadata at the path
        /// </summary>
        /// <param name="path">Path at which to query the metadata; may be relative or absolute.</param>
        /// <returns>Data returned by the metadata service</returns>
        public static string GetData(string path)
        {
            return GetData(path, DEFAULT_RETRIES);
        }

        /// <summary>
        /// Return the metadata at the path
        /// </summary>
        /// <param name="path">Path at which to query the metadata; may be relative or absolute.</param>
        /// <param name="tries">Number of attempts to make</param>
        /// <returns>Data returned by the metadata service</returns>
        public static string GetData(string path, int tries)
        {
            var items = GetItems(path, tries, true);
            if (items != null && items.Count > 0)
                return items[0];
            return null;
        }

        /// <summary>
        /// Return the list of items in the metadata at path.
        /// </summary>
        /// <param name="path">Path at which to query the metadata; may be relative or absolute.</param>
        /// <param name="tries">Number of attempts to make</param>
        /// <returns>List of items returned by the metadata service</returns>
        public static IEnumerable<string> GetItems(string path, int tries)
        {
            return GetItems(path, tries, false);
        }

        private static string FetchData(string path)
        {
            return FetchData(path, false);
        }

        private static string FetchData(string path, bool force)
        {
            try
            {
                if (force || !_cache.ContainsKey(path))
                    _cache[path] = GetData(path);

                return _cache[path];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches the api token to use with metadata requests.
        /// </summary>        
        /// <returns>The API token or null</returns>
        public static string FetchApiToken()
        {
            return FetchApiToken(DEFAULT_RETRIES);
        }

        /// <summary>
        /// Fetches the api token to use with metadata requests.
        /// </summary>
        /// <param name="tries">The number of tries to fetch the api token before giving up and throwing the web exception</param>
        /// <returns>The API token or null if an API token couldn't be obtained and doesn't need to be used</returns>
        private static string FetchApiToken(int tries)
        {
            for (int retry = 1; retry <= tries; retry++)
            {
                if (!IsIMDSEnabled || useNullToken)
                {
                    return null;
                }

                try
                {
                    var uriForToken = new Uri(EC2_APITOKEN_URL);

                    var headers = new Dictionary<string, string>();
                    headers.Add(HeaderKeys.XAwsEc2MetadataTokenTtlSeconds, DEFAULT_APITOKEN_TTL.ToString(CultureInfo.InvariantCulture));
                    var content = AWSSDKUtils.ExecuteHttpRequest(uriForToken, "PUT", null, TimeSpan.FromSeconds(5), Proxy, headers);
                    return content.Trim();
                }
                catch (Exception e)
                {
                    HttpStatusCode? httpStatusCode = ExceptionUtils.DetermineHttpStatusCode(e);

                    if (httpStatusCode == HttpStatusCode.NotFound 
                        || httpStatusCode == HttpStatusCode.MethodNotAllowed
                        || httpStatusCode == HttpStatusCode.Forbidden)
                    {
                        useNullToken = true;
                        return null;
                    }

                    if (retry >= tries)
                    {
                        if (httpStatusCode == HttpStatusCode.BadRequest)
                        {
                            Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "Unable to contact EC2 Metadata service to obtain a metadata token.");
                            throw;
                        }

                        Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "Unable to contact EC2 Metadata service to obtain a metadata token. Attempting to access IMDS without a token.");

                        //If there isn't a status code, it was a failure to contact the server which would be
                        //a request failure, a network issue, or a timeout. Cache this response and fallback
                        //to IMDS flow without a token. If the non token IMDS flow returns unauthorized, the 
                        //useNullToken flag will be cleared and the IMDS flow will attempt to obtain another 
                        //token.
                        if (httpStatusCode == null)
                        {
                            useNullToken = true;
                        }

                        //Return null to fallback to the IMDS flow without using a token.                    
                        return null;
                    }

                    PauseExponentially(retry - 1);
                }                
            }

            return null;
        }

        public static void ClearTokenFlag()
        {            
            useNullToken = false;
        }

        private static List<string> GetItems(string relativeOrAbsolutePath, int tries, bool slurp)
        {
            return GetItems(relativeOrAbsolutePath, tries, slurp, null);
        }

        private static List<string> GetItems(string relativeOrAbsolutePath, int tries, bool slurp, string token)
        {
            var items = new List<string>();
            //For all meta-data queries we need to fetch an api token to use. In the event a 
            //token cannot be obtained we will fallback to not using a token.
            Dictionary<string, string> headers = null;
            if(token == null)
            {
                token = FetchApiToken(DEFAULT_RETRIES);                
            }

            if (!string.IsNullOrEmpty(token))
            {
                headers = new Dictionary<string, string>();
                headers.Add(HeaderKeys.XAwsEc2MetadataToken, token);
            }

            try
            {
                if (!IsIMDSEnabled)
                {
                    throw new IMDSDisabledException();
                }

                // if we are given a relative path, we assume the data we need exists under the
                // main metadata root
                var uri = relativeOrAbsolutePath.StartsWith(EC2_METADATA_SVC, StringComparison.Ordinal)
                            ? new Uri(relativeOrAbsolutePath)
                            : new Uri(EC2_METADATA_ROOT + relativeOrAbsolutePath);
                
                var content = AWSSDKUtils.ExecuteHttpRequest(uri, "GET", null, TimeSpan.FromSeconds(5), Proxy, headers);
                using (var stream = new StringReader(content))
                {
                    if (slurp)
                        items.Add(stream.ReadToEnd());
                    else
                    {
                        string line;
                        do
                        {
                            line = stream.ReadLine();
                            if (line != null)
                                items.Add(line.Trim());
                        }
                        while (line != null);
                    }
                }
            }            
            catch (IMDSDisabledException)
            {
                // Keep this behavior identical to when HttpStatusCode.NotFound is returned.
                return null;
            }
            catch (Exception e)
            {
                HttpStatusCode? httpStatusCode = ExceptionUtils.DetermineHttpStatusCode(e);

                if (httpStatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else if (httpStatusCode == HttpStatusCode.Unauthorized)
                {
                    ClearTokenFlag();
                    Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "EC2 Metadata service returned unauthorized for token based secure data flow.");
                    throw;
                }

                if (tries <= 1)
                {
                    Logger.GetLogger(typeof(EC2InstanceMetadata)).Error(e, "Unable to contact EC2 Metadata service.");
                    return null;
                }

                PauseExponentially(DEFAULT_RETRIES - tries);
                return GetItems(relativeOrAbsolutePath, tries - 1, slurp, token);
            }

            return items;
        }
                
        /// <summary>
        /// Exponentially sleeps based on the current retry value. A lower 
        /// value will sleep shorter than a larger value
        /// </summary>
        /// <param name="retry">Base 0 retry index</param>
        private static void PauseExponentially(int retry)
        {
            var pause = (int)(Math.Pow(2, retry) * MIN_PAUSE_MS);
            Thread.Sleep(pause < MIN_PAUSE_MS ? MIN_PAUSE_MS : pause);
        }
#if !NETSTANDARD
        [Serializable]
#endif
        private class IMDSDisabledException : InvalidOperationException { };
    }

    /// <summary>
    /// Returns information about the last time the instance profile was updated, 
    /// including the instance's LastUpdated date, InstanceProfileArn, and InstanceProfileId.
    /// </summary>
    public class IAMInstanceProfileMetadata
    {
        /// <summary>
        /// The status of the instance profile
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Further information about the status of the instance profile
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The date and time the instance profile was updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The Amazon Resource Name (ARN) of the instance profile
        /// </summary>
        public string InstanceProfileArn { get; set; }

        /// <summary>
        /// The Id of the instance profile
        /// </summary>
        public string InstanceProfileId { get; set; }
    }

    /// <summary>
    /// The temporary security credentials (AccessKeyId, SecretAccessKey, SessionToken, and Expiration) associated with the IAM role.
    /// </summary>
    public class IAMSecurityCredentialMetadata
    {
        /// <summary>
        /// The status of the security credential
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Further information about the status of the instance profile
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The date and time the security credential was last updated
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// The type of the security credential
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The uniqe id of the security credential
        /// </summary>
        public string AccessKeyId { get; set; }

        /// <summary>
        /// The secret key used to sign requests
        /// </summary>
        public string SecretAccessKey { get; set; }
        
        /// <summary>
        /// The security token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The date and time when these credentials expire
        /// </summary>
        public DateTime Expiration { get; set; }
    }

    /// <summary>
    /// All of the metadata associated with a network interface on the instance.
    /// </summary>
    public class NetworkInterfaceMetadata
    {
        private  string _path;
        private string _mac;

        private IEnumerable<string> _availableKeys;
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        private NetworkInterfaceMetadata() { }

        /// <summary>
        /// Construct an instance of NetworkInterface
        /// </summary>
        /// <param name="macAddress"></param>
        public NetworkInterfaceMetadata(string macAddress)
        {
            _mac = macAddress;
            _path = string.Format(CultureInfo.InvariantCulture, "/network/interfaces/macs/{0}/", _mac);
        }

        /// <summary>
        /// The interface's Media Access Control (mac) address. 
        /// </summary>
        public string MacAddress
        {
            get { return _mac; }
        }

        /// <summary>
        /// The ID of the owner of the network interface. 
        /// </summary>
        /// <remarks>
        /// In multiple-interface environments, an interface can be attached by a third party, such as Elastic Load Balancing. 
        /// Traffic on an interface is always billed to the interface owner.
        /// </remarks>
        public string OwnerId
        {
            get { return GetData("owner-id"); }
        }

        /// <summary>
        /// The interface's profile
        /// </summary>
        public string Profile
        {
            get { return GetData("profile"); }
        }

        /// <summary>
        /// The interface's local hostname.
        /// </summary>
        public string LocalHostname
        {
            get { return GetData("local-hostname"); }
        }

        /// <summary>
        /// The private IP addresses associated with the interface.
        /// </summary>
        public IEnumerable<string> LocalIPv4s
        {
            get { return GetItems("local-ipv4s"); }
        }

        /// <summary>
        /// The interface's public hostname. 
        /// </summary>
        public string PublicHostname
        {
            get { return GetData("public-hostname"); }
        }

        /// <summary>
        /// The elastic IP addresses associated with the interface. 
        /// </summary>
        /// <remarks>
        /// There may be multiple IP addresses on an instance. 
        /// </remarks>
        public IEnumerable<string> PublicIPv4s
        {
            get { return GetItems("public-ipv4s"); }
        }

        /// <summary>
        /// Security groups to which the network interface belongs.
        /// </summary>
        public IEnumerable<string> SecurityGroups
        {
            get { return GetItems("security-groups"); }
        }

        /// <summary>
        /// IDs of the security groups to which the network interface belongs. Returned only for Amazon EC2 instances launched into a VPC.
        /// </summary>
        public IEnumerable<string> SecurityGroupIds
        {
            get { return GetItems("security-group-ids"); }
        }

        /// <summary>
        /// The ID of the Amazon EC2-VPC subnet in which the interface resides.
        /// </summary>
        /// <remarks>
        /// Returned only for Amazon EC2 instances launched into a VPC. 
        /// </remarks>
        public string SubnetId
        {
            get { return GetData("subnet-id"); }
        }

        /// <summary>
        /// The CIDR block of the Amazon EC2-VPC subnet in which the interface resides.  
        /// </summary>
        /// <remarks>
        /// Returned only for Amazon EC2 instances launched into a VPC.
        /// </remarks>
        public string SubnetIPv4CidrBlock
        {
            get { return GetData("subnet-ipv4-cidr-block"); }
        }

        /// <summary>
        /// The CIDR block of the Amazon EC2-VPC subnet in which the interface resides.
        /// </summary>
        /// <remarks>
        /// Returned only for Amazon EC2 instances launched into a VPC.
        /// </remarks>
        public string VpcId
        {
            get { return GetData("vpc-id"); }
        }

        /// <summary>
        /// Get the private IPv4 address(es) that are associated with the public-ip address and assigned to that interface.
        /// </summary>
        /// <param name="publicIp">The public IP address</param>
        /// <returns>Private IPv4 address(es) associated with the public IP address</returns>
        public IEnumerable<string> GetIpV4Association(string publicIp)
        {
            return EC2InstanceMetadata.GetItems(string.Format(CultureInfo.InvariantCulture, "{0}ipv4-associations/{1}", _path, publicIp));
        }

        private string GetData(string key)
        {
            if (_data.ContainsKey(key))
                return _data[key];

            // Since the keys are variable, cache a list of which ones are available
            // to prevent unnecessary trips to the service.
            if (null == _availableKeys)
                _availableKeys = EC2InstanceMetadata.GetItems(_path);

            if (_availableKeys.Contains(key))
            {
                _data[key] = EC2InstanceMetadata.GetData(_path + key);
                return _data[key];
            }
            else 
                return null;
        }

        private IEnumerable<string> GetItems(string key)
        {
            if (null == _availableKeys)
                _availableKeys = EC2InstanceMetadata.GetItems(_path);

            if (_availableKeys.Contains(key))
            {
                return EC2InstanceMetadata.GetItems(_path + key);
            }
            else
                return new List<string>();
        }
    }

}
