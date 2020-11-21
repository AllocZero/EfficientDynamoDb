using System.Collections.Generic;

namespace EfficientDynamoDb.Internal.Reader
{
    public class JsonObjectMetadata
    {
        public readonly IReadOnlyDictionary<string, JsonObjectMetadata>? Fields;

        public readonly int IsDdbSyntax;

        public readonly bool ReturnDocuments;


        public JsonObjectMetadata(IReadOnlyDictionary<string, JsonObjectMetadata>? fields)
        {
            Fields = fields;
        }
        
        public JsonObjectMetadata(bool isDdbSyntax, bool returnDocuments)
        {
            IsDdbSyntax = isDdbSyntax ? 1 : 0;
            ReturnDocuments = returnDocuments;
        }
        
        public JsonObjectMetadata(IReadOnlyDictionary<string, JsonObjectMetadata>? fields, bool isDdbSyntax, bool returnDocuments)
        {
            Fields = fields;
            IsDdbSyntax = isDdbSyntax ? 1 : 0;
            ReturnDocuments = returnDocuments;
        }
    }
}