using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    public abstract class DdbResumableConverter<T> : DdbConverter<T>
    {
        public sealed override T Read(ref DdbReader reader)
        {
            var newReader = new DdbReader(ref reader.JsonReaderValue, ref reader.State)
            {
                State = {ReadAhead = false, UseFastPath = true}
            };

            newReader.State.GetCurrent().NextClassInfo = newReader.State.Metadata.GetOrAddClassInfo(typeof(T));
            TryRead(ref newReader, out var value);

            reader.JsonReaderValue = newReader.JsonReaderValue;

            return value;
        }
    }
}