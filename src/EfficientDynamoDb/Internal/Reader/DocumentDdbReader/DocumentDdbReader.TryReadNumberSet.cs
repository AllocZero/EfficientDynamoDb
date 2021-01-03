using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.AttributeValues;
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
        
        private static bool TryReadNumberSet(ref DdbReader reader, out string[] value)
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
                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                break;
                        }

                        current.StringBuffer.Add(reader.JsonReaderValue.GetString()!);

                        current.PropertyState = DdbStackFramePropertyState.None;
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
        public static string[] CreateNumberArrayFromBuffer(ref ReusableBuffer<string> buffer)
        {
            return buffer.RentedBuffer.AsSpan(0, buffer.Index).ToArray();
        }
    }
}