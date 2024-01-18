namespace DropWord.Domain.Exceptions;

public class EmptyCollectionOfSentencesToRepeatException : Exception
{
    public EmptyCollectionOfSentencesToRepeatException()
    {
    }

    public EmptyCollectionOfSentencesToRepeatException(string? message) : base(message)
    {
    }

    public EmptyCollectionOfSentencesToRepeatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
