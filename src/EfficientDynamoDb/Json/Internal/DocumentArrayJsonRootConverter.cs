using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters;
using EfficientDynamoDb.Internal.Core;

namespace EfficientDynamoDb.Json
{
    public class DocumentArrayJsonRootConverter : DdbConverter<IReadOnlyList<Document>>, IRootDdbConverter<IReadOnlyList<Document>>
    {
        public DocumentArrayJsonRootConverter() : base(true)
        {
        }

        public bool TryReadRoot(ref DdbReader reader, out IReadOnlyList<Document> value)
        {
            var success = false;
            reader.State.PushDocument();

            try
            {
                ref var current = ref reader.State.GetCurrent();

                if (DocumentJsonReader.TryReadBuffer(ref reader, ref current))
                {
                    value = CreateDocumentsArray(current.AttributesBuffer);
                    return success = true;
                }
            
                Unsafe.SkipInit(out value);
                return success = false;
            }
            finally
            {
                reader.State.Pop(success);
            }
        }

        public override IReadOnlyList<Document> Read(in AttributeValue attributeValue) => throw new NotSupportedException();

        public override AttributeValue Write(ref IReadOnlyList<Document> value) => throw new NotSupportedException();

        private static Document[] CreateDocumentsArray(ReusableBuffer<AttributeValue> buffer)
        {
            var array = new Document[buffer.Index];

            for (var i = 0; i < buffer.Index; i++)
                array[i] = buffer.RentedBuffer![i]._mapValue.Value;

            return array;
        }
    }
}