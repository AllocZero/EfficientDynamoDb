using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ThirdParty.Json.LitJson;

namespace Amazon.DynamoDBv2.DataModel
{
    /// <summary>
    /// S3Link is an object that provides a connection to an S3 resource
    /// that can be stored in a DynamoDB field through DynamoDBContext
    /// </summary>
    public partial class S3Link
    {
        #region Statics

        internal static Dictionary<DynamoDBContext, S3ClientCache> Caches = new Dictionary<DynamoDBContext, S3ClientCache>();
        private static Object cacheLock = new Object();

        ///// <summary>
        ///// Allows the use of a specific config in the creation of the client for a context
        ///// </summary>
        ///// <param name="context">The context the client should be used in</param>
        ///// <param name="config">The config object for the client</param>
        //public static void UseConfigForClient(DynamoDBContext context, AmazonS3Config config)
        //{
        //    var castedClient = ((AmazonDynamoDBClient)context.Client);
        //    var client = new AmazonS3Client(castedClient.GetCredentials(), config);
        //    S3ClientCache cache;
        //    if (!S3Link.Caches.TryGetValue(context, out cache))
        //    {                
        //        cache = new S3ClientCache(castedClient.GetCredentials(),castedClient.CloneConfig<AmazonS3Config>());
        //        S3Link.Caches.Add(context, cache);
        //    }            
        //    cache.UseClient(client, config.RegionEndpoint);
        //}

        /// <summary>
        /// Creates an S3Link that can be used to managed an S3 connection
        /// </summary>
        /// <param name="context">The context that is handling the S3Link</param>
        /// <param name="bucket">The bucket the S3Link should manage</param>
        /// <param name="key">The key that S3Link should store and download from</param>
        /// <param name="region">The region of the S3 resource</param>
        /// <returns>A new S3Link object that can upload and download to the target bucket</returns>
        public static S3Link Create(DynamoDBContext context, string bucket, string key, Amazon.RegionEndpoint region)
        {
            S3ClientCache cacheFromKey;
            if (S3Link.Caches.TryGetValue(context, out cacheFromKey))
            {
                return new S3Link(cacheFromKey, bucket, key, region.SystemName);
            }

            S3ClientCache cache = CreatClientCacheFromContext(context);
            return new S3Link(cache, bucket, key, region.SystemName);
        }

        #endregion

        #region Properties

        private S3ClientCache s3ClientCache;
        private LinkInfo linker;

        /// <summary>
        /// The Key that S3Link stores and downloads a resource to and from
        /// </summary>
        public string Key 
        { 
            get 
            { 
                return this.linker.s3.key; 
            } 
            set 
            { 
                this.linker.s3.key = value; 
            } 
        }

        /// <summary>
        /// The name of the target Bucket for the managed resource
        /// </summary>
        public string BucketName 
        { 
            get 
            { 
                return this.linker.s3.bucket; 
            } 
            set 
            { 
                this.linker.s3.bucket = value; 
            } 
        }

        /// <summary>
        /// The region the S3 resource is in
        /// </summary>
        public string Region 
        { 
            get 
            {
                if (String.IsNullOrEmpty(this.linker.s3.region))
                {
                    return "us-east-1";
                }
                return this.linker.s3.region; 
            } 
            set 
            {
                if (String.IsNullOrEmpty(value))
                {
                    this.linker.s3.region = "us-east-1";
                }
                this.linker.s3.region = value; 
            } 
        }

        /// <summary>
        /// Looks up RegionEndpoint based on region as a string
        /// </summary>
        public RegionEndpoint RegionAsEndpoint
        {
            get
            {
                if (linker.s3.region == null)
                {
                    return RegionEndpoint.GetBySystemName("us-east-1");
                }
                return RegionEndpoint.GetBySystemName(linker.s3.region);
            }
        }

        #endregion

        #region Constuctors

