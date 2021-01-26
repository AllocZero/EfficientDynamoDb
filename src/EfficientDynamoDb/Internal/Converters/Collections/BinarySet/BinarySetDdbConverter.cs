using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections.BinarySet
{
    internal abstract class BinarySetDdbConverter<TCollection, TInitialCollection> : DdbResumableConverter<TCollection> where TInitialCollection : new()
    {
        private static readonly Type ElementTypeValue = typeof(byte[]);

        internal override DdbClassType ClassType => DdbClassType.Enumerable;

        public override Type? ElementType => ElementTypeValue;

        protected abstract void Add(TInitialCollection collection, byte[] item, int index);

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

                        Add(collection, reader.JsonReaderValue.GetBytesFromBase64(), i++);

                        // End object
                        reader.JsonReaderValue.ReadWithVerify();
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
                            if (!SingleValueReadWithReadAhead(false, ref reader))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;
                        }

                        if (current.PropertyState < DdbStackFramePropertyState.TryRead)
                        {
                            Add(collection, reader.JsonReaderValue.GetBytesFromBase64(), current.CollectionIndex++);

                            current.PropertyState = DdbStackFramePropertyState.TryRead;
                        }

                        // End object
                        if (!reader.JsonReaderValue.Read())
                            return success = false;

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