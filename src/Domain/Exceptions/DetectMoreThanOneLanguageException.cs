namespace DropWord.Domain.Exceptions;

public class DetectMoreThanOneLanguageException : Exception
{
    public DetectMoreThanOneLanguageException()
    {
    }

    public DetectMoreThanOneLanguageException(string? message) : base(message)
    {
    }

    public DetectMoreThanOneLanguageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
