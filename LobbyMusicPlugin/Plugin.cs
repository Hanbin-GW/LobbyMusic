using System;
using System.Collections.Generic;
using AdminToys;
using Exiled.API.Features;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;

namespace LobbyMusicPlugin
{
    public class LobbyMusic : Plugin<Config>
    {
        public override string Name => "LobbyMusic";
        public override string Author => "YourName";
        public override Version Version { get; } = new Version(1, 0, 0);
        
        private void OnWaitingForPlayers()
        {
            SpeakerManager.Instance.EnsureMusicDirectoryExists();
            //PlayLobbyMusic();
            if (Server.PlayerCount == 0)
            {
                SpeakerManager.Instance.PlayMusicToPlayer("77.ogg");
            }
        }
        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            base.OnDisabled();
        }
    }
}