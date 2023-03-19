namespace ResultType.Tests;

public class PartialResultTests
{
    [Test]
    public void GetOkTest()
    {
        var exception = new Exception("Mild Error");

        var okResult = PartialResult.Ok;
        var okResult2 = PartialResult<string>.Ok("Test");
        var okResult3 = PartialResult.PartialOk(exception);
        var okResult4 = PartialResult<string>.PartialOk("Test", exception);

        Assert.True(okResult.Succeeded);
        Assert.True(okResult2.Succeeded);
        Assert.True(okResult3.Succeeded);
        Assert.True(okResult4.Succeeded);
        Assert.False(okResult.SucceededPartially);
        Assert.False(okResult2.SucceededPartially);
    }

    [Test]
    public void GetPartialOkTest()
    {
        var exception = new Exception("Mild Error");

        var okResult = PartialResult.PartialOk(exception);
        var okResult2 = PartialResult<string>.PartialOk("Test", exception);

        Assert.True(okResult.SucceededPartially);
        Assert.True(okResult2.SucceededPartially);
        Assert.True(okResult.Succeeded);
        Assert.True(okResult2.Succeeded);
    }

    [Test]
    public void GetErrorTest()
    {
        var exception = new Exception("Test");
        var errorResult = PartialResult.Error(exception);
        var errorResult2 = PartialResult<string>.Error(exception);

        Assert.False(errorResult.Succeeded);
        Assert.False(errorResult2.Succeeded);
    }

    [Test]
    public void ImplicitCastTest()
    {
        var exception = new Exception("Test");
        PartialResult errorResult = exception;
        PartialResult<string> errorResult2 = exception;
        PartialResult<string> okResult = "Test";

        Assert.False(errorResult.Succeeded);
        Assert.False(errorResult2.Succeeded);
        Assert.True(okResult.Succeeded);
    }

    [Test]
    public void ThrowTest()
    {
        var okResult = PartialResult.Ok;
        var okResult2 = PartialResult<string>.Ok("Test");

        var exception = new Exception("Test");
        PartialResult errorResult = exception;
        PartialResult<string> errorResult2 = exception;

        Assert.DoesNotThrow(() => okResult.Check());
        Assert.DoesNotThrow(() => okResult2.Check());

        Assert.Throws(exception.GetType(), () => errorResult.Check());
        Assert.Throws(exception.GetType(), () => errorResult2.Check());
    }
}