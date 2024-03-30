namespace DropWord.Domain.Exceptions;

public class NoNewSentenceException : Exception
{
    public NoNewSentenceException()
    {
    }

    public NoNewSentenceException(string? message) : base(message)
    {
    }

    public NoNewSentenceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
