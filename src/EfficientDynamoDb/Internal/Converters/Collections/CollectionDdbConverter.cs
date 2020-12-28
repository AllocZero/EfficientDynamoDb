using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal abstract class CollectionDdbConverter<TCollection, TInitialCollection, TElement> : DdbResumableConverter<TCollection> where TInitialCollection : new()
    {
        private static readonly Type ElementTypeValue = typeof(TElement);
        
        private readonly DdbConverter<TElement> _elementConverter;

        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        protected CollectionDdbConverter(DdbConverter<TElement> elementConverter)
        {
            _elementConverter = elementConverter;
        }

        protected abstract void Add(TInitialCollection collection, TElement item, int index);

        protected abstract TCollection ToResult(TInitialCollection collection);

        internal override bool TryRead(ref DdbReader reader, out TCollection value)
        {
            var success = false;
            reader.State.Push();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    var i = 0;
                    var collection = new TInitialCollection();

                    if (_elementConverter.UseDirectRead)
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

                            Add(collection, _elementConverter.Read(ref reader), i++);

                            // End object
                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }
                    else
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

                            _elementConverter.TryRead(ref reader, out var item);
                            Add(collection, item, i++);

                            // End object
                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }

                    value = ToResult(collection);

                    return success = true;
                }
                else
                {
                    TInitialCollection collection;
                    Unsafe.SkipInit(out value);

                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = collection = new TInitialCollection();
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        collection = (TInitialCollection) current.ReturnValue!;
                    }

                    while (true)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueStart)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValueStart;

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                break;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValueType)
                        {
                            if (!reader.JsonReaderValue.Read())
                                return success = false;

                            current.AttributeType = DdbJsonReader.GetDdbAttributeType(ref reader.JsonReaderValue);
                            current.PropertyState = DdbStackFramePropertyState.ReadValueType;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(true, ref reader))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (_elementConverter.UseDirectRead)
                        {
                            Add(collection, _elementConverter.Read(ref reader), current.CollectionIndex++);
                        }
                        else
                        {
                            if (!_elementConverter.TryRead(ref reader, out var item))
                                return success = false;

                            Add(collection, item, current.CollectionIndex++);
                        }

                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

                    value = ToResult(collection);

                    return success = true;
                }
            }
            finally
            {
                reader.State.Pop(success);
            }
        }
    }
}