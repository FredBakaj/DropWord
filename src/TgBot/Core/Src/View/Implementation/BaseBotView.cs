using DropWord.Domain.Enums;
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

        [BotView(BaseViewField.Menu)]
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
            var text = MakeHideSentencesPairText(sentence.NewSentence.SentenceToLearnLabel,
                sentence.NewSentence.FirstSentence, sentence.NewSentence.SecondSentence);

            await _botClient.SendTextMessageMarkdown2Async(sentence.Update.GetUserId(), text);
        }

        [BotView(BaseViewField.RepeatSentence)]
        public async Task RepeatSentenceKeyboard(RepeatSentenceVDto repeatSentence)
        {
            var text = MakeHideSentencesPairText(repeatSentence.SentenceToLearnLabel, repeatSentence.FirstSentence,
                repeatSentence.SecondSentence);

            await _botClient.SendTextMessageMarkdown2Async(repeatSentence.Update.GetUserId(), text);
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
                $" це можна зробити натиснувши кнопку \"{BaseField.NewSentenceButton}\"";
            await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
        }

        private async Task MainMenuAsync(UpdateBDto update, string text)
        {
            var replyMarkup = new ReplyKeyboardMarkup(new[]
            {
                new KeyboardButton[]
                {
                    BaseField.RepeatSentenceKeyboard, BaseField.SentencesRepetitionByInputKeyboard,
                    BaseField.NewSentenceButton
                },
                new KeyboardButton[] { "🇬🇧 🔃 🇬🇧", "⚙️" }
            }) { ResizeKeyboard = true };

            await _botClient.SendTextMessageAsync(update.GetUserId(), text, replyMarkup: replyMarkup);
        }

        private string MakeHideSentencesPairText(SentenceToLearnLabelEnum learnSentencesModeEnum, string firstSentence,
            string secondSentence)
        {
            var text = string.Empty;
            if (learnSentencesModeEnum == SentenceToLearnLabelEnum.First)
            {
                text = $"{secondSentence} \n\n" +
                       $"||{firstSentence}||";
            }
            else if (learnSentencesModeEnum == SentenceToLearnLabelEnum.Second)
            {
                text = $"{firstSentence} \n\n" +
                       $"||{secondSentence}||";
            }
            
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
                        callbackData: BaseField.ResetCountRepeatSentencesCallback),
                }
            });
            await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text, replyMarkup: inlineKeyboard);
        }
    }
}
