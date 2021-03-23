using EfficientDynamoDb.Context;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public interface IDictionaryKeyConverter<T>
    {
        /// <summary>
        /// Gets string value. Only called when value is a part of a dictionary key.
        /// </summary>
        string WriteStringValue(ref T value);
        
        /// <summary>
        /// Writes string value without attribute type as a JSON property name. Only called when value is a part of a dictionary key.
        /// </summary>
        void WritePropertyName(in DdbWriter ddbWriter, ref T value)
        {
            ddbWriter.JsonWriter.WriteStringValue(WriteStringValue(ref value));
        }
    }
}