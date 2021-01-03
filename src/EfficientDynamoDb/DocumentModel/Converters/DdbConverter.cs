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
using EfficientDynamoDb.Internal.Reader.DocumentDdbReader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public abstract class DdbConverter
    {
        internal abstract DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata dynamoDbContextMetadata);
        
        internal abstract DdbClassType ClassType { get; }

        public abstract bool CanConvert(Type typeToConvert);
        
        public abstract Type? ElementType { get; }
        
        internal bool UseDirectRead { get; set; }
        
        internal bool IsInternal { get; set; }
    }
    
    public abstract class DdbConverter<T> : DdbConverter
    {
        internal override DdbClassType ClassType => DdbClassType.Value;

        public override Type? ElementType => null;

        protected DdbConverter() : this(false)
        {
        }

        protected DdbConverter(bool isInternal)
        {
            UseDirectRead = ClassType == DdbClassType.Value;
            IsInternal = isInternal;
        }
        
        public abstract T Read(in AttributeValue attributeValue);

        public abstract AttributeValue Write(ref T value);

        /// <summary>
        /// Low-level read implementation for direct JSON to Entity conversion.
        /// </summary>
        public virtual T Read(ref DdbReader reader)
        {
            if (!DocumentDdbReader.TryReadValue(ref reader, out var attributeValue))
                throw new DdbException("Couldn't fully read attribute value. Should never happen and is an indication of incorrect read ahead.");

            return Read(in attributeValue);
        }

        internal virtual bool TryRead(ref DdbReader reader, out T value)
        {
            value = Read(ref reader);
            return true;
        }

        /// <summary>
        /// Writes value together with attribute name and attribute type. Only called when value is a part of class property.
        /// </summary>
        public virtual void Write(Utf8JsonWriter writer, string attributeName, ref T value)
        {
            var attributeValue = Write(ref value);
            if (attributeValue.IsNull)
                return;
            
            writer.WritePropertyName(attributeName);
            attributeValue.Write(writer);
        }

        /// <summary>
        /// Writes value together with attribute type. Only called when value is a part of dynamodb list or dictionary value.
        /// </summary>
        public virtual void Write(Utf8JsonWriter writer, ref T value)
        {
            var attributeValue = Write(ref value);
            attributeValue.Write(writer);
        }

        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName, DynamoDbContextMetadata metadata) => new DdbPropertyInfo<T>(propertyInfo, attributeName, this, metadata);
        
        public override bool CanConvert(Type typeToConvert) => typeToConvert == typeof(T);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool SingleValueReadWithReadAhead(bool canUseDirectRead, ref DdbReader reader)
        {
            // If converter uses direct read and underlying json value is object or array - we have to seek forward and make sure that object or array is fully available
            // Otherwise custom converters can try to read half of the object
            // All internal converters should support state management and should never use this functionality for performance reasons
            var readAhead = (reader.State.ReadAhead && canUseDirectRead);
            if (!readAhead)
                return reader.JsonReaderValue.Read();

            return DoSingleValueReadWithReadAhead(ref reader);
        }

        private static bool DoSingleValueReadWithReadAhead(ref DdbReader reader)
        {
            // When we're reading ahead we always have to save the state as we don't know if the next token
            // is an opening object or an array brace.
            var initialReaderState = reader.JsonReaderValue.CurrentState;
            var initialReaderBytesConsumed = reader.JsonReaderValue.BytesConsumed;

            if (!reader.JsonReaderValue.Read())
                return false;

            if (reader.JsonReaderValue.TokenType != JsonTokenType.StartObject && reader.JsonReaderValue.TokenType != JsonTokenType.StartArray)
                return true;

            // Perform the actual read-ahead.
            // Attempt to skip to make sure we have all the data we need.
            var complete = reader.JsonReaderValue.TrySkip();

            // We need to restore the state in all cases as we need to be positioned back before
            // the current token to either attempt to skip again or to actually read the value.

            var originalSpan = new ReadOnlySpan<byte>(reader.State.Buffer, reader.State.BufferStart, reader.State.BufferLength);
            var offset = checked((int) initialReaderBytesConsumed);
            reader = new DdbReader(originalSpan.Slice(offset),
                reader.JsonReaderValue.IsFinalBlock,
                ref initialReaderState, ref reader.State);

            reader.State.BufferStart += offset;
            reader.State.BufferLength -= offset;

            Debug.Assert(reader.JsonReaderValue.BytesConsumed == 0);
            reader.State.BytesConsumed += initialReaderBytesConsumed;

            if (!complete)
            {
                // Couldn't read to the end of the object, exit out to get more data in the buffer.
                return false;
            }

            // Success, requeue the reader to the start token.
            reader.JsonReaderValue.ReadWithVerify();

            return true;
        }
    }
}