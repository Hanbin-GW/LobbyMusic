using System.ComponentModel;
using Exiled.API.Interfaces;

namespace LobbyMusicPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("Enable loop the one specific song?")]
        public bool LoopSingleSong { get; set; } = true;
        [Description("Choose a loop specific song")]
        public string SingleSongName { get; set; } = "77.opus";
        [Description("If the song is not loop, the song will be play in this list")]
        public string[] QueueSongs { get; set; } = { "Example1.ogg", "Example2.ogg" };
        [Description("You can set a standard volume when music is playing.")]
        public float StandardVolume { get; set; } = 70f;

        [Description("Play music without player?")]
        public bool IsPlayMusicNoPerson { get; set; } = false;
    }
}
