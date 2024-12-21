using System.ComponentModel;
using Exiled.API.Interfaces;

namespace LobbyMusicPlugin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        [Description("Choose a loop specific song")]
        public string Path { get; set; } = "77.opus";
    }
}
