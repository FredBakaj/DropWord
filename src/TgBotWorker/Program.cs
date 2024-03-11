using DropWord.Application;
using DropWord.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder();
host.ConfigureFunctionsWorkerDefaults();
    host.ConfigureServices(services =>
    {
        services.AddApplicationServices();
        services.AddInfrastructureServices(services.BuildServiceProvider().GetRequiredService<IConfiguration>());
    });

var app = host.Build();

app.Run();
