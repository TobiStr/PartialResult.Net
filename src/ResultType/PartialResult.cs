// Copyright (c) 2023 Tobias Streng, MIT license

namespace ResultType;

/// <summary>
/// Used to determine, whether a method executes successfully, partially successful or not. Has implicit cast implemented:
/// Return <see cref="Exception"/> (or inherited type) to return a Result with Success set to 'Error'.
/// </summary>
public class PartialResult
{
    protected readonly Exception? error;

    /// <summary>
    /// Indicates if the operation was successful.
    /// </summary>
    public Success Success { get; protected set; }

    /// <summary>
    /// Returns if the operation succeeded fully or partially.
    /// </summary>
    public bool Succeeded
    {
        get => Success == Success.Success || Success == Success.PartialSuccess;
    }

    /// <summary>
    /// Returns if the operation succeeded partially.
    /// </summary>
    public bool SucceededPartially
    {
        get => Success == Success.PartialSuccess;
    }

    /// <summary>
    /// Gets the message of the Exception, if available.
    /// </summary>
    public string Message { get => error?.Message ?? ""; }

    protected PartialResult(Success success, Exception? error)
    {
        Success = success;
        this.error = error;
    }

    /// <summary>
    /// Gets the error payload of the failed result.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public Exception GetError() => error
        ?? throw new InvalidOperationException($"Error property for this Result not set.");

    /// <summary>
    /// Returns a positive Result.
    /// </summary>
    public static PartialResult Ok
    {
        get => new PartialResult(Success.Success, null);
    }

    /// <summary>
    /// Returns a partial positive Result with a mild error.
    /// </summary>
    public static PartialResult PartialOk(Exception error)
    {
        return new PartialResult(Success.PartialSuccess, error);
    }

    /// <summary>
    /// Returns a negative Result with an Exception as payload.
    /// </summary>
    public static PartialResult Error(Exception error)
    {
        return new PartialResult(Success.Error, error);
    }

    /// <summary>
    /// Implicit cast from type Exception (or inherited) to Result with Success set to 'false'.
    /// </summary>
    public static implicit operator PartialResult(Exception exception) =>
        new PartialResult(Success.Error, exception);
}

/// <summary>
/// Used to determine, whether a method executes successfully, partially successful or not. Has implicit casts implemented:
/// Return <typeparamref name="TPayload"/> to return a successful Result with this payload.
/// Return <see cref="Exception"/> (or inherited type) to return a Result with Success set to 'Error'.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public sealed class PartialResult<TPayload> : PartialResult
    where TPayload : class
{
    private readonly TPayload? payload;

    private PartialResult(TPayload? payload, Exception? error, Success success) : base(success, error)
    {
        this.payload = payload;
    }

    public PartialResult(TPayload payload) : base(Success.Success, null)
    {
        this.payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public PartialResult(TPayload payload, Exception error) : base(Success.PartialSuccess, error)
    {
        this.payload = payload ?? throw new ArgumentNullException(nameof(payload));
    }

    public PartialResult(Exception error) : base(Success.Error, error)
    {
    }

    /// <summary>
    /// Gets the underlying payload of the successful result.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public TPayload GetOk() => Success == Success.Success || Success == Success.PartialSuccess
        ? payload ?? throw new InvalidOperationException($"Payload for Result<{typeof(TPayload)}> was not set.")
        : throw new InvalidOperationException($"Operation for Result<{typeof(TPayload)}> was not successful.");

    /// <summary>
    /// Gets the error payload of the failed result.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public new Exception GetError() => error
        ?? throw new InvalidOperationException($"Error property for Result<{typeof(TPayload)}> not set.");

    /// <summary>
    /// Returns a positive Result with an object as payload.
    /// </summary>
    public new static PartialResult<TPayload> Ok(TPayload payload)
    {
        return new PartialResult<TPayload>(payload, null, Success.Success);
    }

    /// <summary>
    /// Returns a positive Result with an object as payload.
    /// </summary>
    public static PartialResult<TPayload> PartialOk(TPayload payload, Exception error)
    {
        return new PartialResult<TPayload>(payload, error, Success.PartialSuccess);
    }

    /// <summary>
    /// Returns a negative Result with an Exception as payload.
    /// </summary>
    public new static PartialResult<TPayload> Error(Exception error)
    {
        return new PartialResult<TPayload>(null, error, Success.Error);
    }

    /// <summary>
    /// Implicit cast from type <typeparamref name="TPayload"/> to Result with Success set to 'true'.
    /// </summary>
    public static implicit operator PartialResult<TPayload>(TPayload payload) =>
        new(payload, null, Success.Success);

    /// <summary>
    /// Implicit cast from type Exception (or inherited) to Result<TPayload> with Success set to 'false'.
    /// </summary>
    public static implicit operator PartialResult<TPayload>(Exception exception) =>
        new(exception);
}