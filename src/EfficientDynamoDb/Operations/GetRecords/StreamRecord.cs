using System;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations
{
    /// <summary>
    /// A description of a single data modification that was performed on an item in a DynamoDB table.
    /// </summary>
    public class StreamRecord
    {
        /// <summary>
        /// The approximate date and time when the stream record was created, rounded to the nearest second.
        /// </summary>
        public DateTime ApproximateCreationDateTime { get; set; }

        /// <summary>
        /// The primary key attribute(s) for the DynamoDB item that was modified.
        /// </summary>
        public Document Keys { get; set; } = null!;

        /// <summary>
        /// The item in the DynamoDB table as it appeared after it was modified.
        /// This property is present only if <see cref="StreamViewType"/> is set to <see cref="DescribeTable.Models.Enums.StreamViewType.NewImage"/> or <see cref="DescribeTable.Models.Enums.StreamViewType.NewAndOldImages"/>
        /// </summary>
        public Document? NewImage { get; set; }

        /// <summary>
        /// The item in the DynamoDB table as it appeared before it was modified.
        /// This property is present only if <see cref="StreamViewType"/> is set to <see cref="DescribeTable.Models.Enums.StreamViewType.OldImage"/> or <see cref="DescribeTable.Models.Enums.StreamViewType.NewAndOldImages"/>
        /// </summary>
        public Document? OldImage { get; set; }

        /// <summary>
        /// The sequence number of the stream record.
        /// </summary>
        public string SequenceNumber { get; set; } = "";

        /// <summary>
        /// The size of the stream record, in bytes.
        /// </summary>
        public int SizeBytes { get; set; }

        /// <summary>
        /// The type of data from the modified DynamoDB item that was captured in this stream record.
        /// </summary>
        public StreamViewType StreamViewType { get; set; }
    }
}