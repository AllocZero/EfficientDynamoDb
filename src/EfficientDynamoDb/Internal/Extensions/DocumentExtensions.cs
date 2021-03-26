using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Extensions
{
    internal static class DocumentExtensions
    {
        public static T ToObject<T>(this Document document, DynamoDbContextMetadata metadata) where T : class
        {
            var classInfo = metadata.GetOrAddClassInfo(typeof(T));

            var entity = classInfo.Constructor!();

            foreach (var pair in document)
            {
                if (!classInfo.AttributesMap.TryGetValue(pair.Key, out var propertyInfo) || pair.Value.IsNull)
                    continue;

                propertyInfo.SetValue(entity, pair.Value);
            }

            return (T) entity;
        }
        
        public static Document ToDocument<T>(this T entity, DynamoDbContextMetadata metadata) where T : class
        {
            var classInfo = metadata.GetOrAddClassInfo(typeof(T));
            
            var document = new Document(classInfo.Properties.Length);
            foreach (var property in classInfo.Properties)
                property.SetDocumentValue(entity, document);

            return document;
        }
    }
}