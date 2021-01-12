using System.Text.Json;
using EfficientDynamoDb.Context;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public interface IDictionaryKeyConverter<T>
    {
        /// <summary>
        /// Writes raw value without attribute type. Only called when value is a part of dictionary key.
        /// </summary>
        void WritePropertyName(in DdbWriter ddbWriter, ref T value);
    }
}