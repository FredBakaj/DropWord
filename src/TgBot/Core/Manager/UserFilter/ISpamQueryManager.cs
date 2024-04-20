using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Manager.UserFilter
{
    public interface ISpamQueryManager
    {
        Task<bool> IsNoReachLimit(long userId);
        Task AddRecord(SpamQueryBDto spamQuery);
    }
}
