using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SatansFiddle.Services.Message;
using SatansFiddle.Services.Music;
using SelfHostBot.Server.Bot;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);
IConfiguration configuration = builder.Build();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
        services.AddHostedService<Bot>()
        .AddSingleton<Bot>()
        .AddScoped<IMessageService, MessageService>()
        .AddScoped<IMusicService, MusicService>()
        .AddSingleton<IConfiguration>(configuration)
        .AddLogging())
    .Build();



await host.RunAsync();


