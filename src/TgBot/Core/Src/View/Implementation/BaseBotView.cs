using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;
using DropWord.TgBot.Core.Attribute;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.Utils;
using DropWord.TgBot.Core.ViewComponent;
using DropWord.TgBot.Core.ViewDto;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace DropWord.TgBot.Core.Src.View.Implementation
{
    public class BaseBotView : ABotView
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IMainMenuComponent _mainMenuComponent;

        public BaseBotView(ITelegramBotClient botClient, IMainMenuComponent mainMenuComponent)
        {
            _botClient = botClient;
            _mainMenuComponent = mainMenuComponent;
        }

        [BotView(BaseViewField.Menu)]
        public async Task Intro(UpdateBDto update)
        {
            var text = "Головне меню";
            await _mainMenuComponent.SendAsync(update, text);
        }

        [BotView(BaseViewField.AddSentences)]
        public async Task AddSentences(AddCollectionSentencesVDto collectionSentences)
        {
            var text = "<b>Добавлено нові речення</b> \u2795 \u2795 \u2795 \n";
            foreach (var item in collectionSentences.Sentences)
            {
                text += $"{collectionSentences.FirstLanguageEmoji} {item.FirstSentence.Sentence}\n" +
                        $"{collectionSentences.SecondLanguageEmoji} {item.SecondSentence.Sentence}\n\n";
            }

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Видалити",
                        callbackData: BaseField.DeleteAddedSentencesCallback + ":" +
                                      collectionSentences.CollectionId),
                }
            });
            await _botClient.SendTextMessageAsync(collectionSentences.Update.GetUserId(), text,
                replyMarkup: inlineKeyboard, parseMode: ParseMode.Html);
        }

        [BotView(BaseViewField.AddSentence)]
        public async Task AddSentence(AddedSentenceVDto sentence)
        {
            var viewElements = AddSentenceElements(sentence.SentencePairId, sentence.FirstSentence.Sentence,
                sentence.SecondSentence.Sentence);

            var text = viewElements.Item1;

            InlineKeyboardMarkup inlineKeyboard = viewElements.Item2;

            await _botClient.SendTextMessageAsync(sentence.Update.GetUserId(), text,
                replyMarkup: inlineKeyboard);
        }


        [BotView(BaseViewField.EditSentence)]
        public async Task EditSentence(EditSentenceVDto editSentenceVDto)
        {
            var text =
                $"Оберить мову речення яке хочете зминити 🪚 \n" +
                $" {editSentenceVDto.FirstSentence.Sentence}\n\n" +
                $" {editSentenceVDto.SecondSentence.Sentence}";

            var firstLangEmojy = CustomConvert.LanguageToEmoji(editSentenceVDto.FirstSentence.Language);
            var secondLangEmojy = CustomConvert.LanguageToEmoji(editSentenceVDto.SecondSentence.Language);
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: firstLangEmojy,
                        callbackData: BaseField.SelectEditAddedSentenceLanguageCallback + ":" +
                                      editSentenceVDto.FirstSentence.Id),
                    InlineKeyboardButton.WithCallbackData(text: secondLangEmojy,
                        callbackData: BaseField.SelectEditAddedSentenceLanguageCallback + ":" +
                                      editSentenceVDto.SecondSentence.Id),
                },
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Повернутись",
                        callbackData: BaseField.CancelEditSingleAddedSentenceCallback + ":" +
                                      editSentenceVDto.Id),
                }
            });
            await _botClient.EditMessageTextAsync(editSentenceVDto.Update.GetUserId(),
                editSentenceVDto.Update.GetMessage().MessageId, text,
                replyMarkup: inlineKeyboard);
        }

        [BotView(BaseViewField.InputEditSentence)]
        public async Task InputEditSentence(UpdateBDto updateBDto)
        {
            var text = "Введіть речення ✏️";
            var textEditMessage = updateBDto.GetMessage().Text;
            await _botClient.EditMessageTextAsync(updateBDto.GetUserId(), updateBDto.GetMessage().MessageId,
                textEditMessage!);
            await _botClient.SendTextMessageAsync(updateBDto.GetUserId(), text);
        }

        [BotView(BaseViewField.CancelEditAddedSentence)]
        public async Task CancelEditAddedSentence(CancelEditAddedSentenceVDto viewDto)
        {
            var viewElements = AddSentenceElements(viewDto.SentencePairId, viewDto.FirstSentence.Sentence,
                viewDto.SecondSentence.Sentence);

            var text = viewElements.Item1;

            InlineKeyboardMarkup inlineKeyboard = viewElements.Item2;

            await _botClient.EditMessageTextAsync(viewDto.Update.GetUserId(), viewDto.Update.GetMessage().MessageId,
                text, replyMarkup: inlineKeyboard);
        }

        [BotView(BaseViewField.DeleteAddedSentence)]
        public async Task DeleteAddedSentence(UpdateBDto updateBDto)
        {
            var text = "Речення було видалено 🖐";
            await _botClient.EditMessageTextAsync(updateBDto.GetUserId(), updateBDto.GetMessage().MessageId, text);
        }

        [BotView(BaseViewField.DeleteAddedSentenceCollection)]
        public async Task DeleteAddedSentenceCollection(UpdateBDto updateBDto)
        {
            var text = "колекцію речень видалено 📄🖐";
            await _botClient.EditMessageTextAsync(updateBDto.GetUserId(), updateBDto.GetMessage().MessageId, text);
        }

        [BotView(BaseViewField.MaxCountSentencesException)]
        public async Task MaxCountSentencesException(MaxCountSentencesExceptionVDto viewDto)
        {
            var text = $"🔴 Перевищена межа кількості речень у тексті." +
                       $" Максимальна кількість речень {viewDto.MaxCountSentences} " +
                       $"(Зверніть увагу, що перенесення рядка, так само як і крапка," +
                       $" вважається новим реченням.)";
            await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text);
        }

        [BotView(BaseViewField.MaxLengthSentenceException)]
        public async Task MaxLengthSentenceException(MaxLengthSentenceExceptionVDto viewDto)
        {
            var text = $"🔴 Перевищена межа кількості символів у реченні." +
                       $" Максимальна кількість символів {viewDto.MaxLengthSentence} ";
            await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text);
        }

        [BotView(BaseViewField.LimitAddSentencesExceededException)]
        public async Task LimitAddSentencesExceededException(LimitAddSentencesExceededExceptionVDto viewDto)
        {
            var text = $"🔴 Перевищено денний ліміт на додавання речень. Денний ліміт {viewDto.MaxCountSentence}";
            await _botClient.SendTextMessageAsync(viewDto.Update.GetUserId(), text);
        }

        [BotView(BaseViewField.SentencesNotValidForAddException)]
        public async Task SentencesNotValidForAddException(UpdateBDto viewDto)
        {
            var text = $"🔴 У тексті присутні заборонені символи (~ * _)";
            await _botClient.SendTextMessageAsync(viewDto.GetUserId(), text);
        }
        [BotView(BaseViewField.NoNewSentenceException)]
        public async Task NoNewSentenceException(UpdateBDto viewDto)
        {
            var text = $"Зараз у вас немає нових речень для вивчення😔 Щоб додати надішліть повідомлення в бот✍️";
            await _botClient.SendTextMessageAsync(viewDto.GetUserId(), text);
        }
        
        [BotView(BaseViewField.DeleteAddedSentenceFailed)]
        public async Task DeleteAddedSentenceFailed(UpdateBDto viewDto)
        {
            var text = viewDto.GetMessage().Text + "\n\n" +
                $"🟡 На жаль це речення вже неможливо видалити";
            await _botClient.EditMessageTextAsync(viewDto.GetUserId(), viewDto.GetMessage().MessageId, text);
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
            await _mainMenuComponent.SendAsync(updateBDto, text);
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

        private (string, InlineKeyboardMarkup) AddSentenceElements(int sentencePairId, string firstSentence,
            string secondSentence)
        {
            var text =
                $"Добавлено нове речення \u2795 \n" +
                $" {firstSentence}\n\n" +
                $" {secondSentence}";
            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new[]
                {
                    InlineKeyboardButton.WithCallbackData(text: "Видалити",
                        callbackData: BaseField.DeleteSingleAddedSentenceCallback + ":" + sentencePairId),
                    InlineKeyboardButton.WithCallbackData(text: "Змінити",
                        callbackData: BaseField.EditSingleAddedSentenceCallback + ":" + sentencePairId),
                }
            });

            return (text, inlineKeyboard);
        }
    }
}
