namespace DropWord.TgBot.Core.Model;

public class StateTempDataBDto<T> where T : class
{
    public string DataType { get; set; } = null!;
    public T Data { get; set; } = null!;
}
