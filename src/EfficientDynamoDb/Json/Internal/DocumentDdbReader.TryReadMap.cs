using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Json
{
    internal static partial class DocumentJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool TryReadMap(ref DdbReader reader, ref DdbEntityReadStackFrame frame)
        {
            if (!TryReadMap(ref reader, out var document))
                return false;
            
            frame.AttributesBuffer.Add(document);
            return true;
        }
        
        public static bool TryReadMap(ref DdbReader reader, out Document value)
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
                        // Property name
                        reader.JsonReaderValue.ReadWithVerify();

                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                            break;

                        current.StringBuffer.Add(GetCachedString(ref reader.JsonReaderValue, ref reader.State));
                        
                        // Attribute value
                        reader.JsonReaderValue.ReadWithVerify();

                        TryReadValue(ref reader, ref current);
                    }

                    value = CreateDocumentFromBuffer(ref current)!;

                    return success = true;
                }
                else
                {
                    Unsafe.SkipInit(out value);
                    
                    if (current.PropertyState != DdbStackFramePropertyState.None)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.Name)
                        {
                            current.StringBuffer.Add(GetCachedString(ref reader.JsonReaderValue, ref reader.State));
                            current.PropertyState = DdbStackFramePropertyState.Name;
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
                        }

                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

                    while (true)
                    {
                        // Property name
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        if (reader.JsonReaderValue.TokenType == JsonTokenType.EndObject)
                            break;

                        current.StringBuffer.Add(GetCachedString(ref reader.JsonReaderValue, ref reader.State));
                        current.PropertyState = DdbStackFramePropertyState.Name;

                        if (!reader.JsonReaderValue.Read())
                            return success = false;

                        current.PropertyState = DdbStackFramePropertyState.ReadValue;

                        if (!TryReadValue(ref reader, ref current))
                            return success = false;
                        
                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

                    value = CreateDocumentFromBuffer(ref current);
                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Document CreateDocumentFromBuffer(ref DdbEntityReadStackFrame frame)
        {
            var document = new Document(frame.StringBuffer.Index);
            
            for (var i = 0; i < frame.StringBuffer.Index; i++)
                document.Add(frame.StringBuffer.RentedBuffer![i], frame.AttributesBuffer.RentedBuffer![i]);

            return document;
        }
    }
}