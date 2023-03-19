// Copyright (c) 2022 Tobias Streng, MIT license

using System.Runtime.Serialization;

namespace ResultType.Extensions;

public static class PartialResultExtensions
{
    /// <summary>
    /// Checks if the <see cref="PartialResult"/> was not successful and throws the underlying exception, if available.
    /// While re-throwing, it preserves the StackTrace. Returns the Result if successful, so that it can be quickly checked.
    /// </summary>
    /// <param name="result"></param>
    public static PartialResult Check(this PartialResult result)
    {
        if (!result.Succeeded)
        {
            var error = result.GetError();
            PreserveStackTrace(error);
            throw error;
        }
        return result;
    }

    /// <summary>
    /// Checks if the <see cref="PartialResult<typeparamref name="T"/>"/> was not successful and throws the underlying exception, if available.
    /// While re-throwing, it preserves the StackTrace. Returns the Result if successful, so that it can be quickly checked.
    /// </summary>
    /// <param name="result"></param>
    public static PartialResult<T> Check<T>(this PartialResult<T> result) where T : class
    {
        if (!result.Succeeded)
        {
            var error = result.GetError();
            PreserveStackTrace(error);
            throw error;
        }
        return result;
    }

    /// <summary>
    /// Preserves the StackTrace, so that the Exception can be catched and re-thrown over multiple layers.
    /// </summary>
    /// <remarks>https://stackoverflow.com/a/2085377</remarks>
    private static void PreserveStackTrace(Exception exception)
    {
        var streamingContext = new StreamingContext(StreamingContextStates.CrossAppDomain);
        var objectManager = new ObjectManager(null, streamingContext);
        var serializationInfo = new SerializationInfo(exception.GetType(), new FormatterConverter());

        exception.GetObjectData(serializationInfo, streamingContext);
        objectManager.RegisterObject(exception, 1, serializationInfo);
        objectManager.DoFixups();
    }
}