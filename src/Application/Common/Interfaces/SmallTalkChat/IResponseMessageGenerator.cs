using DropWord.Application.Common.Models.SmallTalkChat;

namespace DropWord.Application.Common.Interfaces.SmallTalkChat;

public interface IResponseMessageGenerator
{
    Task<ResponseMessageModel> GenerateAiMessageAsync(string prompt, int maxNewToken);
}
