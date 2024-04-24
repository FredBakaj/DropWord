using DropWord.Infrastructure.Data;
using DropWord.TgBot.Di;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Logging.AddConfiguration(builder.Configuration);
builder.Logging.AddAzureWebAppDiagnostics();
builder.Configuration.AddEnvironmentVariables(prefix: "DropWord_");
ServicesBuild.BuildService(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging() || app.Environment.IsProduction())
{
    await app.InitialiseDatabaseAsync();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
