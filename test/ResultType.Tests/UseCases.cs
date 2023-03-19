namespace ResultType.Tests;

public class UseCases
{
    [Test]
    public void SampleUseCase()
    {
        var result = CopyPerson();

        if (!result.Succeeded)
            Console.WriteLine($"{nameof(CopyPerson)} was not successful: {result.Message}");
    }

    [Test]
    public void SampleFailingUseCase()
    {
        var result = CopyPersonThrowing();

        if (!result.Succeeded)
            Console.WriteLine($"{nameof(CopyPerson)} was not successful: {result.Message}");
    }

    private PartialResult CopyPerson()
    {
        try
        {
            var personResult = GetPersonFromDatabase()
                .Check();

            return InsertPersonToOtherDatabase(personResult.GetOk())
                .Check();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private PartialResult CopyPersonThrowing()
    {
        try
        {
            var personResult = GetPersonFromDatabaseThrowing()
                .Check();

            return InsertPersonToOtherDatabase(personResult.GetOk())
                .Check();
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private PartialResult<Person> GetPersonFromDatabase()
    {
        try
        {
            return new Person("Test", 20);
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private PartialResult<Person> GetPersonFromDatabaseThrowing()
    {
        try
        {
            throw new InvalidDataException("Person not available!");
        }
        catch (Exception ex)
        {
            return ex;
        }
    }

    private PartialResult InsertPersonToOtherDatabase(Person person)
    {
        try
        {
            return PartialResult.Ok;
        }
        catch (Exception ex)
        {
            return ex;
        }
    }
}

internal record Person(string Name, int Age);