using System;
using EfficientDynamoDb.Internal.Extensions;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Internal.Extensions;

[TestFixture]
public class StringNormalizationExtensionsShould
{
    [TestCaseSource(nameof(UpperSnakeCaseTestCases))]
    public void ConvertToUpperSnakeCase(string value, string expected)
    {
        Span<char> destination = stackalloc char[value.Length * 2];

        var written = value.ToUpperSnakeCaseAscii(destination);

        written.ShouldBe(expected.Length);
        destination[..written].ToString().ShouldBe(expected);
    }

    [Test]
    public void NotTouchTheRestOfTheDestination()
    {
        var destination = new char[32];
        Array.Fill(destination, '#');

        var written = "PayPerRequest".ToUpperSnakeCaseAscii(destination);

        written.ShouldBe("PAY_PER_REQUEST".Length);
        destination[written..].ShouldAllBe(c => c == '#');
    }

    private static TestCaseData<string, string>[] UpperSnakeCaseTestCases =>
    [
        new("", "") { TestName = "Empty string" },
        new("a", "A") { TestName = "Single lowercase char" },
        new("A", "A") { TestName = "Single uppercase char" },
        new("5", "5") { TestName = "Single digit" },
        new("_", "_") { TestName = "Single underscore" },
        new("value", "VALUE") { TestName = "Single lowercase word" },
        new("Value", "VALUE") { TestName = "Single capitalized word" },
        new("VALUE", "VALUE") { TestName = "Single uppercase word" },
        new("PayPerRequest", "PAY_PER_REQUEST") { TestName = "Pascal case" },
        new("payPerRequest", "PAY_PER_REQUEST") { TestName = "Camel case" },
        new("IO", "IO") { TestName = "Acronym only" },
        new("AWSRequestId", "AWS_REQUEST_ID") { TestName = "Leading multi char acronym" },
        new("DynamoDBTable", "DYNAMO_DB_TABLE") { TestName = "Acronym in the middle" },
        new("ValueX", "VALUE_X") { TestName = "Trailing single uppercase char" },
        new("Sha256", "SHA256") { TestName = "Trailing digits" },
        new("Value2Test", "VALUE2_TEST") { TestName = "Digits in the middle" },
        new("2Value", "2_VALUE") { TestName = "Leading digit" },
        new("PayPerRequestV2", "PAY_PER_REQUEST_V2") { TestName = "Uppercase char followed by a digit" },
        new("Pay_Per_Request", "PAY_PER_REQUEST") { TestName = "Existing underscores are not duplicated" },
        new("Foo-Bar", "FOO-BAR") { TestName = "Non-alphanumeric chars are kept as separators" },
    ];
}
