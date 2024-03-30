namespace DropWord.Domain.Exceptions;

public class SentencesNotValidForAddException : Exception
{
    public SentencesNotValidForAddException()
    {
    }

    public SentencesNotValidForAddException(string? message) : base(message)
    {
    }

    public SentencesNotValidForAddException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
