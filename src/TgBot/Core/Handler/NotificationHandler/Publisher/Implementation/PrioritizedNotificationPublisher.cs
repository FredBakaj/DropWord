using System.Reflection;
using DropWord.TgBot.Core.Handler.NotificationHandler.Notification;
using MediatR;

namespace DropWord.TgBot.Core.Handler.NotificationHandler.Publisher.Implementation;

public class PrioritizedNotificationPublisher : IPublisher
{
    private readonly IServiceProvider _serviceProvider;

    public PrioritizedNotificationPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Publish(object notification, CancellationToken cancellationToken = new CancellationToken())
    {
        if (notification == null)
            throw new ArgumentNullException(nameof(notification));

        // Получаем тип уведомления
        var notificationType = notification.GetType();

        // Используем рефлексию для вызова метода Publish<TNotification>
        var method = typeof(IPublisher)
            .GetMethod(nameof(Publish), BindingFlags.Instance | BindingFlags.Public)
            ?.MakeGenericMethod(notificationType);

        // Вызываем метод Publish<TNotification> с правильным типом
        await ((Task)method!.Invoke(this, new object[] { notification, cancellationToken })!)!;
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) 
        where TNotification : INotification
    {
        // Получаем все зарегистрированные обработчики для события
        var handlers = _serviceProvider
            .GetServices<INotificationHandler<TNotification>>()
            .Select(handler => new
            {
                Handler = handler,
                Priority = (handler as IHasPriority)?.Priority ?? int.MaxValue // Приоритет по умолчанию
            })
            .OrderBy(h => h.Priority) // Сортируем по приоритету
            .Select(h => h.Handler)
            .ToList();

        // Вызываем обработчики по порядку
        foreach (var handler in handlers)
        {
            await handler.Handle(notification, cancellationToken);
        }
    }
}

