using System;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.TypeParsers;
using EfficientDynamoDb.Operations;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    internal static class RecordsParser
    {
        internal static Record[] ParseRecords(Document response)
        {
            if (!response.TryGetValue("Records", out var recordsAttribute))
                return Array.Empty<Record>();

            var records = recordsAttribute.AsListAttribute().Items;
            var result = new Record[records.Count];
            for (var i = 0; i < records.Count; i++)
            {
                result[i] = ParseRecord(records[i].AsDocument());
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Record ParseRecord(Document record) => new Record
        {
            AwsRegion = record["awsRegion"].AsString(),
            DynamoDb = ParseStreamRecord(record["dynamodb"].AsDocument()),
            EventId = record["eventID"].AsString(),
            EventName = EnumParser.ParseUpperSnakeCase<EventName>(record["eventName"].AsString()),
            EventSource = record["eventSource"].AsString(),
            EventVersion = record["eventVersion"].AsString(),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static StreamRecord ParseStreamRecord(Document streamRecord)
        {
            var result = new StreamRecord
            {
                ApproximateCreationDateTime = streamRecord["ApproximateCreationDateTime"].AsNumberAttribute().ToDouble().FromUnixSeconds(),
                Keys = streamRecord["Keys"].AsDocument(),
                SequenceNumber = streamRecord["SequenceNumber"].AsString(),
                SizeBytes = streamRecord["SizeBytes"].AsNumberAttribute().ToInt(),
                StreamViewType = EnumParser.ParseUpperSnakeCase<StreamViewType>(streamRecord["StreamViewType"].AsString())
            };
            if (result.StreamViewType == StreamViewType.NewImage || result.StreamViewType == StreamViewType.NewAndOldImages)
                result.NewImage = streamRecord["NewImage"].AsDocument();
            if (result.StreamViewType == StreamViewType.OldImage || result.StreamViewType == StreamViewType.NewAndOldImages)
                result.OldImage = streamRecord["OldImage"].AsDocument();

            return result;
        }
    }
}