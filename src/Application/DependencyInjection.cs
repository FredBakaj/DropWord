using System.Reflection;
using DropWord.Application.Common.Behaviours;
using DropWord.Application.DI.Factory;
using DropWord.Application.DI.Manager;
using DropWord.Application.DI.Strategy;
using Microsoft.Extensions.DependencyInjection;

namespace DropWord.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            // cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        });
        
        FactoryBuild.BuildService(services);
        StrategyBuild.BuildService(services);
        ManagerBuild.BuildService(services);

        return services;
    }
}
