using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;

namespace DropWord.TgBot.Core.Manager.Settings;

public interface IMenuSettingsManager
{
    Task<SettingsMenuVDto> CreateSettingsMenuVDto(UpdateBDto updateBDto);
}
