using System.Text.Json;
using EfficientDynamoDb.Internal.Reader;

namespace EfficientDynamoDb.DocumentModel.Converters
{
    // public abstract class DdbResumableConverter<T> : DdbConverter<T>
    // {
    //     public override T Read(ref Utf8JsonReader reader, AttributeType attributeType)
    //     {
    //         var state = new DdbEntityReadStack(DdbEntityReadStack.DefaultStackLength, );
    //         state.ReadAhead = false;
    //         state.UseFastPath = true;
    //         state.GetCurrent().ClassInfo = 
    //         try
    //         {
    //             TryRead(ref reader, ref state, out var value);
    //
    //             return value;
    //         }
    //         finally
    //         {
    //             state.Dispose();
    //         }
    //     }
    // }
}