using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Attributes;
using EfficientDynamoDb.DocumentModel.AttributeValues;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public abstract class DdbConverter
    {
        internal abstract DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata dynamoDbContextMetadata);
        
        internal abstract DdbClassType ClassType { get; }

        public abstract bool CanConvert(Type typeToConvert);
        
        public abstract Type? ElementType { get; }
        
        internal bool UseDirectRead { get; set; }
    }
    
    public abstract class DdbConverter<T> : DdbConverter
    {
        internal override DdbClassType ClassType => DdbClassType.Value;

        public override Type? ElementType => null;

        protected DdbConverter()
        {
            UseDirectRead = ClassType == DdbClassType.Value;
        }

        public abstract T Read(in AttributeValue attributeValue);

        public abstract AttributeValue Write(ref T value);

        public abstract T Read(ref Utf8JsonReader reader, AttributeType attributeType);

        internal virtual bool TryRead(ref Utf8JsonReader reader, ref DdbEntityReadStack state, out T value)
        {
            value = Read(ref reader, state.GetCurrent().AttributeType);
            return true;
        }

        public virtual void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            var attributeValue = Write(ref value);
            if (attributeValue.IsNull)
                return;
            
            writer.WritePropertyName(attributeName);
            attributeValue.Write(writer);
        }

        public virtual void Write(Utf8JsonWriter writer, ref T value)
        {
            var attributeValue = Write(ref value);
            attributeValue.Write(writer);
        }

        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata metadata) => new DdbPropertyInfo<T>(propertyInfo, attributeName, this, metadata);
        
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool SingleValueReadWithReadAhead(bool canUseDirectRead, ref Utf8JsonReader reader, ref DdbEntityReadStack state)
        {
            // If converter uses direct read and underlying json value is object or array - we have to seek forward and make sure that object or array is fully available
            // Otherwise custom converters can try to read half of the object
            // All internal converters should support state management and should never use this functionality for performance reasons
            var readAhead = (state.ReadAhead && canUseDirectRead);

            if (!reader.Read())
                return false;
            
            var tokenType = reader.TokenType;
            return !readAhead || (tokenType != JsonTokenType.StartObject && tokenType != JsonTokenType.StartArray) || DoSingleValueReadWithReadAhead(ref reader, ref state);
        }

        private static bool DoSingleValueReadWithReadAhead(ref Utf8JsonReader reader, ref DdbEntityReadStack state)
        {
            // When we're reading ahead we always have to save the state as we don't know if the next token
            // is an opening object or an array brace.
            var initialReaderState = reader.CurrentState;
            var initialReaderBytesConsumed = reader.BytesConsumed;

            // Perform the actual read-ahead.
            // Attempt to skip to make sure we have all the data we need.
            var complete = reader.TrySkip();

            // We need to restore the state in all cases as we need to be positioned back before
            // the current token to either attempt to skip again or to actually read the value.

            var originalSpan = new ReadOnlySpan<byte>(state.Buffer, state.BufferStart, state.BufferLength);
            var offset = checked((int) initialReaderBytesConsumed);
            reader = new Utf8JsonReader(originalSpan.Slice(offset),
                isFinalBlock: reader.IsFinalBlock,
                state: initialReaderState);

            state.BufferStart += offset;
            state.BufferLength -= offset;

            Debug.Assert(reader.BytesConsumed == 0);
            state.BytesConsumed += initialReaderBytesConsumed;

            if (!complete)
            {
                // Couldn't read to the end of the object, exit out to get more data in the buffer.
                return false;
            }

            // Success, requeue the reader to the start token.
            reader.ReadWithVerify();

            return true;
        }
    }
}