using System;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Converters;

[TestFixture]
public class DateTimeDdbConverterShould
{
    [TestCaseSource(nameof(RoundTripIso8601TestCases))]
    public void CorrectlyParseRoundTripIso8601DateTime(string str, DateTime expected)
    {
        var converter = new DateTimeDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        var result = converter.Read(in attributeValue);
        result.ToUniversalTime().ShouldBe(expected);
    }
    
    [TestCaseSource(nameof(NonRoundTripIso8601TestCases))]
    public void FailToParseNonRoundTripIso8601DateTime(string str)
    {
        var converter = new DateTimeDdbConverter();
        var attributeValue = new AttributeValue(new StringAttributeValue(str));

        Should.Throw<FormatException>(() => converter.Read(in attributeValue));
    }

    private static TestCaseData<string, DateTime>[] RoundTripIso8601TestCases =>
    [
        new("2025-01-01T13:00:00.0000000Z", new DateTime(2025, 01, 01, 13, 0, 0, DateTimeKind.Utc)) { TestName = "With 0 fractional seconds" },
        new("2025-01-01T13:00:00.1200000Z", new DateTime(2025, 01, 01, 13, 0, 0, 120, DateTimeKind.Utc)) { TestName = "With milliseconds" },
        new("2025-01-01T13:00:00.0000000+03:00", new DateTime(2025, 01, 01, 10, 0, 0, DateTimeKind.Utc)) { TestName = "With timezone offset" },
    ];

    private static TestCaseData<string>[] NonRoundTripIso8601TestCases =>
    [
        new("2025-01-01T13:00:00.000Z") { TestName = "AWS SDK format" },
        new("2025-01-01T13:00:00Z") { TestName = "Without fractional seconds" },
    ];
}