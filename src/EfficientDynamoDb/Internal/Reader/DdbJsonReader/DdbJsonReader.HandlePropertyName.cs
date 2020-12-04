using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandlePropertyName(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            ref var current = ref state.GetCurrent();
            if (state.ContainsDdbAttributeType())
            {
                current.AttributeType = GetDdbAttributeType(ref reader);
            }
            else
            {
                current.KeyName = GetCachedString(ref reader, ref state);

                if (current.Metadata?.Fields == null)
                    return;

                if (current.Metadata.Fields.TryGetValue(current.KeyName!, out var metadata))
                {
                    current.NextMetadata = metadata;
                    state.IsDdbSyntax = metadata.IsDdbSyntax;
                }
                else
                {
                    current.NextMetadata = null;
                    state.IsDdbSyntax = 0;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AttributeType GetDdbAttributeType(ref Utf8JsonReader reader)
        {
            var key = reader.ValueSpan.Length > 1 ? MemoryMarshal.Read<short>(reader.ValueSpan) : reader.ValueSpan[0];

            return AttributeTypesMap.Get(key);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetCachedString(ref Utf8JsonReader reader, ref DdbReadStack state)
        {
            return !state.KeysCache.IsInitialized || !state.KeysCache.TryGetOrAdd(ref reader, out var value)
                ? reader.GetString()!
                : value!;
        }
    }
}