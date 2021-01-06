using System.Text.Json;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public interface IDictionaryKeyConverter<T>
    {
        /// <summary>
        /// Writes raw value without attribute type. Only called when value is a part of dictionary key.
        /// </summary>
        void WritePropertyName(Utf8JsonWriter writer, ref T value);
    }
}