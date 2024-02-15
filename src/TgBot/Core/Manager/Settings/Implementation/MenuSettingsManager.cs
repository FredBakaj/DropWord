using DropWord.Application.UseCase.User.Queries.GetUser;
using DropWord.Domain.Enums;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Utils;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Manager.Settings.Implementation;

public class MenuSettingsManager : IMenuSettingsManager
{
    private readonly ISender _sender;

    public MenuSettingsManager(ISender sender)
    {
        _sender = sender;
    }
    
    public async Task<SettingsMenuVDto> CreateSettingsMenuVDto(UpdateBDto updateBDto)
    {
        var user = await _sender.Send(new GetUserQuery() { UserId = updateBDto.GetUserId() });
        
        var changeModeIcon = GetChangeModeIcons(user.UserSettings.MainLanguage,
            user.UserSettings.LearnLanguage, user.UserSettings.LearnSentencesModeEnum);
        
        var learnLanguagePairEmoji = $"{CustomConvert.LanguageToEmoji(user.UserSettings.MainLanguage)}" +
                                     $"{CustomConvert.LanguageToEmoji(user.UserSettings.LearnLanguage)}";
        var viewDto = new SettingsMenuVDto()
        {
            Update = updateBDto,
            ChangeModeIcon = changeModeIcon,
            LearnLanguagePairEmoji = learnLanguagePairEmoji
        };
        return viewDto;
    }
    
    private string GetChangeModeIcons(string mainLanguage, string learnLanguage,
        LearnSentencesModeEnum learnSentencesModeEnum)
    {
        var changeModeIcons = new Dictionary<LearnSentencesModeEnum, string>()
        {
            {
                LearnSentencesModeEnum.MainLanguage,
                CustomConvert.LanguageToEmoji(mainLanguage)
            },
            {
                LearnSentencesModeEnum.LearnLanguage,
                CustomConvert.LanguageToEmoji(learnLanguage)
            },
            { LearnSentencesModeEnum.Learned, "🧠" },
            { LearnSentencesModeEnum.Random, "🎲" },
        };
        return changeModeIcons[learnSentencesModeEnum];
    }
}
