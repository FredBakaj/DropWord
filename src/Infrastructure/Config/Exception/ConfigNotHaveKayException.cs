namespace DropWord.Infrastructure.Config.Exception;

public class ConfigNotHaveKayException : System.Exception
{
    public ConfigNotHaveKayException()
    {
    }

    public ConfigNotHaveKayException(string? message) : base(message)
    {
    }

    public ConfigNotHaveKayException(string? message, System.Exception? innerException) : base(message, innerException)
    {
    }
}
