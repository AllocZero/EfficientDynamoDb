using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Json
{
  internal abstract class JsonCollectionDdbConverter<TCollection, TInitialCollection, TElement> : DdbResumableConverter<TCollection> where TInitialCollection : new()
    {
        private static readonly Type ElementTypeValue = typeof(TElement);
        
        protected readonly DdbConverter<TElement> ElementConverter;

        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        protected JsonCollectionDdbConverter(DdbConverter<TElement> elementConverter)
        {
            ElementConverter = elementConverter;
        }

        protected abstract void Add(TInitialCollection collection, TElement item, int index);

        protected abstract TCollection ToResult(TInitialCollection collection);
        
        

        internal override bool TryRead(ref DdbReader reader, out TCollection value)
        {
            if (reader.AttributeType == AttributeType.Null)
            {
                value = default!;
                return true;
            }
            
            var success = false;
            reader.State.Push();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    var i = 0;
                    var collection = new TInitialCollection();

                    reader.JsonReaderValue.ReadWithVerify();

                    if (ElementConverter.UseDirectRead)
                    {
                        while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                        {
                            Add(collection, ElementConverter.Read(ref reader), i++);

                            reader.JsonReaderValue.ReadWithVerify();
                        }
                    }
                    else
                    {
                        while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                        {
                            ElementConverter.TryRead(ref reader, out var element);
                            Add(collection, element, i++);

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

                    if (ElementConverter.UseDirectRead)
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!SingleValueReadWithReadAhead(ElementConverter.CanSeek, ref reader))
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;

                                if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                    break;
                            }

                            Add(collection, ElementConverter.Read(ref reader), current.CollectionIndex++);

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
                    }
                    else
                    {
                        while (true)
                        {
                            if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                            {
                                if (!reader.JsonReaderValue.Read())
                                    return success = false;

                                current.PropertyState = DdbStackFramePropertyState.ReadValue;

                                if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                    break;
                            }

                            if (!ElementConverter.TryRead(ref reader, out var element))
                                return success = false;

                            Add(collection, element, current.CollectionIndex++);

                            current.PropertyState = DdbStackFramePropertyState.None;
                        }
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