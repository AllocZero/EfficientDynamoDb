using System;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class EntityDdbJsonReader
    {
        private static void ReadCore<T>(ref JsonReaderState readerState, bool isFinalBlock, ReadOnlySpan<byte> buffer, ref DdbEntityReadStack readStack) where T : class
        {
            var reader = new Utf8JsonReader(buffer, isFinalBlock, readerState);
            readStack.BytesConsumed = 0;
            ReadCore<T>(ref reader, ref readStack);

            readerState = reader.CurrentState;
        }

        private static void ReadCore<T>(ref Utf8JsonReader reader, ref DdbEntityReadStack state) where T : class
        {
            if(!state.UseFastPath)
                throw new NotImplementedException();

            var classInfo = state.Metadata.GetOrAddClassInfo(typeof(T));
            
            ref var current = ref state.GetCurrent();
            current.ClassInfo = classInfo;

            var converter = (DdbConverter<T>) state.Metadata.GetOrAddConverter(typeof(T), typeof(JsonObjectDdbConverter<T>));

            converter.TryRead(ref reader, ref state, AttributeType.Unknown, out var value);

            current.ReturnValue = value;
            
            state.BytesConsumed += reader.BytesConsumed;
        }
    }
}