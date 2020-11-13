using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace Benchmarks.AwsDdbSdk.Converters
{
    public class SparseIntConverter : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value) => default(int).Equals(value) ? null : (DynamoDBEntry) (int) value;

        public object FromEntry(DynamoDBEntry entry) => entry.AsInt();
    }
}