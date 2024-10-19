namespace DropWord.Domain.Exceptions;

public class OpenedChatWithAnotherException: Exception
{
    public OpenedChatWithAnotherException()
    {
    }

    public OpenedChatWithAnotherException(string? message) : base(message)
    {
    }

    public OpenedChatWithAnotherException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
