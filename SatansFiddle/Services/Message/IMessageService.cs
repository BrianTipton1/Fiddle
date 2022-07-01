using Discord.WebSocket;

namespace SatansFiddle.Services.Message
{
    public interface IMessageService
    {
        public Task HandleMessage(SocketMessage message);
        public SocketVoiceChannel? GetVoiceChannel(SocketMessage message);
        public string GetLink(string url);
        public bool HasVideo(string url);
    }
}
