using System.Reflection;
using DropWord.Application;
using DropWord.Application.DI.Strategy;
using DropWord.Infrastructure;
using DropWord.TgBot.Core.Handler.NotificationHandler.Publisher.Implementation;
using DropWord.TgBot.Core.Service;
using DropWord.TgBot.Core.Service.Implementation;
using DropWord.TgBot.Di.Command;
using DropWord.TgBot.Di.Controller;
using DropWord.TgBot.Di.Factory;
using DropWord.TgBot.Di.Handler;
using DropWord.TgBot.Di.Manager;
using DropWord.TgBot.Di.Middleware;
using DropWord.TgBot.Di.View;
using DropWord.TgBot.Mapping;
using MediatR;
using Telegram.Bot;

namespace DropWord.TgBot.Di
{
    public class ServicesBuild
    {
        public static void BuildService(WebApplicationBuilder builder)
        {
            IServiceCollection services = builder.Services;
            IConfiguration configuration = builder.Configuration;

            services.AddSingleton<ITelegramBotClient, TelegramBotClient>(
                provider => new TelegramBotClient(
                    provider.GetService<IConfiguration>()?.GetSection("CommonSettings")["BotToken"] ??
                    throw new InvalidOperationException()));

            //Background service
            services.AddScoped<IUpdateHandler, UpdateHandler>();
            if (Convert.ToBoolean(configuration.GetSection("CommonSettings")["IsWebHook"]))
            {
                services.AddHostedService<ConfigureWebhook>();
                services.AddHostedService<ResetWebhookService>();
                services.ConfigureTelegramBotMvc();
            }
            else
            {
                services.AddScoped<ReceiverService>();
                services.AddHostedService<PollingService>();
            }

            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);

            //Asp service
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddAutoMapper(typeof(MapperProfile));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddSingleton<IPublisher, PrioritizedNotificationPublisher>();

            //Custom service
            services.AddScoped<ILogger, Logger<ServicesBuild>>();
            ManagerBuild.BuildService(services);
            MiddlewareBuild.BuildService(services);
            ControllerBuild.BuildService(services);
            ViewBuild.BuildService(services);
            ViewComponentBuild.BuildService(services);
            CommandBuild.BuildService(services);
            FactoryBuild.BuildService(services);
            StrategyBuild.BuildService(services);
            HandlerBuild.BuildService(services);
        }
    }
}
