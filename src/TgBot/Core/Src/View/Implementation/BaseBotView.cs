using DropWord.Infrastructure.Common.Enum;
using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation
{
    public class BaseBotView : ABotView
    {
        private readonly ITelegramBotClient _botClient;

        public BaseBotView(ITelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        [BotView(BaseViewField.Intro)]
        public async Task Intro(UpdateBDto update)
        {
            var text = "Головне меню";
            await MainMenu(update, text);
        }

        [BotView(BaseViewField.AddSentences)]
        public async Task AddSentences(AddSentencesVDto sentences)
        {
            var text = "Було збережено наступні речення";
            foreach (var item in sentences.Sentences)
            {
                text += $"# {item.FirstSentence}\n= {item.SecondSentence}\n\n";
            }

            await _botClient.SendTextMessageAsync(sentences.Update.GetUserId(), text);
        }

        [BotView(BaseViewField.AddSentence)]
        public async Task AddSentence(AddSentencesVDto sentences)
        {
            var sentence = sentences.Sentences.First();
            var text = $"{sentence.FirstSentence}\n\n {sentence.SecondSentence}";
            await _botClient.SendTextMessageAsync(sentences.Update.GetUserId(), text);
        }
        [BotView(BaseViewField.NewSentence)]
        public async Task NewSentence(NewSentenceVDto sentence)
        {
            var text = string.Empty;
            if (sentence.NewSentence.HideSentence == HideSentenceEnum.MainLanguage)
            {
                text = $"{sentence.NewSentence.SecondSentence} \n\n" +
                       $"||{sentence.NewSentence.FirstSentence}||";
            }
            else if (sentence.NewSentence.HideSentence == HideSentenceEnum.LearnLanguage)
            {
                text = $"{sentence.NewSentence.FirstSentence} \n\n" +
                       $"||{sentence.NewSentence.SecondSentence}||";
            }
            
            await _botClient.SendTextMessageAsync(sentence.Update.GetUserId(), text, parseMode:ParseMode.MarkdownV2);
        }
        private async Task MainMenu(UpdateBDto update, string text)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[] { BaseControllerField.RepeatSentenceKeyboard, "Повтор ✍️", BaseControllerField.NewSentenceButton },
                new KeyboardButton[] { "🇬🇧    🔃 🇬🇧", "⚙️" }
            }) { ResizeKeyboard = true };

            await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
        }
    }
}
