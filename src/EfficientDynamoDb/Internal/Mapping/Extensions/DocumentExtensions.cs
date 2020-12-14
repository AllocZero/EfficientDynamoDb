using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Mapping.Extensions
{
    public static class DocumentExtensions
    {
        public static T ToObject<T>(this Document document)
        {
            var classInfo = DdbClassInfoCache.GetOrAdd(typeof(T));

            var entity = classInfo.Constructor();

            foreach (var pair in document)
            {
                if (!classInfo.PropertiesMap.TryGetValue(pair.Key, out var propertyInfo))
                    continue;

                propertyInfo.SetValue(entity, pair.Value);
            }

            return (T) entity;
        }
    }
}