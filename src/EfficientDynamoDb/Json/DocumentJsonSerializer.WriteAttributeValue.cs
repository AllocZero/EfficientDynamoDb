using System;
using System.Text;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Json
{
    public static partial class DocumentJsonSerializer
    {
        private static void WriteAttributeValue(PooledByteBufferWriter buffer, Utf8JsonWriter writer, in AttributeValue attributeValue)
        {
            switch (attributeValue.Type)
            {
                case AttributeType.String:
                    writer.WriteStringValue(attributeValue.AsString());
                    break;
                case AttributeType.Number:
                    // Write null, then delete
                    writer.WriteNullValue();
                    writer.Flush();
                    buffer.Advance(-NullUtf8BytesCount);

                    var numberValue = attributeValue.AsNumberAttribute().Value;
                    var bytesSize = Encoding.UTF8.GetByteCount(numberValue);

                    var bytesWritten = Encoding.UTF8.GetBytes(numberValue, buffer.GetSpan(bytesSize));
                    buffer.Advance(bytesWritten);
                    break;
                case AttributeType.Bool:
                    writer.WriteBooleanValue(attributeValue.AsBool());
                    break;
                case AttributeType.Map:
                    writer.WriteStartObject();

                    foreach (var (key, value) in attributeValue.AsMapAttribute().Value)
                    {
                        writer.WritePropertyName(key);
                        WriteAttributeValue(buffer, writer, value);
                    }

                    writer.WriteEndObject();
                    break;
                case AttributeType.List:
                    writer.WriteStartArray();
                    foreach (var item in attributeValue.AsListAttribute().Items)
                        WriteAttributeValue(buffer, writer, in item);
                    writer.WriteEndArray();
                    break;
                case AttributeType.StringSet:
                    writer.WriteStartArray();
                    foreach (var item in attributeValue.AsStringSetAttribute().Items)
                        writer.WriteStringValue(item);
                    writer.WriteEndArray();
                    break;
                case AttributeType.NumberSet:
                    writer.WriteStartArray();

                    if (writer.BytesPending > 0)
                        writer.Flush();

                    var isFirst = true;
                    foreach (var item in attributeValue.AsNumberSetAttribute().Items)
                    {
                        if (!isFirst)
                        {
                            buffer.GetSpan(1)[0] = CommaUtf8Byte;
                            buffer.Advance(1);
                        }

                        var byteCount = Encoding.UTF8.GetByteCount(item);
                        buffer.Advance(Encoding.UTF8.GetBytes(item, buffer.GetSpan(byteCount)));
                        isFirst = false;
                    }

                    writer.WriteEndArray();
                    break;
                case AttributeType.Null:
                    writer.WriteNullValue();
                    break;
                case AttributeType.Binary:
                    writer.WriteBase64StringValue(attributeValue.AsBinaryAttribute().Value);
                    break;
                case AttributeType.BinarySet:
                    writer.WriteStartArray();
                    foreach (var item in attributeValue.AsBinarySetAttribute().Items)
                        writer.WriteBase64StringValue(item);
                    writer.WriteEndArray();
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"AttributeType '{attributeValue.Type}' is not supported.");
            }
        }
    }
}