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
            await MainMenuAsync(update, text);
        }

        [BotView(BaseViewField.AddSentences)]
        public async Task AddSentences(AddSentencesVDto sentences)
        {
            var text = "Було збережено наступні речення";
            foreach (var item in sentences.Sentences)
            {
                text += $"# {item.FirstSentence}\n= {item.SecondSentence}\n\n";
            }

            await MainMenuAsync(sentences.Update, text);
        }

        [BotView(BaseViewField.AddSentence)]
        public async Task AddSentence(AddSentencesVDto sentences)
        {
            var sentence = sentences.Sentences.First();
            var text = $"{sentence.FirstSentence}\n\n {sentence.SecondSentence}";
            await MainMenuAsync(sentences.Update, text);
        }

        [BotView(BaseViewField.NewSentence)]
        public async Task NewSentence(NewSentenceVDto sentence)
        {
            var text = MakeHideSentencesPairText(sentence.NewSentence.HideSentence,
                sentence.NewSentence.FirstSentence, sentence.NewSentence.SecondSentence);

            await _botClient.SendTextMessageAsync(sentence.Update.GetUserId(), text, parseMode: ParseMode.MarkdownV2);
        }

        [BotView(BaseViewField.RepeatSentence)]
        public async Task RepeatSentenceKeyboard(RepeatSentenceVDto repeatSentence)
        {
            var text = MakeHideSentencesPairText(repeatSentence.HideSentenceEnum, repeatSentence.FirstSentence,
                repeatSentence.SecondSentence);

            await _botClient.SendTextMessageAsync(repeatSentence.Update.GetUserId(), text,
                parseMode: ParseMode.MarkdownV2);
        }

        [BotView(BaseViewField.ResetCountRepeatSentence)]
        public async Task ResetCountRepeatSentence(ResetCountRepeatSentenceVDto viewDto)
        {
            var text =
                $"ви повторили {viewDto.Count} речень чи хочете ви повернутися на початок, щоб повторит те що вже повторили?)";
            await ResetRepeatSentencesAsync(viewDto.Update, text);
        }

        [BotView(BaseViewField.ResetOutOfSentencesToRepeat)]
        public async Task ResetOutOfSentencesToRepeat(UpdateBDto updateBDto)
        {
            var text = $"ви дійшли до кінця 🎉🎊🥳 \n Повернутися на початок?";
            await ResetRepeatSentencesAsync(updateBDto, text);
        }

        [BotView(BaseViewField.ConfirmResetCountRepeatSentence)]
        public async Task ConfirmResetCountRepeatSentence(UpdateBDto updateBDto)
        {
            var text = "Ви повернулись на початок, щоб повторит те що могли забути)";
            await MainMenuAsync(updateBDto, text);
        }

        [BotView(BaseViewField.EmptyCollectionOfSentencesToRepeat)]
        public async Task EmptyCollectionOfSentencesToRepeat(UpdateBDto updateBDto)
        {
            var text =
                $"Наразі у вас порожній список для повторення ☹️," +
                $" поповнити його можна за допомогою вивчення нових слів," +
                $" це можна зробити натиснувши кнопку \"{BaseControllerField.NewSentenceButton}\"";
            await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
        }

        private async Task MainMenuAsync(UpdateBDto update, string text)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]
                {
                    BaseControllerField.RepeatSentenceKeyboard, "Повтор ✍️",
                    BaseControllerField.NewSentenceButton
                },
                new KeyboardButton[] { "🇬🇧 🔃 🇬🇧", "⚙️" }
            }) { ResizeKeyboard = true };

            await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
        }

        private string MakeHideSentencesPairText(HideSentenceEnum hideSentenceEnum, string firstSentence,
            string secondSentence)
        {
            var text = string.Empty;
            if (hideSentenceEnum == HideSentenceEnum.MainLanguage)
            {
                text = $"{secondSentence} \n\n" +
                       $"||{firstSentence}||";
            }
            else if (hideSentenceEnum == HideSentenceEnum.LearnLanguage)
            {
                text = $"{firstSentence} \n\n" +
                       $"||{secondSentence}||";
            }

            text = text.Replace("-", "\\-");
            text = text.Replace("#", "\\#");
            return text;
        }

        private async Task ResetRepeatSentencesAsync(UpdateBDto updateBDto, string text)
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Повернутись",
                        callbackData: BaseControllerField.ResetCountRepeatSentencesCallback),
                }
            });
            await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: inlineKeyboard);
        }
    }
}