        internal S3Link(S3ClientCache clientCache, string bucketName, string key)
            : this(clientCache, new LinkInfo(bucketName, key)) { }

        internal S3Link(S3ClientCache clientCache, string bucketName, string key, string region)
            : this(clientCache, new LinkInfo(bucketName, key, region)) { }

        private S3Link(S3ClientCache clientCache, LinkInfo linker)
        {
            if (linker == null) throw new ArgumentNullException("linker");
            if (clientCache == null) throw new ArgumentNullException("clientCache");

            this.s3ClientCache = clientCache;
            this.linker = linker;
        }

        internal S3Link(S3ClientCache clientCache, string json)
        {
            if (clientCache == null) throw new ArgumentNullException("clientCache");
            if (json == null) throw new ArgumentNullException("json");
            this.s3ClientCache = clientCache;
            linker = JsonMapper.ToObject<LinkInfo>(json);
        }

        internal static RegionEndpoint GetRegionFromJSON(string json)
        {
            var linker = JsonMapper.ToObject<LinkInfo>(json);
            if (linker.s3.region == null)
            {
                return RegionEndpoint.GetBySystemName("us-east-1");
            }
            return RegionEndpoint.GetBySystemName(linker.s3.region);
        }

        #endregion

        #region Methods

        #region Small getters


        internal static S3ClientCache CreatClientCacheFromContext(DynamoDBContext context)
        {
            var client = ((AmazonDynamoDBClient)context.Client);
            var cache = new S3ClientCache(client);

            lock (S3Link.cacheLock)
            {
                S3Link.Caches[context] = cache;
            }

            return cache;
        }

        #endregion

        #region Misc

        /// <summary>
        /// Provides a URL for accessing the S3 object managed by S3Link
        /// </summary>
        /// <param name="expiration">The time the link should become invalid</param>
        /// <returns>A URL directing to the S3 object</returns>
        public string GetPreSignedURL(DateTime expiration)
        {
            return this.s3ClientCache.GetClient(this.RegionAsEndpoint).GeneratePreSignedURL(this.linker.s3.bucket, this.linker.s3.key, expiration, null);
        }

        internal string ToJson()
        {
            return JsonMapper.ToJson(linker);
        }

        #endregion

        #endregion

        #region Helper Classes

        internal class S3LinkConverter : IPropertyConverter
        {
            private DynamoDBContext context;

            public S3LinkConverter(DynamoDBContext context)
            {
                this.context = context;
            }

            public DocumentModel.DynamoDBEntry ToEntry(object value)
            {
                Primitive S3string = ((S3Link)value).ToJson();
                return S3string;
            }

            public object FromEntry(DocumentModel.DynamoDBEntry entry)
            {
                S3ClientCache cache;
                if (!S3Link.Caches.TryGetValue(context, out cache))
                {
                    cache = S3Link.CreatClientCacheFromContext(context);
                }
                return new S3Link(cache, entry.AsString());
            }            
        }

        private class LinkInfo
        {
            public S3 s3 { get; set; }

            // For JSON mapper
            public LinkInfo() { }

            public LinkInfo(string bucketName, string key)
                : this(bucketName, key, null) { }

            public LinkInfo(string bucketName, string key, string region)
            {
                if (bucketName == null) throw new ArgumentNullException("bucketName");
                if (key == null) throw new ArgumentNullException("key");
                if (String.IsNullOrEmpty(region))
                {
                    region = null;
                }
                s3 = new S3(bucketName, key, region);
            }
        }

        private class S3
        {
            public string bucket { get; set; }
            public string key { get; set; }
            public string region { get; set; }

            // For JSON mapper
            public S3() { }

            public S3(string bucketName, string key, string region)
            {
                this.bucket = bucketName;
                this.key = key;
                if (String.IsNullOrEmpty(region))
                {
                    region = null;
                }
                this.region = region;
            }
        }

        #endregion

    }
}
