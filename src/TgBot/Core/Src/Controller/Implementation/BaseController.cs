using AutoMapper;
using DropWord.Application.UseCase.Sentence.Commands.AddSentence;
using DropWord.Application.UseCase.Sentence.Commands.DeleteAddedSentence;
using DropWord.Application.UseCase.Sentence.Commands.LearnNewSentence;
using DropWord.Application.UseCase.Sentence.Commands.ParseSentences;
using DropWord.Application.UseCase.Sentence.Commands.RepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.ResetCountRepeatSentence;
using DropWord.Application.UseCase.Sentence.Commands.UpdateSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetNewSentence;
using DropWord.Application.UseCase.Sentence.Queries.GetSentenceForRepeat;
using DropWord.Application.UseCase.Sentence.Queries.GetSentencesPair;
using DropWord.Application.UseCase.SentencesCollection.Commands.AddCollection;
using DropWord.Application.UseCase.SentencesCollection.Commands.DeleteAddedSentenceCollection;
using DropWord.Application.UseCase.SentencesCollection.Queries.GetSentencesCollection;
using DropWord.Application.UseCase.User.Queries.GetUser;
using DropWord.Application.UseCase.UserSettings.Commands.ChangeLanguagePair;
using DropWord.Application.UseCase.UserSettings.Commands.ChangeLearnSentencesMode;
using DropWord.Application.UseCase.UserSettings.Commands.ChangeSentencesRepeatForDayMode;
using DropWord.Application.UseCase.UserSettings.Commands.ChangeTimeZone;
using DropWord.Application.UseCase.UserSettings.Commands.SendFeedback;
using DropWord.Domain.Constants;
using DropWord.Domain.Enums;
using DropWord.Domain.Exceptions;
using DropWord.TgBot.Core.Extension;
using DropWord.TgBot.Core.Field.Controller;
using DropWord.TgBot.Core.Field.View;
using DropWord.TgBot.Core.Handler.BotStateTreeHandler;
using DropWord.TgBot.Core.Handler.BotStateTreeUserHandler;
using DropWord.TgBot.Core.Handler.BotViewHandler;
using DropWord.TgBot.Core.Manager.RepeatSentence;
using DropWord.TgBot.Core.Manager.Settings;
using DropWord.TgBot.Core.Model;
using DropWord.TgBot.Core.StateDto;
using DropWord.TgBot.Core.Utils;
using DropWord.TgBot.Core.ViewDto;
using MediatR;

namespace DropWord.TgBot.Core.Src.Controller.Implementation
{
    public class BaseController : IBotController
    {
        private readonly IBotStateTreeHandler _botStateTreeHandler;
        private readonly IBotViewHandler _botViewHandler;
        private readonly ISender _sender;
        private readonly IBotStateTreeUserHandler _botStateTreeUserHandler;
        private readonly IRepeatSentenceManager _repeatSentenceManager;
        private readonly IMenuSettingsManager _menuSettingsManager;
        private readonly IMapper _mapper;
        private readonly int _maxSentenceLength;
        private readonly int _maxCountSentences;
        private int _maxCountAddedSentences;
        private int _maxMinutesForDeleteSentence;


        public string Name() => BaseField.BaseState;

        public BaseController(IBotStateTreeHandler botStateTreeHandler, IBotViewHandler botViewHandler, ISender sender,
            IBotStateTreeUserHandler botStateTreeUserHandler,
            IRepeatSentenceManager repeatSentenceManager,
            IMenuSettingsManager menuSettingsManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            _botStateTreeHandler = botStateTreeHandler;
            _botViewHandler = botViewHandler;
            _sender = sender;
            _botStateTreeUserHandler = botStateTreeUserHandler;
            _repeatSentenceManager = repeatSentenceManager;
            _menuSettingsManager = menuSettingsManager;
            _mapper = mapper;

            _maxSentenceLength =
                Convert.ToInt32(configuration.GetSection("SentencesSettings")["MaxLengthSentenceForSave"]);
            _maxCountSentences =
                Convert.ToInt32(configuration.GetSection("SentencesSettings")["MaxCountSentencesForSave"]);
            _maxCountAddedSentences =
                Convert.ToInt32(configuration.GetSection("ApplicationSettings")["LimitForAddedSentences"]);
            _maxMinutesForDeleteSentence =
                Convert.ToInt32(configuration.GetSection("SentencesSettings")["MaxMinutesForDeleteSentence"]);


            Initialize();
        }

