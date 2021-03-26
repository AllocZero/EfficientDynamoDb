using EfficientDynamoDb.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Operations.DescribeTable.Models
{
    public class StreamSpecification
    {
        public bool StreamEnabled { get; }
        
        public StreamViewType StreamViewType { get; }

        public StreamSpecification(bool streamEnabled, StreamViewType streamViewType)
        {
            StreamEnabled = streamEnabled;
            StreamViewType = streamViewType;
        }
    }
}