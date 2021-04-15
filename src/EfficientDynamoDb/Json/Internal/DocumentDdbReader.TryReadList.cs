using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Core;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Json
{
    internal static partial class DocumentJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadList(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadList(ref reader, out var value))
                return false;
            
            frame.AttributesBuffer.Add(new AttributeValue(new ListAttributeValue(value)));
            return true;
        }
        
        private static bool TryReadList(ref DdbReader reader, out List<AttributeValue> value)
        {
            var success = false;
            reader.State.PushDocument();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                // ReSharper disable once AssignmentInConditionalExpression
                if (success = TryReadBuffer(ref reader, ref current))
                    value = CreateListFromBuffer(ref current.AttributesBuffer);
                else
                    Unsafe.SkipInit(out value);

                return success;
            }
            finally
            {
                reader.State.Pop(success);
            }
        }

        public static bool TryReadBuffer(ref DdbReader reader, ref DdbEntityReadStackFrame current)
        {
            if (reader.State.UseFastPath)
            {
                while (true)
                {
                    // Value or end array
                    reader.JsonReaderValue.ReadWithVerify();

                    if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                        break;

                    TryReadValue(ref reader, ref current);
                }

                return true;
            }
            else
            {

                if (current.PropertyState != DdbStackFramePropertyState.None)
                {
                    if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                    {
                        if (!reader.JsonReaderValue.Read())
                            return false;

                        current.PropertyState = DdbStackFramePropertyState.ReadValue;
                    }

                    if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                    {
                        if (!TryReadValue(ref reader, ref current))
                            return false;
                    }

                    current.PropertyState = DdbStackFramePropertyState.None;
                }

                while (true)
                {
                    if (!reader.JsonReaderValue.Read())
                        return false;

                    current.PropertyState = DdbStackFramePropertyState.ReadValue;

                    if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                        break;

                    if (!TryReadValue(ref reader, ref current))
                        return false;

                    current.PropertyState = DdbStackFramePropertyState.None;
                }


                return true;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static List<AttributeValue> CreateListFromBuffer(ref ReusableBuffer<AttributeValue> buffer)
        {
            var list = new List<AttributeValue>(buffer.Index);

            for (var i = 0; i < buffer.Index; i++)
                list.Add(buffer.RentedBuffer![i]);

            return list;
        }
    }
}