        public async Task Exec(UpdateBDto update)
        {
            await _botStateTreeHandler.ExecuteRoute(update);
        }

        private void Initialize()
        {
            _botStateTreeHandler.AddAction(BaseField.BaseAction, BaseAction);
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.NewSentenceButton,
                NewSentenceButton);
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.RepeatSentenceKeyboard,
                RepeatSentenceKeyboard);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction,
                BaseField.ResetCountRepeatSentencesCallback,
                ResetCountRepeatSentencesCallback);
            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.SentencesRepetitionByInputKeyboard,
                SentencesRepetitionByInputKeyboard);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.EditSingleAddedSentenceCallback,
                StartEditAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.CancelEditSingleAddedSentenceCallback,
                CancelEditAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.SelectEditAddedSentenceLanguageCallback,
                SelectEditAddedSentenceLanguageCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.DeleteSingleAddedSentenceCallback,
                DeleteAddedSentenceCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.DeleteAddedSentencesCallback,
                DeleteAddedSentencesCallbackAsync);

            _botStateTreeHandler.AddAction(BaseField.InputEditSentenceAction, InputEditAddedSentenceActionAsync);
            _botStateTreeHandler.AddAction(BaseField.ReloadAction, ReloadAction);

            _botStateTreeHandler.AddKeyboard(BaseField.BaseAction, BaseField.SettingsKeyboard,
                SettingsMenuKeyboardAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.ChangeLearnSentencesModeCallback,
                ChangeLearnSentencesModeCallbackAsync);

            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.OpenChangeLearnLanguagePairCallback,
                OpenChangeLearnLanguagePairCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.ChangeLearnLanguagePairCallback,
                ChangeLearnLanguagePairCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.BackToSettingsMenuCallback,
                BackToSettingsMenuCallbackAsync);

            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.OpenChangeTimeZoneCallback,
                OpenChangeTimeZoneCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.ChangeTimeZoneCallback,
                ChangeTimeZoneCallbackAsync);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.OpenChangeTimesForDayCallback,
                OpenChangeTimesForDayCallback);
            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.ChangeTimesForDayCallback,
                ChangeTimesForDayCallback);

            _botStateTreeHandler.AddCallback(BaseField.BaseAction, BaseField.InputFeedbackCallback,
                OnInputFeedbackAsync);
            _botStateTreeHandler.AddAction(BaseField.InputFeedbackAction, InputFeedbackAction);
            _botStateTreeHandler.AddKeyboard(BaseField.InputFeedbackAction, BaseField.CancelInputFeedbackKeyboard,
                OnCancelInputFeedbackAsync);
        }

        //Добавление предложений в базу 
        private async Task BaseAction(UpdateBDto update)
        {
            try
            {
                var sentenceParse =
                    await _sender.Send(new ParseSentencesCommand() { Content = update.GetMessage().Text! });

                if (sentenceParse.Sentences.Count() == 1)
                {
                    var addedSentence = await _sender.Send(new AddSentenceCommand()
                    {
                        UserId = update.GetUserId(), Sentence = sentenceParse.Sentences.First()
                    });
                    var viewDto = _mapper.Map<AddedSentenceVDto>(addedSentence);
                    viewDto.Update = update;
                    await _botViewHandler.SendAsync(BaseViewField.AddSentence, viewDto);
                }
                else if (sentenceParse.Sentences.Count() > 1)
                {
                    var addedSentences = await _sender.Send(new AddCollectionCommand()
                    {
                        UserId = update.GetUserId(), Sentences = sentenceParse.Sentences, Description = "Text"
                    });
                    var userSettings = await _sender.Send(new GetUserQuery() { UserId = update.GetUserId() });

                    var viewDto = _mapper.Map<AddCollectionSentencesVDto>(addedSentences);
                    viewDto.FirstLanguageEmoji = CustomConvert.LanguageToEmoji(userSettings.UserSettings.MainLanguage);
                    viewDto.SecondLanguageEmoji =
                        CustomConvert.LanguageToEmoji(userSettings.UserSettings.LearnLanguage);
                    viewDto.Update = update;

                    await _botViewHandler.SendAsync(BaseViewField.AddSentences, viewDto);
                }
            }
            catch (InvalidDataException)
            {
                await _botViewHandler.SendAsync(BaseViewField.InvalidDataException, update);

            }
            catch (MaxCountSentencesException)
            {
                var viewDto =
                    new MaxCountSentencesExceptionVDto() { Update = update, MaxCountSentences = _maxCountSentences };
                await _botViewHandler.SendAsync(BaseViewField.MaxCountSentencesException, viewDto);
            }
            catch (MaxLengthSentenceException)
            {
                var viewDto =
                    new MaxLengthSentenceExceptionVDto() { Update = update, MaxLengthSentence = _maxSentenceLength };
                await _botViewHandler.SendAsync(BaseViewField.MaxLengthSentenceException, viewDto);
            }
            catch (LimitAddSentencesExceededException)
            {
                var viewDto =
                    new LimitAddSentencesExceededExceptionVDto()
                    {
                        Update = update, MaxCountSentence = _maxCountAddedSentences
                    };
                await _botViewHandler.SendAsync(BaseViewField.LimitAddSentencesExceededException, viewDto);
            }
            catch (SentencesNotValidForAddException)
            {
                await _botViewHandler.SendAsync(BaseViewField.SentencesNotValidForAddException, update);
            }
            catch (TryAddOneWordException)
            {
                await _botViewHandler.SendAsync(BaseViewField.TryAddOneWordException, update);

            }
        }

        private async Task ReloadAction(UpdateBDto updateBDto)
        {
            await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto, BaseField.BaseState,
                BaseField.BaseAction);
            await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
        }

        private async Task StartEditAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);
            var sentencesPair = await _sender.Send(new GetSentencesPairQuery()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            var viewModel = _mapper.Map<EditSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;

            await _botViewHandler.SendAsync(BaseViewField.EditSentence, viewModel);
        }

        private async Task CancelEditAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);
            var sentencesPair = await _sender.Send(new GetSentencesPairQuery()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            var viewModel = _mapper.Map<CancelEditAddedSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;
            await _botViewHandler.SendAsync(BaseViewField.CancelEditAddedSentence, viewModel);
        }

        private async Task SelectEditAddedSentenceLanguageCallbackAsync(UpdateBDto updateBDto)
        {
            int sentenceId = Convert.ToInt32(updateBDto.CallbackData);
            var data = new EditSentenceSDto() { SentenceId = sentenceId };
            await _botStateTreeUserHandler.SetDataAndActionAsync(updateBDto, BaseField.InputEditSentenceAction, data);
            await _botViewHandler.SendAsync(BaseViewField.InputEditSentence, updateBDto);
        }

        private async Task InputEditAddedSentenceActionAsync(UpdateBDto updateBDto)
        {
            int sentenceId = ((await _botStateTreeUserHandler.GetDataAsync<EditSentenceSDto>(updateBDto))!).SentenceId;
            string updateSentenceText = updateBDto.GetMessage().Text!;
            long userId = updateBDto.GetUserId();

            //Возвращаем пользователя в базовое состояние
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, BaseField.BaseAction);
            await _botStateTreeUserHandler.ClearDataAsync(updateBDto);

            // Обновление предложения
            var sentencesPair = await _sender.Send(new UpdateSentenceCommand()
            {
                UserId = userId, SentenceId = sentenceId, Sentence = updateSentenceText
            });

            //Отображаем вюшку обнавленой пары предложений
            var viewModel = _mapper.Map<AddedSentenceVDto>(sentencesPair);
            viewModel.Update = updateBDto;
            await _botViewHandler.SendAsync(BaseViewField.AddSentence, viewModel);
        }

        private async Task DeleteAddedSentenceCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesPairId = Convert.ToInt32(updateBDto.CallbackData);

            var sentencesPair = await _sender.Send(new GetSentencesPairQuery()
            {
                UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
            });
            if (DateTimeOffset.UtcNow - sentencesPair.Created < TimeSpan.FromMinutes(_maxMinutesForDeleteSentence))
            {
                await _sender.Send(new DeleteAddedSentenceCommand()
                {
                    UserId = updateBDto.GetUserId(), SentencesPairId = sentencesPairId
                });
                await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentence, updateBDto);
            }
            else
            {
                await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentenceFailed, updateBDto);
            }
        }

        private async Task DeleteAddedSentencesCallbackAsync(UpdateBDto updateBDto)
        {
            int sentencesCollectionId = Convert.ToInt32(updateBDto.CallbackData);

            var sentencesCollection = await _sender.Send(new GetSentencesCollectionQuery()
            {
                SentencesCollectionId = sentencesCollectionId
            });

            if (DateTimeOffset.UtcNow - sentencesCollection.Created < TimeSpan.FromMinutes(_maxMinutesForDeleteSentence))
            {
                await _sender.Send(new DeleteAddedSentenceCollectionCommand()
                {
                    UserId = updateBDto.GetUserId(), CollectionId = sentencesCollectionId
                });
                await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentenceCollection, updateBDto);
            }
            else
            {
                await _botViewHandler.SendAsync(BaseViewField.DeleteAddedSentenceFailed, updateBDto);
            }
        }

        //Отправляет новую пару предложений для изучения
        private async Task NewSentenceButton(UpdateBDto update)
        {
            try
            {
                var newSentenceDto = await _sender.Send(new GetNewSentenceQuery() { UserId = update.GetUserId() });

                var viewDto = new NewSentenceVDto() { Update = update, NewSentence = newSentenceDto };
                await _botViewHandler.SendAsync(BaseViewField.NewSentence, viewDto);

                await _sender.Send(new LearnNewSentenceCommand()
                {
                    UserId = update.GetUserId(), SentencePairId = newSentenceDto.SentencePairId
                });
            }
            catch (NoNewSentenceException)
            {
                await _botViewHandler.SendAsync(BaseViewField.NoNewSentenceException, update);
            }
        }

        //Отправляет уже изученую пару предложений для повторения
        private async Task RepeatSentenceKeyboard(UpdateBDto updateBDto)
        {
            //Оборачиваем отображения пары слов в проверку на кол-во отображонных слов 
            if (await _repeatSentenceManager.CanShowResetCountRepeatSentences(updateBDto))
            {
                await RepeatSentenceAsync(updateBDto);
                await _repeatSentenceManager.ClearShowResetCountRepeatSentencesView(updateBDto);
            }
            else
            {
                var countRepetitionSentences =
                    await _repeatSentenceManager.GetCountRepetitionSentences(updateBDto.GetUserId());
                var viewDto = new ResetCountRepeatSentenceVDto()
                {
                    Update = updateBDto, Count = countRepetitionSentences!.Value
                };
                await _botViewHandler.SendAsync(BaseViewField.ResetCountRepeatSentence, viewDto);
                await _repeatSentenceManager.SaveShowResetCountRepeatSentencesView(updateBDto);
            }
        }

        private async Task RepeatSentenceAsync(UpdateBDto updateBDto)
        {
            try
            {
                var repeatSentenceDto = await _sender.Send(new GetSentenceForRepeatQuery()
                {
                    UserId = updateBDto.GetUserId()
                });

                var viewDto = new RepeatSentenceVDto()
                {
                    Update = updateBDto,
                    SentenceToLearnLabel = repeatSentenceDto.SentenceToLearnLabel,
                    FirstSentence = repeatSentenceDto.FirstSentence,
                    SecondSentence = repeatSentenceDto.SecondSentence
                };
                await _botViewHandler.SendAsync(BaseViewField.RepeatSentence, viewDto);

                await _sender.Send(new RepeatSentenceCommand()
                {
                    UserId = updateBDto.GetUserId(),
                    IsLearn = false,
                    UsingSentencesPairId = repeatSentenceDto.UsingSentencesPairId
                });
            }
            catch (OutOfSentencesToRepeatException)
            {
                await _botViewHandler.SendAsync(BaseViewField.ResetOutOfSentencesToRepeat, updateBDto);
            }
            catch (EmptyCollectionOfSentencesToRepeatException)
            {
                await _botViewHandler.SendAsync(BaseViewField.EmptyCollectionOfSentencesToRepeat, updateBDto);
            }
        }

        private async Task SentencesRepetitionByInputKeyboard(UpdateBDto updateBDto)
        {
            //Оборачиваем отображения пары слов в проверку на кол-во отображонных слов 
            if (await _repeatSentenceManager.CanShowResetCountRepeatSentences(updateBDto))
            {
                await StartSentencesRepetitionByInputAsync(updateBDto);
                await _repeatSentenceManager.ClearShowResetCountRepeatSentencesView(updateBDto);
            }
            else
            {
                var countRepetitionSentences =
                    await _repeatSentenceManager.GetCountRepetitionSentences(updateBDto.GetUserId());
                var viewDto = new ResetCountRepeatSentenceVDto()
                {
                    Update = updateBDto, Count = countRepetitionSentences!.Value
                };
                await _botViewHandler.SendAsync(BaseViewField.ResetCountRepeatSentence, viewDto);

                await _repeatSentenceManager.SaveShowResetCountRepeatSentencesView(updateBDto);
            }
        }

        private async Task StartSentencesRepetitionByInputAsync(UpdateBDto updateBDto)
        {
            try
            {
                //Сохроняет промежуточные данны для сравнения слов между вводами
                var nextSentencePair = await _repeatSentenceManager.GetSentencesPairAndSaveInDataAsync(updateBDto);
                var nextSentence = _repeatSentenceManager.GetNextSentence(nextSentencePair);

                //Отправляет сообщение начало повтора вводом 
                var viewModel =
                    new StartInputVDto() { Update = updateBDto, Sentence = nextSentence };
                await _botViewHandler.SendAsync(SentencesRepetitionByInputViewField.StartInput, viewModel);
                
                //Переход в состояние повтора вводом
                await _botStateTreeUserHandler.SetStateAndActionAsync(updateBDto,
                    SentencesRepetitionByInputField.State,
                    SentencesRepetitionByInputField.Action);
            }
            catch (OutOfSentencesToRepeatException)
            {
                await _botViewHandler.SendAsync(BaseViewField.ResetOutOfSentencesToRepeat, updateBDto);
            }
            catch (EmptyCollectionOfSentencesToRepeatException)
            {
                await _botViewHandler.SendAsync(BaseViewField.EmptyCollectionOfSentencesToRepeat, updateBDto);
            }
        }

        //Сбрасывает счетчик повторения слов в начало
        //Для того чтобы пользователь начал повторение слов с начала
        private async Task ResetCountRepeatSentencesCallback(UpdateBDto updateBDto)
        {
            await _sender.Send(new ResetCountRepeatSentenceCommand() { UserId = updateBDto.GetUserId() });
            await _botViewHandler.SendAsync(BaseViewField.ConfirmResetCountRepeatSentence, updateBDto);
        }

        //открывает настройки по нажатию кнопки на нижней панели
        private async Task SettingsMenuKeyboardAsync(UpdateBDto updateBDto)
        {
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.SettingsMenu, viewDto);
        }

        //калбек на нажатие кнопки изменить режим отображения слов
        private async Task ChangeLearnSentencesModeCallbackAsync(UpdateBDto updateBDto)
        {
            await _sender.Send(new ChangeLearnSentencesModeCommand() { UserId = updateBDto.GetUserId() });
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.EditSettingsMenu, viewDto);
        }

        //калбек на открытие меню изменение пары языков
        private async Task OpenChangeLearnLanguagePairCallbackAsync(UpdateBDto updateBDto)
        {
            var user = await _sender.Send(new GetUserQuery() { UserId = updateBDto.GetUserId() });
            var mainLanguage = user.UserSettings.MainLanguage;
            var learnLanguages = new List<string>()
            {
                LanguageConst.English, LanguageConst.German, LanguageConst.French, LanguageConst.Polish,
            };
            learnLanguages = learnLanguages.Where(x => x != user.UserSettings.LearnLanguage).ToList();

            var learnLanguagesDict = learnLanguages.ToDictionary(x => $"{mainLanguage}|{x}",
                x => $"{CustomConvert.LanguageToEmoji(mainLanguage)}{CustomConvert.LanguageToEmoji(x)}");

            var viewDto = new ChangeLearnLanguagePairCallbackVDto()
            {
                Update = updateBDto, MainLanguage = mainLanguage, LearnLanguageVariants = learnLanguagesDict
            };
            await _botViewHandler.SendAsync(SettingsViewField.OpenChangeLearnLanguagePairCallback, viewDto);
        }

        //калбек на изменение пары языков 
        private async Task ChangeLearnLanguagePairCallbackAsync(UpdateBDto updateBDto)
        {
            var newLearnLanguagePair = updateBDto.CallbackData.Split("|");
            await _sender.Send(new ChangeLanguagePairCommand()
            {
                UserId = updateBDto.GetUserId(),
                MainLanguage = newLearnLanguagePair[0],
                LearnLanguage = newLearnLanguagePair[1]
            });
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.EditSettingsMenu, viewDto);
        }

        private async Task OpenChangeTimeZoneCallbackAsync(UpdateBDto updateBDto)
        {
            var user = await _sender.Send(new GetUserQuery() { UserId = updateBDto.GetUserId() });
            var currentTimeZone = CustomConvert.IntToUTC(user.UserSettings.TimeZone);

            var timeZonesDict = TimeZoneForDayConst.TimeZone
                .ToDictionary(x => x.ToString(), x => CustomConvert.IntToUTC(x));

            var viewDto = new ChangeTimeZoneCallbackVDto()
            {
                Update = updateBDto, CurrentTimeZone = currentTimeZone, TimeZoneVariants = timeZonesDict
            };

            await _botViewHandler.SendAsync(SettingsViewField.OpenChangeTimeZoneCallback, viewDto);
        }

        private async Task ChangeTimeZoneCallbackAsync(UpdateBDto updateBDto)
        {
            var newTimeZone = Convert.ToInt32(updateBDto.CallbackData);
            await _sender.Send(new ChangeTimeZoneCommand() { UserId = updateBDto.GetUserId(), TimeZone = newTimeZone });
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.EditSettingsMenu, viewDto);
        }

        private async Task OpenChangeTimesForDayCallback(UpdateBDto updateBDto)
        {
            var user = await _sender.Send(new GetUserQuery() { UserId = updateBDto.GetUserId() });
            var currentTimesForDay =
                CustomConvert.TimesForDayToViewText(user.UserSettings.SentencesRepeatForDayTimesModeEnum);

            var timesForDayArray = new int[]
            {
                (int)SentencesRepeatForDayTimesModeEnum.TurnOff, (int)SentencesRepeatForDayTimesModeEnum.Times1InDay,
                (int)SentencesRepeatForDayTimesModeEnum.Times3InDay, (int)SentencesRepeatForDayTimesModeEnum.Times5InDay,
                (int)SentencesRepeatForDayTimesModeEnum.Times10InDay
            };

            var timesForDayDict = timesForDayArray
                .ToDictionary(x => x.ToString(),
                    x => CustomConvert.TimesForDayToViewText((SentencesRepeatForDayTimesModeEnum)x));


            var viewDto = new ChangeTimesForDayCallbackVDto()
            {
                Update = updateBDto, CurrentTimesForDay = currentTimesForDay, TimesForDayVariants = timesForDayDict
            };

            await _botViewHandler.SendAsync(SettingsViewField.OpenChangeTimesForDayCallback, viewDto);
        }

        private async Task ChangeTimesForDayCallback(UpdateBDto updateBDto)
        {
            var newTimeZone = (SentencesRepeatForDayTimesModeEnum)Convert.ToInt32(updateBDto.CallbackData);
            await _sender.Send(new ChangeSentencesRepeatForDayModeCommand()
            {
                UserId = updateBDto.GetUserId(), SentencesRepeatForDayTimesModeEnum = newTimeZone
            });
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.EditSettingsMenu, viewDto);
        }


        private async Task BackToSettingsMenuCallbackAsync(UpdateBDto updateBDto)
        {
            var viewDto = await _menuSettingsManager.CreateSettingsMenuVDto(updateBDto);
            await _botViewHandler.SendAsync(SettingsViewField.EditSettingsMenu, viewDto);
        }

        private async Task OnInputFeedbackAsync(UpdateBDto updateBDto)
        {
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, BaseField.InputFeedbackAction);
            await _botViewHandler.SendAsync(SettingsViewField.StartInputFeedback, updateBDto);
        }

        private async Task InputFeedbackAction(UpdateBDto updateBDto)
        {
            var text = updateBDto.GetMessage().Text;
            await _sender.Send(new SendFeedbackCommand() { UserId = updateBDto.GetUserId(), Text = text! });
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, BaseField.BaseAction);
            await _botViewHandler.SendAsync(SettingsViewField.SendFeedback, updateBDto);
        }

        private async Task OnCancelInputFeedbackAsync(UpdateBDto updateBDto)
        {
            await _botStateTreeUserHandler.SetActionAsync(updateBDto, BaseField.BaseAction);
            await _botViewHandler.SendAsync(BaseViewField.Menu, updateBDto);
        }
    }
}
