using EfficientDynamoDb.Context.Operations.DescribeTable.Models.Enums;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models
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