using System.Text.Json;
using EfficientDynamoDb.DocumentModel.Exceptions;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public interface ISetValueConverter<T>
    {
        /// <summary>
        /// Writes raw value without attribute type. Only called when value is a part of set.
        /// </summary>
        void WriteStringValue(Utf8JsonWriter writer, ref T value);
    }
}