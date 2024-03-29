using System;
using System.Collections.Generic;
using System.Text.Json;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.Extensions;
using EfficientDynamoDb.Internal.Metadata;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.Internal.Converters.Collections
{
    internal abstract class SetDdbConverter<TSet, TElement> : DdbResumableConverter<TSet?> where TSet : class, ISet<TElement>
    {
        private static readonly Type ElementTypeValue = typeof(TElement);
        
        internal sealed override DdbClassType ClassType => DdbClassType.Enumerable;

        internal sealed override Type? ElementType => ElementTypeValue;

        protected readonly DdbConverter<TElement> ElementConverterValue;

        protected readonly ISetValueConverter<TElement> ElementSetValueConverter;

        internal sealed override DdbConverter? ElementConverter => ElementConverterValue;

        protected SetDdbConverter(DynamoDbContextMetadata metadata)
        {
            ElementConverterValue = metadata.GetOrAddConverter<TElement>();
            ElementSetValueConverter = ElementConverterValue as ISetValueConverter<TElement> ??
                                       throw new DdbException($"{ElementConverterValue.GetType().Name} must implement ISetValueConverter in order to store value as a part of dynamodb set.");
        }

        protected abstract TSet CreateSet();

        public sealed override bool ShouldWrite(ref TSet? value) => value?.Count > 0;

        internal sealed override bool TryRead(ref DdbReader reader, out TSet value)
        {
            var success = false;
            reader.State.Push();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (reader.State.UseFastPath)
                {
                    value = CreateSet();

                    reader.JsonReaderValue.ReadWithVerify();

                    while (reader.JsonReaderValue.TokenType != JsonTokenType.EndArray)
                    {
                        reader.State.GetCurrent().AttributeType = AttributeType.String;
                        value.Add(ElementConverterValue.Read(ref reader));

                        reader.JsonReaderValue.ReadWithVerify();
                    }

                    return success = true;
                }
                else
                {
                    if (current.ObjectState < DdbStackFrameObjectState.CreatedObject)
                    {
                        current.ReturnValue = value = CreateSet();
                        current.ObjectState = DdbStackFrameObjectState.CreatedObject;
                    }
                    else
                    {
                        value = (TSet) current.ReturnValue!;
                    }

                    while (true)
                    {
                        if (current.PropertyState < DdbStackFramePropertyState.ReadValue)
                        {
                            if (!SingleValueReadWithReadAhead(ElementConverterValue.CanSeek, ref reader))
                                return success = false;

                            current.PropertyState = DdbStackFramePropertyState.ReadValue;

                            if (reader.JsonReaderValue.TokenType == JsonTokenType.EndArray)
                                break;
                        }

                        reader.State.GetCurrent().AttributeType = AttributeType.String;
                        value.Add(ElementConverterValue.Read(ref reader));

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