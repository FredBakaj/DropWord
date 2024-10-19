namespace DropWord.Domain.Exceptions;

public class NoOpenChatException: Exception
{
    public NoOpenChatException()
    {
    }

    public NoOpenChatException(string? message) : base(message)
    {
    }

    public NoOpenChatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
