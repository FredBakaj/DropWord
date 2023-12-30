using System.Reflection;
using DropWord.TgBot.Core.Attribute;

namespace DropWord.TgBot.Core.Src.View;

public class ABotView : IBotView
{
    /// <summary>
    /// Возвращает все методы которые имеют атрибут ViewAttribute в обектах которые наследуют AViewItem
    /// </summary>
    /// <returns></returns>
    public Task<Dictionary<string, Func<object[], Task>>> GetMethodsAsync()
    {
        Type thisClass = GetType();
        var result = new Dictionary<string, Func<object[], Task>>();
        foreach (var method in thisClass.GetMethods())
        {
            var methodAttributes = method.GetCustomAttributes(typeof(BotViewAttribute), false);
            if (methodAttributes.Length > 0)
            {
                ParameterInfo[] parameters = method.GetParameters();
                string nameView = ((BotViewAttribute)methodAttributes[0]).NameView;
                Func<object[], Task> methodDelegate = async (args) =>
                {
                    if (parameters.Length == args.Length)
                    {
                        object[] typedArgs = new object[args.Length];
                        for (int i = 0; i < args.Length; i++)
                        {
                            typedArgs[i] = Convert.ChangeType(args[i], parameters[i].ParameterType);
                        }

                        await (Task)method.Invoke(this, typedArgs)!;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("parameters");
                    }
                };
                result.Add(nameView, methodDelegate);
            }
        }

        return Task.FromResult(result);
    }
}
