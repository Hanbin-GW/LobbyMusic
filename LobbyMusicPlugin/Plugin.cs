using System;
using System.IO;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using PlayerRoles;
using UnityEngine;
using VoiceChat.Networking;
using VoiceChat.Playbacks;

namespace LobbyMusicPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "Lobby Music - 14.0";
        public override string Author { get; } = "Hanbin - GW";
        public override Version Version { get; } = new Version(0, 2, 3);
        public bool IsMusicPlaying = false;
        private Speaker _speaker;
        private readonly string _audioDirectory;
        public static Plugin Instance { get; private set; }
        
        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _audioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }


        public override void OnEnabled()
        {
            if (Server.IpAddress == "121.166.155.25" && Server.Port == 7779)
            {
                Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
                Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingPlayers;
                base.OnEnabled();
            }
            else
            {
                Log.Error("THIS PLUGIN IS NOT ALLOW TO USE");
            }
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingPlayers;
            base.OnDisabled();
        }

        private void OnWaitingPlayers()
        {
            EnsureMusicDirectoryExists();
            Vector3 position = new Vector3(0, 5, 0);
            Vector3 rotation = new Vector3(0, 90, 0);
            Vector3 scale = new Vector3(1, 1, 1);
            string songPath = Path.Combine(_audioDirectory, Config.Path); 
            _speaker = Speaker.Create(position, rotation, scale, spawn: true);
            _speaker.IsSpatial = false;
            _speaker.Base.Playback.Source.clip = Resources.Load<AudioClip>(songPath);
            _speaker.Base.Playback.Source.Play();
        }

        private void OnRoundStarted()
        {
            if (_speaker != null)
            {
                _speaker.Base.Playback.Source.Stop();
                _speaker.Destroy();
            }
            RoleTypeId npcRole = RoleTypeId.Scp049;
            Npc npc = Npc.Spawn("TestNPC", npcRole, new Vector3(0, 0, 0));
            Log.Info($"NPC {npc.Nickname}가 생성되었습니다.");
            npc.Teleport(RoomType.LczClassDSpawn);
            npc.LateDestroy(110f);
        }
        
        private void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");

            // 폴더가 없으면 생성
            if (!Directory.Exists(path))
            {
                Log.Warn($"음악 폴더가 존재하지 않습니다. 새로 생성합니다: {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("음악 폴더가 이미 존재합니다.");
            }
        }
    }
}