namespace DropWord.Domain.Exceptions;

public class LimitAddSentencesExceededException : Exception
{
    public LimitAddSentencesExceededException()
    {
    }

    public LimitAddSentencesExceededException(string? message) : base(message)
    {
    }

    public LimitAddSentencesExceededException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
