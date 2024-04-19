namespace DropWord.Domain.Exceptions;

public class TryAddOneWordException : Exception
{
    public TryAddOneWordException()
    {
    }

    public TryAddOneWordException(string? message) : base(message)
    {
    }

    public TryAddOneWordException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
