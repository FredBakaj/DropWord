namespace DropWord.TgBot.Core.Attribute;

[AttributeUsage(AttributeTargets.Method)]
public class BotViewAttribute : System.Attribute
{
    public string NameView { get; }

    public BotViewAttribute(string nameView)
    {
        NameView = nameView;
    }
}
