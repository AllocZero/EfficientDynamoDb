using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Operations;

public interface IOpResult
{
    public OpErrorType ErrorType { get; }
        
    public DdbException? Exception { get; }
        
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess { get; }
}

[StructLayout(LayoutKind.Auto)]
public readonly struct OpResult : IOpResult
{
    public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;

    public DdbException? Exception { get; }

    public OpResult(DdbException? exception)
    {
        Exception = exception;
    }

    public void EnsureSuccess()
    {
        if (Exception is not null)
            throw Exception;
    }

    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess => Exception is null;
}

public readonly struct OpResult<T> : IOpResult
{
    public T? Value { get; }

    public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;
        
    public DdbException? Exception { get; }
        
    public OpResult(DdbException exception)
    {
        Value = default;
        Exception = exception;
    }
        
    public OpResult(T value)
    {
        Value = value;
        Exception = null;
    }

    [MemberNotNullWhen(true, nameof(Value))]
    public T EnsureSuccess()
    {
        if (ErrorType != OpErrorType.None && Exception is not null)
            throw Exception;

        return Value!;
    }

    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess => Exception is null;
        
    public OpResult DiscardValue() => new(Exception);
}