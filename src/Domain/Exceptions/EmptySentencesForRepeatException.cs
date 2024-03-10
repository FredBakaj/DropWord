namespace DropWord.Domain.Exceptions;

public class EmptySentencesForRepeatException : Exception
{
    public EmptySentencesForRepeatException()
    {
    }

    public EmptySentencesForRepeatException(string? message) : base(message)
    {
    }

    public EmptySentencesForRepeatException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
