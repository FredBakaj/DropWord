using System.Runtime.Serialization;

namespace DropWord.Domain.Exceptions;

public class MaxLengthSentenceException : Exception
{
    public MaxLengthSentenceException()
    {
    }

    public MaxLengthSentenceException(string? message) : base(message)
    {
    }

    public MaxLengthSentenceException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
