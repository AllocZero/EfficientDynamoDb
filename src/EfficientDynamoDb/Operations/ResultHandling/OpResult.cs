using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using EfficientDynamoDb.Exceptions;

namespace EfficientDynamoDb.Operations;

/// <summary>
/// Represents the result of a DynamoDB operation.
/// </summary>
public interface IOpResult
{
    /// <summary>
    /// Gets the type of error that occurred during the operation.
    /// </summary>
    public OpErrorType ErrorType { get; }
        
    /// <summary>
    /// Gets the exception that occurred during the operation, if any.
    /// </summary>
    public DdbException? Exception { get; }
        
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess { get; }
}

/// <summary>
/// Represents the result of a DynamoDB operation.
/// </summary>
[StructLayout(LayoutKind.Auto)]
public readonly struct OpResult : IOpResult
{
    /// <inheritdoc />
    public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;

    /// <inheritdoc />
    public DdbException? Exception { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OpResult"/> struct.
    /// </summary>
    /// <param name="exception">The exception that occurred during the operation.</param>
    public OpResult(DdbException? exception)
    {
        Exception = exception;
    }

    /// <summary>
    /// Throws an exception if the operation was not successful.
    /// </summary>
    /// <exception cref="DdbException">The exception that occurred during the operation.</exception>
    public void EnsureSuccess()
    {
        if (Exception is not null)
            throw Exception;
    }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess => Exception is null;
}

/// <summary>
/// Represents the result of a DynamoDB operation with a value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly struct OpResult<T> : IOpResult
{
    /// <summary>
    /// Gets the actual value of the operation result if it was successful, otherwise the default value of <typeparamref name="T"/>.
    /// </summary>
    public T? Value { get; }

    /// <inheritdoc />
    public OpErrorType ErrorType => Exception?.OpErrorType ?? OpErrorType.None;
        
    /// <inheritdoc />
    public DdbException? Exception { get; }
        
    /// <summary>
    /// Initializes a new instance of the <see cref="OpResult{T}"/> struct with an exception.
    /// </summary>
    /// <param name="exception">The exception that occurred during the operation.</param>
    public OpResult(DdbException exception)
    {
        Value = default;
        Exception = exception;
    }
        
    /// <summary>
    /// Initializes a new instance of the <see cref="OpResult{T}"/> struct with a value.
    /// </summary>
    /// <param name="value">The value of the operation result.</param>
    public OpResult(T value)
    {
        Value = value;
        Exception = null;
    }

    /// <summary>
    /// Throws an exception if the operation was not successful, otherwise returns the value.
    /// </summary>
    /// <returns>The value of the operation result.</returns>
    /// <exception cref="DdbException">The exception that occurred during the operation.</exception>
    [MemberNotNullWhen(true, nameof(Value))]
    public T EnsureSuccess()
    {
        if (ErrorType != OpErrorType.None && Exception is not null)
            throw Exception;

        return Value!;
    }

    /// <inheritdoc />
    [MemberNotNullWhen(false, nameof(Exception))]
    public bool IsSuccess => Exception is null;
        
    /// <summary>
    /// Creates a new <see cref="OpResult"/> from the current instance, discarding the value.
    /// </summary>
    /// <returns>A new <see cref="OpResult"/> instance.</returns>
    public OpResult DiscardValue() => new(Exception);
}