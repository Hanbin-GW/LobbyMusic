using Exiled.API.Interfaces;

namespace LobbyMusicPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public bool LoopSingleSong { get; set; } = true;
        public string SingleSongPath { get; set; } = "77.ogg";
        public string[] QueueSongs { get; set; } = { "Example1.ogg", "Example2.ogg" };

    }
}