using System.ClientModel;
using Azure.AI.OpenAI;
using DropWord.Application.Common.Interfaces;
using DropWord.Application.Manager.Ai;
using DropWord.Domain.Constants;
using DropWord.Infrastructure.Ai;
using DropWord.Infrastructure.Data;
using DropWord.Infrastructure.Data.Interceptors;
using DropWord.Infrastructure.DI.Manager;
using DropWord.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DropWord.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddScoped<IAiManager, AiManager>();
        services.AddScoped<AzureOpenAIClient>(p => new AzureOpenAIClient(
            new Uri(p.GetService<IConfiguration>()?.GetSection("CommonSettings")["AiEndpoint"] ??
                    throw new InvalidOperationException()),
            new ApiKeyCredential(p.GetService<IConfiguration>()?.GetSection("CommonSettings")["AiApiKey"] ??
                                 throw new InvalidOperationException())));

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        ManagerBuild.BuildService(services);

        return services;
    }
}
