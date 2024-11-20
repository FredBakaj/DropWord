using DropWord.Application.UseCase.User.Queries.GetCountUsers;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field;
using DropWord.TgBot.Core.Manager.Info;
using DropWord.TgBot.Core.Model;
using MediatR;
using Telegram.Bot;

namespace DropWord.TgBot.Core.Src.Command.Implementation;

public class CountUsersCommand: IBotCommand
{
    private readonly ITelegramBotClient _client;
    private readonly ISender _sender;
    public string GetCommand() => CommandField.CountUsers;
    public bool IsMoveNext() => false;

    public CountUsersCommand(ITelegramBotClient client, ISender sender)
    {
        _client = client;
        _sender = sender;
    }

    public async Task Exec(UpdateBDto update)
    {
        //TODO сделать нормальное разделение ролей, на юзера и админа
        if (update.GetUserId() == 1903751935)
        {
            var returnUserDays = 7;
            var countUsersDto = await _sender.Send(new GetCountUsersQuery(){ReturnUserDays = returnUserDays});
            var text = $"Count users: {countUsersDto.CountUsers}\n" +
                       $"Count return users for days {returnUserDays}: {countUsersDto.CountReturnUsers}";
            await _client.SendTextMessageMarkdown2Async(update.GetUserId(), text);
        }
    }
}
