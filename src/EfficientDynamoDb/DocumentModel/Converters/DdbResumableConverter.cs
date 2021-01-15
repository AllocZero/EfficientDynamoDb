using EfficientDynamoDb.Context;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    internal abstract class DdbResumableConverter<T> : DdbConverter<T>
    {
        protected DdbResumableConverter() : base(true)
        {
        }

        /// <summary>
        /// A bridge from direct read (custom public converters) to resumable read (internal convreters that use state)
        /// Only used when custom converter calls internal resumable converter
        /// A bridge is required because internal resumable converters don't have direct <c>Read</c> implementation and we need to do some preparations before calling <c>TryRead</c>.
        /// </summary>
        public sealed override T Read(ref DdbReader reader)
        {
            // At this point current json value is guaranteed to fit in the buffer
            // Disable ReadAhead to not do unnecessary TrySkip calls
            // Enable UseFastPath to improve performance
            var oldReadAhead = reader.State.ReadAhead;
            var oldUseFastPath = reader.State.UseFastPath;

            reader.State.ReadAhead = false;
            reader.State.UseFastPath = true;
            
            ref var current = ref reader.State.GetCurrent();
            current.NextClassInfo = reader.State.Metadata.GetOrAddClassInfo(typeof(T));
            
            TryRead(ref reader, out var value);
            
            // Revert state changes
            reader.State.ReadAhead = oldReadAhead;
            reader.State.UseFastPath = oldUseFastPath;
            current.NextClassInfo = null;

            return value;
        }
    }
}