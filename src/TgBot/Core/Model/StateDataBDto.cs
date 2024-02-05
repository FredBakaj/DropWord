namespace DropWord.TgBot.Core.Model;

public class StateDataBDto<T> where T : class
{
    public string DataType { get; set; } = null!;
    public T Data { get; set; } = null!;
}
