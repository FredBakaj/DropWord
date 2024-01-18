namespace DropWord.Domain.Exceptions;

public class OutOfSentencesToRepeatException : Exception
{
    public OutOfSentencesToRepeatException()
    {
    }

    public OutOfSentencesToRepeatException(string? message) : base(message)
    {
    }

    public OutOfSentencesToRepeatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
