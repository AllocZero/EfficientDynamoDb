namespace EfficientDynamoDb.Internal.Reader.Metadata
{
    internal class JsonObjectMetadata
    {
        public readonly IFieldsMetadata? Fields;

        public readonly int IsDdbSyntax;

        public readonly bool ReturnDocuments;

        public readonly bool IsArray;


        public JsonObjectMetadata(IFieldsMetadata? fields)
        {
            Fields = fields;
        }
        
        public JsonObjectMetadata(bool isDdbSyntax, bool returnDocuments)
        {
            IsDdbSyntax = isDdbSyntax ? 1 : 0;
            ReturnDocuments = returnDocuments;
        }

        public JsonObjectMetadata(int isDdbSyntax, bool returnDocuments, bool isArray)
        {
            IsDdbSyntax = isDdbSyntax;
            ReturnDocuments = returnDocuments;
            IsArray = isArray;
        }

        public JsonObjectMetadata(IFieldsMetadata? fields, bool isDdbSyntax, bool returnDocuments)
        {
            Fields = fields;
            IsDdbSyntax = isDdbSyntax ? 1 : 0;
            ReturnDocuments = returnDocuments;
        }

        public JsonObjectMetadata(IFieldsMetadata? fields, bool isDdbSyntax, bool returnDocuments, bool isArray)
        {
            Fields = fields;
            IsDdbSyntax =  isDdbSyntax ? 1 : 0;
            ReturnDocuments = returnDocuments;
            IsArray = isArray;
        }
    }
}