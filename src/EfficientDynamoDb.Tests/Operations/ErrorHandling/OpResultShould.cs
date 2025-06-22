using System;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Operations;
using NUnit.Framework;
using Shouldly;

namespace EfficientDynamoDb.Tests.Operations.ErrorHandling;

[TestFixture]
public class OpResultShould
{
    [Test]
    public void BeInSuccessStateWhenExceptionIsNotSet()
    {
        var result = new OpResult();

        result.IsSuccess.ShouldBeTrue();
        result.Exception.ShouldBeNull();
        result.ErrorType.ShouldBe(OpErrorType.None);
    }

    [Test]
    public void BeInFailureStateWhenExceptionIsSet()
    {
        var result = new OpResult(new ProvisionedThroughputExceededException("error"));

        result.IsSuccess.ShouldBeFalse();
        result.Exception.ShouldNotBeNull();
        result.ErrorType.ShouldBe(OpErrorType.ProvisionedThroughputExceeded);
    }

    [Test]
    public void NotThrowExceptionWhenEnsureSuccessIsCalledOnSuccessResult()
    {
        var result = new OpResult();
        Should.NotThrow(result.EnsureSuccess);
    }

    [Test]
    public void ThrowExceptionWhenEnsureSuccessIsCalledOnFailureResult()
    {
        var result = new OpResult(new ProvisionedThroughputExceededException("error"));

        Should.Throw<ProvisionedThroughputExceededException>(result.EnsureSuccess);
    }

    [Test]
    public void ThrowExceptionWhenIncorrectAsMethodIsCalled()
    {
        var result = new OpResult(new ProvisionedThroughputExceededException("error"));

        Should.Throw<InvalidOperationException>(() => result.AsInternalServerErrorException());
    }

    [TestCaseSource(nameof(AsExceptionCases))]
    public void ReturnCorrectExceptionWhenAsMethodIsCalled(DdbException expectedException, Func<OpResult, DdbException> asFunc)
    {
        var result = new OpResult(expectedException);

        var actualException = asFunc(result);

        actualException.ShouldBe(expectedException);
    }

    public static TestCaseData<DdbException, Func<OpResult, DdbException>>[] AsExceptionCases() =>
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