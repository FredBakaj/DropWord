namespace DropWord.Domain.Exceptions;

public class MaxCountSentencesException : Exception
{
    public MaxCountSentencesException()
    {
    }

    public MaxCountSentencesException(string? message) : base(message)
    {
    }

    public MaxCountSentencesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
