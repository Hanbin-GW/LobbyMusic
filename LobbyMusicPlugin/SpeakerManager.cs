using System;
using System.Collections.Generic;
using System.IO;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Mirror;
using PlayerRoles.FirstPersonControl;
using UnityEngine;
using UserSettings.ServerSpecific;
using VoiceChat;
using VoiceChat.Networking;
using Object = UnityEngine.Object;

namespace LobbyMusicPlugin
{
    public class SpeakerManager
    {
        private Config _config;
        private SpeakerToy _speakerToy;
        private readonly string _audioDirectory;
        private bool _isMusicPlaying = false;
        public static SpeakerManager Instance { get; private set; }

        public SpeakerManager()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _audioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }
        
        public void PlayMusicToPlayer(string filePath)
        {
            EnsureMusicDirectoryExists();
            if (!File.Exists(filePath))
            {
                Log.Error($"음악 파일을 찾을 수 없습니다: {filePath}");
                return;
            }
            byte[] audioData = File.ReadAllBytes(filePath);
            Vector3 lobbyCenter = new Vector3(0, 1, 0);
            _speakerToy = Object.Instantiate(PrefabHelper.GetPrefab<SpeakerToy>(PrefabType.SpeakerToy), lobbyCenter, Quaternion.identity);
            _speakerToy.NetworkMinDistance = 0f;
            _speakerToy.NetworkMaxDistance = 100f;
            NetworkServer.Spawn(_speakerToy.gameObject);
            
            AudioMessage audioMessage = new AudioMessage(_speakerToy.ControllerId, audioData, audioData.Length);
            foreach (Player player in Player.List)
            {
                if (player.ReferenceHub.connectionToClient != null)
                {
                    player.ReferenceHub.connectionToClient.Send(audioMessage);
                }
            }
        }
        
        public void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");

            // 폴더가 없으면 생성
            if (!Directory.Exists(path))
            {
                Log.Warn($"The music folder is not exists. create new: {path}");
                Directory.CreateDirectory(path);  // 폴더 생성
            }
            else
            {
                Log.Info("The Music folder is exists.");
            }
        }

    }
}