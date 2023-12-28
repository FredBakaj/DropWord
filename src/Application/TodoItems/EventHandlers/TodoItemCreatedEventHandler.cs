using DropWord.Domain.Events;
using Microsoft.Extensions.Logging;

namespace DropWord.Application.TodoItems.EventHandlers;

public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    private readonly ILogger<TodoItemCreatedEventHandler> _logger;

    public TodoItemCreatedEventHandler(ILogger<TodoItemCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DropWord Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
