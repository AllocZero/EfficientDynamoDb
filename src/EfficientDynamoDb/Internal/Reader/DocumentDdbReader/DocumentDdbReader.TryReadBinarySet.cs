using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Reader.DocumentDdbReader
{
     internal static partial class DocumentDdbReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadBinarySet(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadBinarySet(ref reader, out var value))
                return false;

            frame.AttributesBuffer.Add(new AttributeValue(new BinarySetAttributeValue(value)));
            return true;
        }
        
        private static bool TryReadBinarySet(ref DdbReader reader, out List<byte[]> value)
        {
            var success = false;
            reader.State.PushDocument();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    reader.JsonReaderValue.ReadWithVerify();

                    while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                    {
                        current.AttributesBuffer.Add(new AttributeValue(new BinaryAttributeValue(reader.JsonReaderValue.GetBytesFromBase64())));

                        reader.JsonReaderValue.ReadWithVerify();
                    }

                    value = CreateBinarySetFromBuffer(ref current.AttributesBuffer);

                    return success = true;
                }
                else
                {
                    Unsafe.SkipInit(out value);
                    
                    while (true)
                    {
                        if (!reader.JsonReaderValue.Read())
                            return success = false;
                        
                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                            break;

                        current.AttributesBuffer.Add(new AttributeValue(new BinaryAttributeValue(reader.JsonReaderValue.GetBytesFromBase64())));
                    }

                    value = CreateBinarySetFromBuffer(ref current.AttributesBuffer);

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<byte[]> CreateBinarySetFromBuffer(ref ReusableBuffer<AttributeValue> buffer)
        {
            if (buffer.Index == 0)
                return new List<byte[]>();
            
            var set = new List<byte[]>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                set.Add(buffer.RentedBuffer![i].AsBinaryAttribute().Value);

            return set;
        }
    }
}