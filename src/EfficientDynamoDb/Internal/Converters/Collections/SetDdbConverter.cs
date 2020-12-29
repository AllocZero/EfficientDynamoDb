using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Context;
using EfficientDynamoDb.DocumentModel.Converters;
using EfficientDynamoDb.DocumentModel.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal abstract class SetDdbConverter<T> : DdbResumableConverter<HashSet<T>>
    {
        private static readonly Type ElementTypeValue = typeof(T);
        
        internal sealed override DdbClassType ClassType => DdbClassType.Enumerable;

        public sealed override Type? ElementType => ElementTypeValue;

        protected readonly DdbConverter<T> ElementConverter;

        protected readonly ISetValueConverter<T> ElementSetValueConverter;
        
        protected SetDdbConverter(DynamoDbContextMetadata metadata)
        {
            ElementConverter = metadata.GetOrAddConverter<T>();
            ElementSetValueConverter = ElementConverter as ISetValueConverter<T> ??
                                       throw new DdbException($"{ElementConverter.GetType().Name} must implement ISetValueConverter in order to store value as a part of dynamodb set.");
        }

        internal sealed override bool TryRead(ref DdbReader reader, out HashSet<T> value)
        {
            var success = false;
            reader.State.Push();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    value = new HashSet<T>();

                    reader.JsonReaderValue.ReadWithVerify();

                    while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                    {
                        value.Add(ElementConverter.Read(ref reader));

                        reader.JsonReaderValue.ReadWithVerify();
                    }

                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = value = new HashSet<T>();
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        value = (HashSet<T>) current.ReturnValue!;
                    }

                    while (true)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(ElementConverter.UseDirectRead, ref reader))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                break;
                        }

                        value.Add(ElementConverter.Read(ref reader));

                        current.PropertyState = DdbStackFramePropertyState.None;
                    }

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