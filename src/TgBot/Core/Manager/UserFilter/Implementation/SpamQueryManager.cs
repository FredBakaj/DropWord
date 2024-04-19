using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Manager.UserFilter.Implementation
{
    public class SpamQueryManager : ISpamQueryManager
    {
        private List<SpamQueryBDto> _recordCollection;

        public SpamQueryManager()
        {
            _recordCollection = new List<SpamQueryBDto>();
        }

        public bool IsNoReachLimit(long userId)
        {
            var count = _recordCollection.Where(x => x.UserId == userId)
                .Where(x => x.CreateRecord > DateTime.UtcNow.AddSeconds(-5))
                 .Count();

            if (count < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AddRecord(SpamQueryBDto spamQuery)
        {
            if (_recordCollection.Count > 10000)
            {
                _recordCollection.Clear();
            }
            _recordCollection.Add(spamQuery);
        }
    }
}
