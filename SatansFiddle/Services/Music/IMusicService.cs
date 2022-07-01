using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SatansFiddle.Services.Music
{
    public interface IMusicService
    {
        public Process CreateStream(string url);
        public Task SendAsync(IAudioClient client, string url);
        public Task PlaySong(SocketVoiceChannel voiceChannel, string url);
        public Task StopSong(SocketVoiceChannel voiceChannel);

    }
}
