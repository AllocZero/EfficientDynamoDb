using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using EfficientDynamoDb.Context.Operations.GetItem;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.ReturnDataFlags;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Internal.Builder.GetItemHttpContents
{
    public class GetItemHttpContent<TPkAttribute> : GetItemHttpContentBase where TPkAttribute : IAttributeValue
    {
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue) : base(tableName)
        {
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
        }
        
        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();

            return default;
        }
    }

    public class GetItemHttpContent<TPkAttribute, TSkAttribute> : GetItemHttpContentBase
        where TPkAttribute : IAttributeValue
        where TSkAttribute : IAttributeValue
    {
        private readonly string _pkName;
        private readonly TPkAttribute _pkAttributeValue;
        private readonly string _skName;
        private readonly TSkAttribute _skAttributeValue;

        public GetItemHttpContent(string tableName, string pkName, TPkAttribute pkAttributeValue, string skName, TSkAttribute skAttributeValue) : base(tableName)
        {
            _pkName = pkName;
            _pkAttributeValue = pkAttributeValue;
            _skName = skName;
            _skAttributeValue = skAttributeValue;
        }
        
        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _pkAttributeValue.Write(writer);
            
            writer.WritePropertyName(_skName);
            _skAttributeValue.Write(writer);
            
            writer.WriteEndObject();
            
            writer.WriteString("TableName", TableName);

            writer.WriteEndObject();

            return default;
        }
    }

    public class GetItemHttpContent : GetItemHttpContentBase
    {
        private readonly GetItemRequest _request;
        private readonly string _pkName;
        private readonly string _skName;

        public GetItemHttpContent(GetItemRequest request, string prefixedTableName, string pkName, string skName) : base(prefixedTableName)
        {
            _request = request;
            _pkName = pkName;
            _skName = skName;
        }

        protected override ValueTask WriteDataAsync(Utf8JsonWriter writer, PooledByteBufferWriter bufferWriter)
        {
            writer.WriteStartObject();
            
            WritePrimaryKey(writer);
            
            writer.WriteString("TableName", TableName);

            if (_request.ConsistentRead)
                writer.WriteBoolean("ConsistentRead", true);
            
            if (_request.ExpressionAttributeNames?.Count > 0)
                WriteExpressionAttributeNames(writer);
            
            if (_request.ProjectionExpression?.Count > 0)
                writer.WriteString("ProjectionExpression", string.Join(",", _request.ProjectionExpression));
            
            if (_request.ReturnConsumedCapacity != ReturnConsumedCapacity.None)
                WriteReturnConsumedCapacity(writer);

            writer.WriteEndObject();

            return default;  
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WritePrimaryKey(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("Key");
            writer.WriteStartObject();
            
            writer.WritePropertyName(_pkName);
            _request.Key!.PartitionKeyValue.Write(writer);

            if (_request.Key.SortKeyValue != null)
            {
                writer.WritePropertyName(_skName);
                _request.Key.SortKeyValue.Value.Write(writer);
            }
            
            writer.WriteEndObject();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteExpressionAttributeNames(Utf8JsonWriter writer)
        {
            writer.WritePropertyName("ExpressionAttributeNames");
                
            writer.WriteStartObject();

            foreach (var pair in _request.ExpressionAttributeNames!)
                writer.WriteString(pair.Key, pair.Value);
                
            writer.WriteEndObject();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void WriteReturnConsumedCapacity(Utf8JsonWriter writer)
        {
            writer.WriteString("ReturnConsumedCapacity", _request.ReturnConsumedCapacity switch
            {
                ReturnConsumedCapacity.Indexes => "INDEXES",
                ReturnConsumedCapacity.Total => "TOTAL",
                ReturnConsumedCapacity.None => "NONE",
            });
        }
    }
}