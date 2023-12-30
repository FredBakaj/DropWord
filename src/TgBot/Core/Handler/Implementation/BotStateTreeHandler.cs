using DropWord.TgBot.Core.Model;

namespace DropWord.TgBot.Core.Handler.Implementation
{
    public class BotStateTreeHandler : IBotStateTreeHandler
    {
        protected readonly Dictionary<string, Func<UpdateBDto, Task>> _actions = new();
        protected readonly Dictionary<string, Dictionary<string, Func<UpdateBDto, Task>>?> _callbacks = new();
        private readonly Dictionary<string, Dictionary<string, Func<UpdateBDto, Task>>?> _keyboards = new();


        public async Task ExecuteRoute(UpdateBDto updateBDto)
        {
            var message = updateBDto.Message;
            var callback = updateBDto.CallbackQuery;
            if (message != null)
            {
                try
                {
                    if (updateBDto.TelegramState != null)
                    {
                        if (message.Text != null)
                        {
                            await _keyboards[updateBDto.TelegramState.Action]?[message.Text].Invoke(updateBDto)!;
                        }
                    }
                }
                catch (KeyNotFoundException)
                {
                    if (updateBDto.TelegramState != null)
                    {
                        await _actions[updateBDto.TelegramState.Action].Invoke(updateBDto);
                    }
                }
            }
            else if (callback != null)
            {
                if (callback.Data != null && callback.Data.Contains(":"))
                {
                    var dataParts = callback.Data.Split(':');
                    if (dataParts.Length > 2)
                        throw new ArgumentOutOfRangeException("more than one argument was passed to callback");
                    updateBDto.CallbackData = dataParts[1];
                    if (updateBDto.TelegramState != null)
                    {
                        await _callbacks[updateBDto.TelegramState.Action]?[dataParts[0]].Invoke(updateBDto)!;
                    }
                }
                else
                {
                    try
                    {
                        if (updateBDto.TelegramState != null)
                        {
                            if (callback.Data != null)
                            {
                                await _callbacks[updateBDto.TelegramState.Action]?[callback.Data].Invoke(updateBDto)!;
                            }
                        }
                    }
                    catch (KeyNotFoundException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        public void AddAction(string action, Func<UpdateBDto, Task> func)
        {
            _actions.Add(action, func);
        }
        public void AddCallback(string action, string callback, Func<UpdateBDto, Task> func)
        {
            if (_callbacks.TryGetValue(action, out Dictionary<string, Func<UpdateBDto, Task>>? callbackButtons))
            {
                callbackButtons?.Add(callback, func);
            }
            else
            {
                _callbacks.Add(action, new Dictionary<string, Func<UpdateBDto, Task>>());
                _callbacks[action]?.Add(callback, func);
            }
        }

        public void AddKeyboard(string action, string keyboard, Func<UpdateBDto, Task> func)
        {
            if (_keyboards.TryGetValue(action, out Dictionary<string, Func<UpdateBDto, Task>>? keyboardsButtons))
            {
                keyboardsButtons?.Add(keyboard, func);
            }
            else
            {
                _keyboards.Add(action, new Dictionary<string, Func<UpdateBDto, Task>>());
                _keyboards[action]?.Add(keyboard, func);
            }
        }
    }
}
