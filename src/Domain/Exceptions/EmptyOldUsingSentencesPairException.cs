namespace DropWord.Domain.Exceptions;

public class EmptyOldUsingSentencesPairException : Exception
{
    public EmptyOldUsingSentencesPairException()
    {
    }

    public EmptyOldUsingSentencesPairException(string? message) : base(message)
    {
    }

    public EmptyOldUsingSentencesPairException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
