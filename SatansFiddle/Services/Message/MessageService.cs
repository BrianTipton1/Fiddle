using Discord.WebSocket;
using SatansFiddle.Services.Music;
using System.Text.RegularExpressions;

namespace SatansFiddle.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMusicService musicService;

        public MessageService(IMusicService musicService)
        {
            this.musicService = musicService ?? throw new ArgumentNullException(nameof(musicService));
        }

        public string GetLink(string url)
        {
            Match match = Regex.Match(url, "^.*((youtu.be\\/)|(v\\/)|(\\/u\\/\\w\\/)|(embed\\/)|(watch\\?))\\??v?=?([^#&?]*).*");
            return match.Groups[7].Value;
        }

        public SocketVoiceChannel? GetVoiceChannel(SocketMessage message)
        {
            var guilds = message.Author.MutualGuilds;
            var guild = guilds.FirstOrDefault(guild => guild.TextChannels.Contains(message.Channel));
            foreach (SocketVoiceChannel voicechannel in guild.VoiceChannels)
            {
                if (voicechannel != null)
                {
                    if (voicechannel.ConnectedUsers.Contains(message.Author))
                    {
                        return voicechannel;
                    }
                }
            }
            return null;
        }
        public async Task HandleMessage(SocketMessage message)
        {
            if (message.Content != null && message.Content.StartsWith('$'))
            {
                if (HasVideo(message.Content))
                {
                    string link = GetLink(message.Content);
                    var voicechannel = GetVoiceChannel(message);
                    if (voicechannel != null)
                    {
                        await musicService.PlaySong(voicechannel, link);
                        Console.WriteLine($"This garbage was played {link}");
                        return;
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("You need to be in a voice channel to play audio");
                        return;
                    }
                }
                if (message.Content.Contains("stop"))
                {
                    var voicechannel = GetVoiceChannel(message);
                    if (voicechannel != null)
                    {
                        await musicService.StopSong(voicechannel);
                        return;
                    }
                    else
                    {
                        await message.Channel.SendMessageAsync("You need to be in a voice channel to stop audio");
                        return;
                    }
                }
                else
                {
                    await message.Channel.SendMessageAsync($"Your link below seems to be malformed, please check" +
                        $" and try again\n```{message.Content.Remove(0, 1)}``` If problem persists you might need to check the regex for youtube videos. (See MessageService.cs)");
                }
            }
        }

        public bool HasVideo(string url)
        {
            return Regex.IsMatch(url, "^.*((youtu.be\\/)|(v\\/)|(\\/u\\/\\w\\/)|(embed\\/)|(watch\\?))\\??v?=?([^#&?]*).*");
        }
    }
}
