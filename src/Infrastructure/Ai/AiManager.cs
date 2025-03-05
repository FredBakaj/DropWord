using System.ClientModel;
using Azure.AI.OpenAI;
using DropWord.Application.Manager.Ai;
using OpenAI.Chat;

namespace DropWord.Infrastructure.Ai;

public class AiManager : IAiManager
{
    private readonly AzureOpenAIClient _azureOpenAiClient;
    string _deploymentId = "gpt-4";
    private readonly ChatClient _chatClient;

    public AiManager(AzureOpenAIClient azureOpenAiClient)
    {
        _azureOpenAiClient = azureOpenAiClient;
        _chatClient = _azureOpenAiClient.GetChatClient(_deploymentId);
    }

    public async Task<string> QueryToLlmModelAsync(string prompt)
    {
        var messages = new List<ChatMessage> { new SystemChatMessage("Ai agent"), new UserChatMessage(prompt) };

        var options = new ChatCompletionOptions
        {
            Temperature = (float)0.7, MaxOutputTokenCount = 800, FrequencyPenalty = 0, PresencePenalty = 0,
        };
        var completion = (await _chatClient.CompleteChatAsync(messages, options)).Value;

        if (completion.Content != null && completion.Content.Count > 0)
        {
            string responseText = completion.Content[0].Text;
            return responseText;
        }

        throw new InvalidOperationException("Ai model return empty text");

    }
}
