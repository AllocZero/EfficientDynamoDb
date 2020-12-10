using System.Runtime.CompilerServices;
using EfficientDynamoDb.Context.Config;
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
            builder.Append(metadata.Timestamp.ToIso8601BasicDate());
            builder.Append('/');
            builder.Append(metadata.RegionEndpoint.Region);
            builder.Append('/');
            builder.Append(RegionEndpoint.ServiceName);
            builder.Append('/');
            builder.Append(SigningConstants.AwsSignTerminator);
        }
    }
}