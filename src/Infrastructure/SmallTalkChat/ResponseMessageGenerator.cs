using DropWord.Application.Common.Interfaces.SmallTalkChat;
using DropWord.Application.Common.Models.SmallTalkChat;
using DropWord.Infrastructure.Sentence.Dto;
using DropWord.Infrastructure.Sentence.Field.RestApiRequest;
using DropWord.Infrastructure.SmallTalkChat.Dto;
using DropWord.Infrastructure.SmallTalkChat.Field.RestApiRequest;
using DropWord.Infrastructure.Utils.RestApiClient.Implementation;

namespace DropWord.Infrastructure.SmallTalkChat;

public class ResponseMessageGenerator: IResponseMessageGenerator
{
    public async Task<ResponseMessageModel> GenerateAiMessageAsync(string prompt, int maxNewToken)
    {
        string apiUrl = ResponseMessageGeneratorField.ApiUrl;
        var restApiClient = new RestApiClient();
        var headers = new Dictionary<string, string>()
        {
            { "worker-key", ResponseMessageGeneratorField.Key }
        };
        var body = new { prompt = prompt, max_new_tokens = maxNewToken };
        var apiResponse = await restApiClient.PostAsync<ResponseMessageDTO>(apiUrl, headers, body);
        
        
        return new ResponseMessageModel()
        {
            Message = apiResponse.Message
        };
    }
}
