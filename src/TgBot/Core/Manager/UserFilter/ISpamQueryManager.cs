using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Manager.UserFilter
{
    public interface ISpamQueryManager
    {
        bool IsNoReachLimit(long userId);
        void AddRecord(SpamQueryBDto spamQuery);
    }
}
