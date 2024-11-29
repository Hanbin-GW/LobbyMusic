using System;
using System.Collections.Generic;
using System.IO;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs;
using UnityEngine;
using UnityEngine.Playables;
using Log = Exiled.API.Features.Log;

namespace LobbyMusicPlugin
{
    public class LobbyMusic : Plugin<Config>
    {
        public override string Name => "LobbyMusic";
        public override string Author => "YourName";
        public override Version Version { get; } = new Version(0, 5, 0);
        private readonly string _audioDirectory;
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

        private void OnWaitingForPlayers()
        {
            SpeakerToy speakerToy = new SpeakerToy();
            //speakerToy.Playback(_audioDirectory);
            speakerToy.Volume = 50f;
        }
    }
}