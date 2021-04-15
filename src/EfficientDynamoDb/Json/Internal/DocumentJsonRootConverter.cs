using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.Internal.Converters;

namespace EfficientDynamoDb.Json
{
    internal sealed class DocumentJsonRootConverter : DdbConverter<Document>, IRootDdbConverter<Document>
    {
        public DocumentJsonRootConverter() : base(true)
        {
        }

        public bool TryReadRoot(ref DdbReader reader, out Document value) => DocumentJsonReader.TryReadMap(ref reader, out value);

        public override Document Read(in AttributeValue attributeValue) => throw new NotSupportedException();

        public override AttributeValue Write(ref Document value) => throw new NotSupportedException();
    }
}