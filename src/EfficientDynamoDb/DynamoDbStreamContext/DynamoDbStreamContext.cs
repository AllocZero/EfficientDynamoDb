using EfficientDynamoDb.Internal;
using EfficientDynamoDb.Internal.Constants;

namespace EfficientDynamoDb
{
    public interface IDynamoDbStreamsContext
    {
        IDynamoDbStreamsLowLevelContext LowLevel { get; }
    }
    
    public class DynamoDbStreamsContext : IDynamoDbStreamsContext
    {
        private readonly DynamoDbStreamsLowLevelContext _lowLevel;
        
        public DynamoDbStreamsContext(DynamoDbContextConfig config)
        {
            _lowLevel = new DynamoDbStreamsLowLevelContext(config, new HttpApi(config, ServiceNames.DynamoDbStreams));
        }
        
        public IDynamoDbStreamsLowLevelContext LowLevel => _lowLevel;
    }
}