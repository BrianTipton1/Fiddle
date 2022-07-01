
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SatansFiddle.Services.Message;

namespace SelfHostBot.Server.Bot
{
    public class Bot : IHostedService
    {
        private DiscordSocketClient? _client;
        private readonly IConfiguration config;
        private readonly IMessageService messageService;

        public Bot(IConfiguration config, IMessageService messageService)
        {
            this.config = config ?? throw new ArgumentNullException(nameof(config));
            this.messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        }
        public void Initialize()
        {

            if (this._client == null)
            {
                return;
            }
            _client.MessageReceived += MessageReceived;
            //_client.Ready += Ready;
            _client.Disconnected += Disconnected;
            _client.Connected += Ready;
        }
        private Task Ready()
        {
            Console.WriteLine($"Bot connected at {DateTime.Now}");
            return Task.CompletedTask;
        }
        private Task Disconnected(Exception e)
        {
            Console.WriteLine($"Bot disconnected at {DateTime.Now} :(");
            return Task.CompletedTask;
        }
        public Task MessageReceived(SocketMessage message)
        {
            messageService.HandleMessage(message);
            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var conf = new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                GatewayIntents =
                   GatewayIntents.All
            };

            this._client = new DiscordSocketClient(conf);
            await this._client.LoginAsync(Discord.TokenType.Bot, config["Discord:Token"]);
            await this._client.StartAsync();
            Initialize();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_client != null)
            {
                await _client.LogoutAsync();
            }
        }
    }
}
