using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Converters;

[TestFixture]
public class DateTimeOffsetDdbConverterShould
{
    [TestCaseSource(nameof(RoundTripIso8601TestCases))]
    public void CorrectlyParseRoundTripIso8601DateTime(string str, DateTimeOffset expected)
    {
        var converter = new DateTimeOffsetDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        var result = converter.Read(in attributeValue);
        result.ShouldBe(expected);
    }
    
    [TestCaseSource(nameof(NonRoundTripIso8601TestCases))]
    public void FailToParseNonRoundTripIso8601DateTime(string str)
    {
        var converter = new DateTimeOffsetDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        Should.Throw<FormatException>(() => converter.Read(in attributeValue));
    }

    private static TestCaseData<string, DateTimeOffset>[] RoundTripIso8601TestCases =>
    [
        new("2025-01-01T13:00:00.0000000Z", new DateTimeOffset(2025, 01, 01, 13, 0, 0, TimeSpan.Zero)) { TestName = "UTC with 0 fractional seconds" },
        new("2025-01-01T13:00:00.1200000Z", new DateTimeOffset(2025, 01, 01, 13, 0, 0, 120, TimeSpan.Zero)) { TestName = "UTC with milliseconds" },
        new("2025-01-01T13:00:00.0000000+03:00", new DateTimeOffset(2025, 01, 01, 13, 0, 0, TimeSpan.FromHours(3))) { TestName = "With timezone offset" },
    ];

    private static TestCaseData<string>[] NonRoundTripIso8601TestCases =>
    [
        new("2025-01-01T13:00:00.000Z") { TestName = "AWS SDK format" },
        new("2025-01-01T13:00:00Z") { TestName = "Without fractional seconds" },
    ];
}