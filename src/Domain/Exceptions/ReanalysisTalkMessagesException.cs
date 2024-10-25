namespace DropWord.Domain.Exceptions;

public class ReanalysisTalkMessagesException: Exception
{
    public ReanalysisTalkMessagesException()
    {
    }

    public ReanalysisTalkMessagesException(string? message) : base(message)
    {
    }

    public ReanalysisTalkMessagesException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
