using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;

namespace EfficientDynamoDb.Internal.Reader.DocumentDdbReader
{
    internal static partial class DocumentDdbReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadStringSet(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadStringSet(ref reader, out var value))
                return false;

            frame.AttributesBuffer.Add(new AttributeValue(new StringSetAttributeValue(value)));
            return true;
        }
        
        private static bool TryReadStringSet(ref DdbReader reader, out HashSet<string> value)
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
                        current.StringBuffer.Add(reader.JsonReaderValue.GetString()!);

                        reader.JsonReaderValue.ReadWithVerify();
                    }

                    value = CreateStringSetFromBuffer(ref current.StringBuffer);

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

                        current.StringBuffer.Add(reader.JsonReaderValue.GetString()!);
                    }

                    value = CreateStringSetFromBuffer(ref current.StringBuffer);

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<string> CreateStringSetFromBuffer(ref ReusableBuffer<string> buffer)
        {
            if (buffer.Index == 0)
                return new HashSet<string>();
            
            var set = new HashSet<string>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                set.Add(buffer.RentedBuffer![i]);

            return set;
        }
    }
}