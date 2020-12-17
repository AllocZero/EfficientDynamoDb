using EfficientDynamoDb.Internal.Metadata;

namespace EfficientDynamoDb.DocumentModel.Extensions
{
    public static class DocumentExtensions
    {
        public static T ToObject<T>(this Document document) where T : class
        {
            var classInfo = DdbClassInfoCache.GetOrAdd(typeof(T));

            var entity = classInfo.Constructor();

            foreach (var pair in document)
            {
                if (!classInfo.PropertiesMap.TryGetValue(pair.Key, out var propertyInfo) || pair.Value.IsNull)
                    continue;

                propertyInfo.SetValue(entity, pair.Value);
            }

            return (T) entity;
        }
        
        public static Document ToDocument<T>(this T entity) where T : class
        {
            var classInfo = DdbClassInfoCache.GetOrAdd(typeof(T));
            
            var document = new Document(classInfo.Properties.Length);
            foreach (var property in classInfo.Properties)
                property.SetDocumentValue(entity, document);

            return document;
        }
    }
}