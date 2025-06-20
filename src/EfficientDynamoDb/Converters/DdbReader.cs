using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Converters
{
    public ref struct DdbReader
    {
        internal Utf8JsonReader JsonReaderValue;
        
        public Utf8JsonReader JsonReader
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => JsonReaderValue;
        }

        internal DdbEntityReadStack State;

        public AttributeType AttributeType => State.GetCurrent().AttributeType;

        internal DdbReader(scoped in ReadOnlySpan<byte> buffer, bool isFinalBlock, scoped ref JsonReaderState readerState, scoped ref DdbEntityReadStack readStack)
        {
            JsonReaderValue = new Utf8JsonReader(buffer, isFinalBlock, readerState);
            State = readStack;
        }

        internal DdbReader(ref Utf8JsonReader jsonReader, ref DdbEntityReadStack state)
        {
            JsonReaderValue = jsonReader;
            State = state;
        }
    }
}