using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleBoolValue(ref DdbReadStack state, bool value)
        {
            ref var current = ref state.GetCurrent();

            switch (current.AttributeType)
            {
                case AttributeType.Bool:
                {
                    ref var prevState = ref state.GetPrevious();

                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
                case AttributeType.Null:
                {
                    ref var prevState = ref state.GetPrevious();

                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
                default:
                {
                    current.StringBuffer.Add(current.KeyName!);
                    current.AttributesBuffer.Add(new AttributeValue(new BoolAttributeValue(value)));
                    break;
                }
            }
        }
    }
}