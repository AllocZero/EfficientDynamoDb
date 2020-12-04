using System.Runtime.CompilerServices;
using EfficientDynamoDb.DocumentModel;
using EfficientDynamoDb.DocumentModel.AttributeValues;

namespace EfficientDynamoDb.Internal.Reader
{
    internal static partial class DdbJsonReader
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void HandleEndArray(ref DdbReadStack state)
        {
            ref var initialCurrent = ref state.GetCurrent();

            state.PopArray();
            ref var current = ref state.GetCurrent();

            switch (current.AttributeType)
            {
                case AttributeType.List:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new ListAttributeValue(DdbReadStackFrame.CreateListFromBuffer(ref initialCurrent.AttributesBuffer))));
                    break;
                }
                case AttributeType.StringSet:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new StringSetAttributeValue(DdbReadStackFrame.CreateStringSetFromBuffer(ref initialCurrent.StringBuffer))));
                    break;
                }
                case AttributeType.NumberSet:
                {
                    ref var prevState = ref state.GetPrevious();
                    prevState.StringBuffer.Add(prevState.KeyName!);
                    prevState.AttributesBuffer.Add(new AttributeValue(new NumberSetAttributeValue(DdbReadStackFrame.CreateNumberArrayFromBuffer(ref initialCurrent.StringBuffer))));
                    break;
                }
                default:
                {
                    current.StringBuffer.Add(current.KeyName!);

                    current.AttributesBuffer.Add(current.NextMetadata?.ReturnDocuments == true
                        ? new AttributeValue(new DocumentListAttributeValue(DdbReadStackFrame.CreateDocumentListFromBuffer(ref initialCurrent.AttributesBuffer)))
                        : new AttributeValue(new ListAttributeValue(DdbReadStackFrame.CreateListFromBuffer(ref initialCurrent.AttributesBuffer))));

                    break;
                }
            }
        }
    }
}