using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Converters;

[TestFixture]
public class DateOnlyDdbConverterShould
{
    [TestCaseSource(nameof(RoundTripIso8601TestCases))]
    public void CorrectlyParseRoundTripIso8601DateTime(string str, DateOnly expected)
    {
        var converter = new DateOnlyDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        var result = converter.Read(in attributeValue);
        result.Year.ShouldBe(expected.Year);
        result.Month.ShouldBe(expected.Month);
        result.Day.ShouldBe(expected.Day);
    }
    
    [TestCaseSource(nameof(NonRoundTripIso8601TestCases))]
    public void FailToParseNonRoundTripIso8601DateTime(string str)
    {
        var converter = new DateOnlyDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        Should.Throw<FormatException>(() => converter.Read(in attributeValue));
    }

    private static TestCaseData<string, DateOnly>[] RoundTripIso8601TestCases =>
    [
        new("2025-01-01", new(2025, 01, 01)) { TestName = "Simple date" },
    ];

    private static TestCaseData<string>[] NonRoundTripIso8601TestCases =>
    [
        new("2025") { TestName = "Only year" },
        new("2025-01") { TestName = "Only year and month" },
    ];
}