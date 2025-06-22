using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Operations.ResultHandling;

[TestFixture]
public class OpResultWithValueShould
{
    [Test]
    public void BeInSuccessStateWhenValueIsSet()
    {
        var result = new OpResult<string>("test");

        result.IsSuccess.ShouldBeTrue();
        result.Exception.ShouldBeNull();
        result.ErrorType.ShouldBe(OpErrorType.None);
        result.Value.ShouldBe("test");
    }

    [Test]
    public void BeInFailureStateWhenExceptionIsSet()
    {
        var exception = new ProvisionedThroughputExceededException("error");
        var result = new OpResult<string>(exception);

        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldBe(exception);
        result.ErrorType.ShouldBe(OpErrorType.ProvisionedThroughputExceeded);
        result.Value.ShouldBeNull();
    }

    [Test]
    public void ReturnValueWhenEnsureSuccessIsCalledOnSuccessResult()
    {
        var result = new OpResult<string>("test");

        var value = Should.NotThrow(result.EnsureSuccess);

        value.ShouldBe("test");
    }

    [Test]
    public void ThrowExceptionWhenEnsureSuccessIsCalledOnFailureResult()
    {
        var exception = new ProvisionedThroughputExceededException("error");
        var result = new OpResult<string>(exception);

        Should.Throw<ProvisionedThroughputExceededException>(() => result.EnsureSuccess()).ShouldBe(exception);
    }

    [Test]
    public void ReturnOpResultWhenDiscardValueIsCalledForSuccessfulState()
    {
        var result = new OpResult<string>("test");

        var discardedResult = result.DiscardValue();

        discardedResult.IsSuccess.ShouldBeTrue();
        discardedResult.Exception.ShouldBeNull();
        discardedResult.ErrorType.ShouldBe(OpErrorType.None);
    }

    [Test]
    public void ReturnOpResultWhenDiscardValueIsCalledForFailureState()
    {
        var exception = new ProvisionedThroughputExceededException("error");
        var result = new OpResult<string>(exception);

        var discardedResult = result.DiscardValue();

        discardedResult.IsSuccess.ShouldBeFalse();
        discardedResult.Exception.ShouldBe(exception);
        discardedResult.ErrorType.ShouldBe(OpErrorType.ProvisionedThroughputExceeded);
    }

    [TestCaseSource(nameof(AsExceptionCases))]
    public void ReturnCorrectExceptionWhenAsMethodIsCalled(DdbException expectedException, Func<OpResult<string>, DdbException> asFunc)
    {
        var result = new OpResult<string>(expectedException);

        var actualException = asFunc(result);

        actualException.ShouldBe(expectedException);
    }

    public static TestCaseData<DdbException, Func<OpResult<string>, DdbException>>[] AsExceptionCases() =>
    [
        new(new ServiceUnavailableException("error"), r => r.AsServiceUnavailableException())
            { TestName = "ServiceUnavailableException" },
        new(new InternalServerErrorException("error"), r => r.AsInternalServerErrorException())
            { TestName = "InternalServerErrorException" },
        new(new TransactionCanceledException([], "error"), r => r.AsTransactionCanceledException())
            { TestName = "TransactionCanceledException" },
        new(new ConditionalCheckFailedException(null, "error"), r => r.AsConditionalCheckFailedException())
            { TestName = "ConditionalCheckFailedException" },
        new(new ProvisionedThroughputExceededException("error"), r => r.AsProvisionedThroughputExceededException())
            { TestName = "ProvisionedThroughputExceededException" },
        new(new AccessDeniedException("error"), r => r.AsAccessDeniedException())
            { TestName = "AccessDeniedException" },
        new(new IncompleteSignatureException("error"), r => r.AsIncompleteSignatureException())
            { TestName = "IncompleteSignatureException" },
        new(new ItemCollectionSizeLimitExceededException("error"), r => r.AsItemCollectionSizeLimitExceededException())
            { TestName = "ItemCollectionSizeLimitExceededException" },
        new(new LimitExceededException("error"), r => r.AsLimitExceededException())
            { TestName = "LimitExceededException" },
        new(new MissingAuthenticationTokenException("error"), r => r.AsMissingAuthenticationTokenException())
            { TestName = "MissingAuthenticationTokenException" },
        new(new RequestLimitExceededException("error"), r => r.AsRequestLimitExceededException())
            { TestName = "RequestLimitExceededException" },
        new(new ResourceInUseException("error"), r => r.AsResourceInUseException())
            { TestName = "ResourceInUseException" },
        new(new ResourceNotFoundException("error"), r => r.AsResourceNotFoundException())
            { TestName = "ResourceNotFoundException" },
        new(new ThrottlingException("error"), r => r.AsThrottlingException())
            { TestName = "ThrottlingException" },
        new(new UnrecognizedClientException("error"), r => r.AsUnrecognizedClientException())
            { TestName = "UnrecognizedClientException" },
        new(new ValidationException("error"), r => r.AsValidationException())
            { TestName = "ValidationException" },
        new(new IdempotentParameterMismatchException("error"), r => r.AsIdempotentParameterMismatchException())
            { TestName = "IdempotentParameterMismatchException" },
        new(new TransactionInProgressException("error"), r => r.AsTransactionInProgressException())
            { TestName = "TransactionInProgressException" }
    ];
}