using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Internal;

[TestFixture]
public class ErrorHandlerShould
{
    [TestCase("com.amazonaws.dynamodb.v20120810#ExpiredIteratorException", typeof(ExpiredIteratorException))]
    [TestCase("com.amazonaws.dynamodb.v20120810#TrimmedDataAccessException", typeof(TrimmedDataAccessException))]
    public async Task ReturnTypedExceptionForStreamErrors(string errorType, Type expectedException)
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent($$"""{"__type":"{{errorType}}","message":"stream error"}""")
        };

        var exception = await ErrorHandler.ProcessErrorAsync(null!, response);

        exception.ShouldBeOfType(expectedException);
        exception.Message.ShouldBe("stream error");
    }
}
