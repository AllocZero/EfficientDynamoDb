using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder
{
    public abstract class GetItemHttpContentBase : DynamoDbHttpContent
    {
        protected readonly string TableName;

        protected GetItemHttpContentBase(string tableName) : base("DynamoDB_20120810.GetItem") => TableName = tableName;
    }
}