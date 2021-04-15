using System;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Converters.Json;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class EntityDdbJsonReader
    {
        private static void ReadCore<T>(ref JsonReaderState readerState, bool isFinalBlock, ReadOnlySpan<byte> buffer, ref DdbEntityReadStack readStack) where T : class
        {
            var ddbReader = new DdbReader(buffer, isFinalBlock, ref readerState, ref readStack)
            {
                State = {ReadAhead = !isFinalBlock, BytesConsumed = 0}
            };
            
            ReadCore<T>(ref ddbReader);

            readerState = ddbReader.JsonReaderValue.CurrentState;
            readStack = ddbReader.State;
        }

        private static void ReadCore<T>(ref DdbReader reader) where T : class
        {
            ref var current = ref reader.State.GetCurrent();
            
            if (current.ObjectState < DdbStackFrameObjectState.StartToken)
            {
                if (!reader.JsonReaderValue.Read())
                    return;

                current.ObjectState = DdbStackFrameObjectState.StartToken;
            }
            
            var converter = (IRootDdbConverter<T>) current.ClassInfo!.ConverterBase;

            if(converter.TryReadRoot(ref reader, out var value))
                current.ReturnValue = value;
            
            reader.State.BytesConsumed += reader.JsonReaderValue.BytesConsumed;
        }
    }
}