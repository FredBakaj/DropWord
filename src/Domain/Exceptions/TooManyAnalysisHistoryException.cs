namespace DropWord.Domain.Exceptions;

public class TooManyAnalysisHistoryException: Exception
{
    public TooManyAnalysisHistoryException()
    {
    }

    public TooManyAnalysisHistoryException(string? message) : base(message)
    {
    }

    public TooManyAnalysisHistoryException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
