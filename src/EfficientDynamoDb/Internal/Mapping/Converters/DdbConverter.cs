using System.Reflection;
using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Mapping.Converters
{
    public abstract class DdbConverter
    {
        internal abstract DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName);
    }
    
    public abstract class DdbConverter<T> : DdbConverter
    {
        public abstract T Read(AttributeValue attributeValue);
        
        internal sealed override DdbPropertyInfo CreateDdbPropertyInfo(PropertyInfo propertyInfo, string attributeName) => new DdbPropertyInfo<T>(propertyInfo, attributeName, this);
    }
}