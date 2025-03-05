namespace DropWord.Application.Manager.Ai;

public interface IAiManager
{
    Task<string> QueryToLlmModelAsync(string prompt);
}
