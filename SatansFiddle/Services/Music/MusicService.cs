using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;

namespace SatansFiddle.Services.Music
{
    public class MusicService : IMusicService
    {
        public Process CreateStream(string url)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"yt-dlp -o - {url} | ffmpeg -i pipe: -hide_banner -loglevel panic -ac 2 -f s16le -ar 48000 pipe:1\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            };
            return Process.Start(startInfo);
        }

        [Command("join", RunMode = RunMode.Async)]
        public async Task PlaySong(SocketVoiceChannel voiceChannel, string url)
        {
            var client = await voiceChannel.ConnectAsync();
            await this.SendAsync(client, url);
            await voiceChannel.DisconnectAsync();
        }

        public async Task SendAsync(IAudioClient client, string url)
        {
            using (var ffmpeg = CreateStream(url))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }

        public async Task StopSong(SocketVoiceChannel voiceChannel)
        {
            await voiceChannel.DisconnectAsync();   
        }
    }
}
