using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Converters;

[TestFixture]
public class TimeOnlyDdbConverterShould
{
    [TestCaseSource(nameof(RoundTripIso8601TestCases))]
    public void CorrectlyParseRoundTripIso8601DateTime(string str, TimeOnly expected)
    {
        var converter = new TimeOnlyDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        var result = converter.Read(in attributeValue);
        result.Hour.ShouldBe(expected.Hour);
        result.Minute.ShouldBe(expected.Minute);
        result.Second.ShouldBe(expected.Second);
    }
    
    [TestCaseSource(nameof(NonRoundTripIso8601TestCases))]
    public void FailToParseNonRoundTripIso8601DateTime(string str)
    {
        var converter = new TimeOnlyDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        Should.Throw<FormatException>(() => converter.Read(in attributeValue));
    }

    private static TestCaseData<string, TimeOnly>[] RoundTripIso8601TestCases =>
    [
        new("13:00:00.0000000", new(13, 0, 0)) { TestName = "Simple time" },
        new("13:00:00.1200000", new(13, 0, 0, 120)) { TestName = "Time with milliseconds" },
    ];

    private static TestCaseData<string>[] NonRoundTripIso8601TestCases =>
    [
        new("13") { TestName = "Only hour" },
        new("13:00") { TestName = "Only hour and minute" },
        new("13:00:00") { TestName = "No fractions" },
    ];
}