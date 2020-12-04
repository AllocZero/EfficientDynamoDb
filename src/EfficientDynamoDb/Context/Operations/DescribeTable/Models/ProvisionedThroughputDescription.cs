using System;

namespace EfficientDynamoDb.Context.Operations.DescribeTable.Models
{
    public class ProvisionedThroughputDescription
    {
        public DateTime? LastDecreaseDateTime { get; }
        
        public DateTime? LastIncreaseDateTime { get; } 
        
        public int NumberOfDecreasesToday { get; }
        
        public int ReadCapacityUnits { get; }
        
        public int WriteCapacityUnits { get; }

        public ProvisionedThroughputDescription(DateTime? lastDecreaseDateTime, DateTime? lastIncreaseDateTime, int numberOfDecreasesToday, int readCapacityUnits, int writeCapacityUnits)
        {
            LastDecreaseDateTime = lastDecreaseDateTime;
            LastIncreaseDateTime = lastIncreaseDateTime;
            NumberOfDecreasesToday = numberOfDecreasesToday;
            ReadCapacityUnits = readCapacityUnits;
            WriteCapacityUnits = writeCapacityUnits;
        }
    }
}