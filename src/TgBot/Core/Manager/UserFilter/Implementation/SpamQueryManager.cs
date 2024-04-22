using DropWord.TgBot.Core.Model;
using Newtonsoft.Json;

namespace DropWord.TgBot.Core.Manager.UserFilter.Implementation
{
    public class SpamQueryManager : ISpamQueryManager
    {
        private List<SpamQueryBDto> _recordCollection;
        private readonly string directoryPath = $"{System.IO.Directory.GetCurrentDirectory()}/UserRecords";
        
        public SpamQueryManager()
        {
            _recordCollection = new List<SpamQueryBDto>();
        }

        public async Task<bool> IsNoReachLimit(long userId)
        {
            try
            {
                var readCollection = await ReadJsonFromFileAsync($"{directoryPath}/{userId}.txt");
                _recordCollection = JsonConvert.DeserializeObject<List<SpamQueryBDto>>(readCollection)!;
            }
            catch (Exception) { }
            
            var count = _recordCollection.Where(x => x.UserId == userId)
                .Where(x => x.CreateRecord > DateTime.UtcNow.AddSeconds(-5))
                .Count();

            if (count < 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task AddRecord(SpamQueryBDto spamQuery)
        {
            try
            {
                var readCollection = await ReadJsonFromFileAsync($"{directoryPath}/{spamQuery.UserId}.txt");
                _recordCollection = JsonConvert.DeserializeObject<List<SpamQueryBDto>>(readCollection)!;
            }
            catch (Exception) { }
            
            if (_recordCollection.Count > 150)
            {
                _recordCollection.Clear();
            }

            _recordCollection.Add(spamQuery);

            var writeCollection = JsonConvert.SerializeObject(_recordCollection);
            await WriteJsonToFileAsync($"{directoryPath}/{spamQuery.UserId}.txt", writeCollection);
        }

        private async Task WriteJsonToFileAsync(string filePath, string json)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            // Используем FileStream для асинхронной записи данных в файл
            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None,
                       bufferSize: 4096, useAsync: true))
            {
                // Конвертируем JSON строку в массив байтов
                byte[] jsonBytes = System.Text.Encoding.UTF8.GetBytes(json);

                // Асинхронно записываем массив байтов в файл
                await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length);
            }
        }

        static async Task<string> ReadJsonFromFileAsync(string filePath)
        {
            // Используем FileStream для асинхронного чтения данных из файла
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read,
                       bufferSize: 4096, useAsync: true))
            {
                // Используем StreamReader для удобного чтения текста из потока
                using (var reader = new StreamReader(stream))
                {
                    // Асинхронно читаем содержимое файла и возвращаем его как строку
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
