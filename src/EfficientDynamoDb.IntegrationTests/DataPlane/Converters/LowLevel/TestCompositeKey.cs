using System.Text;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;

namespace EfficientDynamoDb.IntegrationTests.DataPlane.Converters.LowLevel;

public class TestCompositeKey
{
    public string Part1 { get; set; } = null!;
    public string Part2 { get; set; } = null!;
    public string Part3 { get; set; } = null!;
}

public class TestCompositeKeyDdbConverter : DdbConverter<TestCompositeKey>
{
    public override TestCompositeKey Read(in AttributeValue attributeValue)
    {
        var list = attributeValue.AsListAttribute();
        return new()
        {
            Part1 = list.Items[0].AsString(),
            Part2 = list.Items[1].AsString(),
            Part3 = list.Items[2].AsString()
        };
    }

    public override AttributeValue Write(ref TestCompositeKey value)
    {
        return new ListAttributeValue([
            new StringAttributeValue(value.Part1), new StringAttributeValue(value.Part2), new StringAttributeValue(value.Part3)
        ]);
    }

    public override TestCompositeKey Read(ref DdbReader reader)
    {
        ref var jsonReader = ref reader.JsonReaderValue;
        jsonReader.Read();
        jsonReader.Read();
        jsonReader.Read();
        var part1 = Encoding.UTF8.GetString(jsonReader.ValueSpan);
        jsonReader.Read();
        
        jsonReader.Read();
        jsonReader.Read();
        jsonReader.Read();
        var part2 = Encoding.UTF8.GetString(jsonReader.ValueSpan);
        jsonReader.Read();
        
        jsonReader.Read();
        jsonReader.Read();
        jsonReader.Read();
        var part3 = Encoding.UTF8.GetString(jsonReader.ValueSpan);
        jsonReader.Read();
        
        // Last end array
        jsonReader.Read();
        
        return new()
        {
            Part1 = part1,
            Part2 = part2,
            Part3 = part3
        };
    }
}