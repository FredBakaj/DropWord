namespace DropWord.Domain.Exceptions;

public class NoTalkMessagesException: Exception
{
    public NoTalkMessagesException()
    {
    }

    public NoTalkMessagesException(string? message) : base(message)
    {
    }

    public NoTalkMessagesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
