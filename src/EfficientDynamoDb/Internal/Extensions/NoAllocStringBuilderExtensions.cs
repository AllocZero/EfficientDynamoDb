using System.Runtime.CompilerServices;
using EfficientDynamoDb.Configs;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Signing;
using EfficientDynamoDb.Internal.Signing.Constants;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class NoAllocStringBuilderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AppendCredentialScope(this ref NoAllocStringBuilder builder, in SigningMetadata metadata)
        {
            builder.Append(metadata.TimestampIso8601BasicDateString);
            builder.Append('/');
            builder.Append(metadata.RegionEndpoint.Region);
            builder.Append('/');
            builder.Append(RegionEndpoint.ServiceName);
            builder.Append('/');
            builder.Append(SigningConstants.AwsSignTerminator);
        }
    }
}