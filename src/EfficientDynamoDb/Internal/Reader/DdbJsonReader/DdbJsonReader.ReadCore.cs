using System;
using System.Text.Json;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        private static void ReadCore(ref JsonReaderState readerState, bool isFinalBlock, ReadOnlySpan<byte> buffer, ref DdbReadStack readStack, IParsingOptions options)
        {
            var reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);
            readStack.BytesConsumed = 0;
            ReadCore(ref reader, ref readStack, options);

            readerState = reader.CurrentState;
        }

        private static void ReadCore(ref Utf8JsonReader reader, ref DdbReadStack state, IParsingOptions options)
        {
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                    {
                        HandleStringValue(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.PropertyName:
                    {
                        HandlePropertyName(ref reader, ref state);
                        break;
                    }
                    case JsonTokenType.StartObject:
                    {
                        if (!state.IsLastFrame || state.GetCurrent().IsProcessingValue())
                        {
                            // Parse inner object start
                            HandleNestedStartObject(ref state);
                        }
                        break;
                    }
                    case JsonTokenType.EndObject:
                    {
                        HandleEndObject(ref state);
                        break;
                    }
                    case JsonTokenType.True:
                    {
                        HandleBoolValue(ref state, true);
                        break;
                    }
                    case JsonTokenType.False:
                    {
                        HandleBoolValue(ref state, false);
                        break;
                    }
                    case JsonTokenType.StartArray:
                    {
                        HandleStartArray(ref state);
                        break;
                    }
                    case JsonTokenType.EndArray:
                    {
                        HandleEndArray(ref state);
                        break;
                    }
                    case JsonTokenType.Number:
                    {
                        HandleNumberValue(ref reader, ref state, options);
                        break;
                    }
                    case JsonTokenType.Null:
                    {
                        HandleNullValue(ref state);
                        break;
                    }
                }
            }
            
            state.BytesConsumed += reader.BytesConsumed;
        }
    }
}