using System;
using System.IO;
using Exiled.API.Features;
using SCPSLAudioApi;

namespace LobbyMusicPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "LobbyMusicPlugin";
        public override string Author { get; } = "Hanbin-GW";
        public override Version Version { get; } = new Version(0, 0, 1);
        public static Plugin Instance { get; private set; }
        private string _audioDirectory;
        private bool isMusicPlaying = false;

        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _audioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
        }
    }
}