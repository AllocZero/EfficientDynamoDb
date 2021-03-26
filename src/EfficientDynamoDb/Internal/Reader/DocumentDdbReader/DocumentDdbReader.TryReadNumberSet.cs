using System;
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
        private static bool TryReadNumberSet(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadNumberSet(ref reader, out var value))
                return false;

            frame.AttributesBuffer.Add(new AttributeValue(new NumberSetAttributeValue(value)));
            return true;
        }
        
        private static bool TryReadNumberSet(ref DdbReader reader, out HashSet<string> value)
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

                    value = CreateNumberArrayFromBuffer(ref current.StringBuffer);

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

                    value = CreateNumberArrayFromBuffer(ref current.StringBuffer);

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static HashSet<string> CreateNumberArrayFromBuffer(ref ReusableBuffer<string> buffer)
        {
            var set = new HashSet<string>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                set.Add(buffer.RentedBuffer![i]);

            return set;
        }
    }
}