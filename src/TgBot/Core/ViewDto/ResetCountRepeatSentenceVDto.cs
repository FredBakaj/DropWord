namespace DropWord.TgBot.Core.ViewDto;

public class ResetCountRepeatSentenceVDto : BaseVDto
{
    //количество слов которые пользователь повторил без сброса
    public int Count { get; set; }
}
