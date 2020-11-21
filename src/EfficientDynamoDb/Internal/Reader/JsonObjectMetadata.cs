using System.Collections.Generic;

namespace EfficientDynamoDb.Internal.Reader
{
    public class JsonObjectMetadata
    {
        public readonly IReadOnlyDictionary<string, JsonObjectMetadata>? Fields;

        public readonly bool IsDdbSyntax;

        public readonly bool ReturnDocuments;


        public JsonObjectMetadata(IReadOnlyDictionary<string, JsonObjectMetadata>? fields)
        {
            Fields = fields;
        }
        
        public JsonObjectMetadata(bool isDdbSyntax, bool returnDocuments)
        {
            IsDdbSyntax = isDdbSyntax;
            ReturnDocuments = returnDocuments;
        }
        
        public JsonObjectMetadata(IReadOnlyDictionary<string, JsonObjectMetadata>? fields, bool isDdbSyntax, bool returnDocuments)
        {
            Fields = fields;
            IsDdbSyntax = isDdbSyntax;
            ReturnDocuments = returnDocuments;
        }
    }
}