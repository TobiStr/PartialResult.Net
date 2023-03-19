# PartialResult Type for .NET
A Proposal of a PartialResult Type in .NET

## When to use

This PartialResult Type is useful in scenarios, where you want to mildly enforce checking, if the underlying method has executed successfully, has thrown mild but acceptable errors, or critical errors.
Additionally you will get a cleaner error-handling and reduce the risk of NullReferenceExceptions.

## Usage

### 1. Copy the class definitions from `PartialResult.cs` and `PartialResultExtensions.cs` to your code base and make them accessible publicly.

### 2. Use `PartialResult` or `PartialResult<T>` as return value for all relevant functions:

```c#
private PartialResult DeleteAllFiles()
{
    var errors = new List<Exception>();
    foreach (var file in files) {
        try
        {
            file.Delete();
        }
        catch (Exception ex)
        {
            errors.Add(ex);
        }
    }

    return errors.Any() 
        ? PartialResult.PartialOk(errors.Reduce())
        : PartialResult.Ok;
}
```

### 3. Use the `.Check()` Extension Method to quickly check and throw.
### 4. Return any thrown exceptions from `.Check()` without worries about the StackTrace, as it gets preserved.

## Features

- Implicit castings allow you to directly return the desired type, which will be casted either to a positive `PartialResult<T>`, when you return an object of type `<T>`, or to a negative `PartialResult<T>` / `PartialResult`, when you return an `Exception` object.
- The Extension Method `Check()` quickly allows you to check if a `PartialResult` was successfull and throw the underlying exception (**while preserving the StackTrace**) if it was not.

```
try
{
    Result result = GetResult().Check();
}
catch (Exception ex)
{
    //Here the Exception includes the StackTrace of all underlying (rethrowing) Methods
    //See StackTraceTest for examples
    logger.LogError(ex.ToString());
}

```
- `PartialResult<T>` is inheriting `PartialResult`, so you can always cast to a normal `PartialResult` if you don't need the payload `<T>` anymore.