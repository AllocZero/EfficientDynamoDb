using EfficientDynamoDb.Context;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public interface ISetValueConverter<T>
    {
        /// <summary>
        /// Writes string value. Only called when value is a part of a set.
        /// </summary>
        string WriteStringValue(ref T value);

        /// <summary>
        /// Writes string value without attribute type. Only called when value is a part of a set.
        /// </summary>
        void WriteStringValue(in DdbWriter ddbWriter, ref T value)
        {
            ddbWriter.JsonWriter.WriteStringValue(WriteStringValue(ref value));
        }
    }
}