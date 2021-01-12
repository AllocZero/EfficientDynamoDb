using System;
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
        private static bool TryReadList(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadList(ref reader, out var value))
                return false;
            
            frame.AttributesBuffer.Add(new AttributeValue(new ListAttributeValue(value)));
            return true;
        }
        
        private static bool TryReadList(ref DdbReader reader, out AttributeValue[] value)
        {
            var success = false;
            reader.State.PushDocument();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    while (true)
                    {
                        // Start object or end array
                        reader.JsonReaderValue.ReadWithVerify();

                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                            break;

                        // Attribute type
                        reader.JsonReaderValue.ReadWithVerify();
                        current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);

                        // Attribute value
                        reader.JsonReaderValue.ReadWithVerify();

                        TryReadValue(ref reader, ref current);

                        // End object
                        reader.JsonReaderValue.ReadWithVerify();
                    }

                    value = CreateListFromBuffer(ref current.AttributesBuffer);

                    return success = true;
                }
                else
                {
                    Unsafe.SkipInit(out value);

                    if (current.PropertyState != DdbStackFramePropertyState.None)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueType)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);
                            current.PropertyState = DdbStackFramePropertyState.ReadValueType;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            if (!TryReadValue(ref reader, ref current))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }

                        // End object
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

                    while (true)
                    {
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.ReadValueStart;

                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                            break;

                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);
                        current.PropertyState = DdbStackFramePropertyState.ReadValueType;

                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.ReadValue;

                        if (!TryReadValue(ref reader, ref current))
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.TryRead;

                        // End object
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

                    value = CreateListFromBuffer(ref current.AttributesBuffer);

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AttributeValue[] CreateListFromBuffer(ref ReusableBuffer<AttributeValue> buffer)
        {
            return buffer.RentedBuffer.AsSpan(0, buffer.Index).ToArray();
        }
    }
}