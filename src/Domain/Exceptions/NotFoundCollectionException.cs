namespace DropWord.Domain.Exceptions;

public class NotFoundCollectionException : Exception
{
    public NotFoundCollectionException()
    {
    }

    public NotFoundCollectionException(string? message) : base(message)
    {
    }

    public NotFoundCollectionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
