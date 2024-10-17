using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using SCPSLAudioApi;
using SCPSLAudioApi.AudioCore;

namespace LobbyMusicPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "LobbyMusicPlugin";
        public override string Author { get; } = "Hanbin-GW";
        public override Version Version { get; } = new Version(0, 3, 0);
        public static Plugin Instance { get; private set; }
        private readonly string _audioDirectory;
        private bool _isMusicPlaying = false;
        public AudioPlayerBase _sharedAudioPlayer;


        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _audioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }

        public override void OnEnabled()
        {
            //Log.Info("You can donate the Ghost server and get a extended version!");
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            //Exiled.Events.Handlers.Player.Verified += OnPlayerVerified;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Log.SendRaw("[AudioPlugin] Path: " + _audioDirectory,ConsoleColor.DarkGreen);
            Log.SendRaw("[AudioPlugin] Custom Lobby Music Plugin Enabled",ConsoleColor.DarkGreen);
            Log.Info("-----------------------------------------");
            Log.Info("|  Thanks for using Hanbin-GW's Plugin  |");
            Log.Info("-----------------------------------------");
            //Log.Info("You can donate the Ghost server and get a extended version!");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            //Log.Info("Thanks for using Hanbin-GW's Plugin.");
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            //Log.SendRaw("[AudioPlugin] Path: " + _audioDirectory,ConsoleColor.DarkGreen);
            base.OnDisabled();
        }

        private void OnRoundStarted()
        {
            StopLobbyMusic();
        }

        private void OnPlayerLeft(LeftEventArgs ev)
        {
            if (Server.PlayerCount == 0)
            {
                StopLobbyMusic();
            }
        }


        // 대기 로비 상태에서 호출되는 이벤트 핸들러
        private void OnWaitingForPlayers()
        {
            EnsureMusicDirectoryExists();
            PlayLobbyMusic();
        }

        
        private void PlayLobbyMusic()
        {
            
            if (_sharedAudioPlayer == null)
            {
                Log.Info("reset sharedAudioPlayer...");
                // AudioPlayerBase 객체를 초기화
                _sharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);

                // 초기화에 실패한 경우 오류 메시지 출력
                if (_sharedAudioPlayer == null)
                {
                    Log.Error("failed to reset sharedAudioPlayer. stop the music play...");
                    return;
                }
            }

            if (_isMusicPlaying)
            { 
                Log.Info("The music is already playing..."); 
                return;
            }
            
            if(Config.LoopSingleSong)
            {
                //audioPlayer.CurrentPlay = Path.Combine(audioDirectory, Config.SingleSongPath); 
                _sharedAudioPlayer.CurrentPlay = Path.Combine(_audioDirectory, Config.SingleSongName); 
                string songPath = Path.Combine(_audioDirectory, Config.SingleSongName); 
                //string songPath = Config.TempSingleSongPath;
                if (!File.Exists(songPath)) 
                { 
                    Log.Error($"Cannot find audio file: {songPath}"); 
                    return;
                }
                _sharedAudioPlayer.Loop = true; 
                _sharedAudioPlayer.Volume = Config.StandardVolume;
                _sharedAudioPlayer.Play(-1);
                _isMusicPlaying = true;
                Log.Info($"Music Path: {_sharedAudioPlayer.CurrentPlay}"); 
                Map.Broadcast(5,$"playing...{Config.LoopSingleSong}");
            }
            else if (Config.LoopSingleSong == false) 
            {
                foreach (string songPath in Config.QueueSongs) 
                { 
                    _sharedAudioPlayer.Enqueue(songPath, 0);
                }

                _sharedAudioPlayer.Loop = false; 
                _sharedAudioPlayer.Volume = Config.StandardVolume;
                _sharedAudioPlayer.Play(0); 
                _isMusicPlaying = true;
                BroadcastFileNameToPlayers(Config.QueueSongs[0]);
            }
        
        }
        
        private void BroadcastFileNameToPlayers(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            Map.Broadcast(5, $"<size=30><color=aqua>Now Playing: {fileName}</color></size>");
        }
        
        public void StopLobbyMusic()
        {
            if (_sharedAudioPlayer != null && _isMusicPlaying == true)
            {
                //audioSource.Stop();
                _sharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                _sharedAudioPlayer.Loop = false;
                _sharedAudioPlayer.Volume = Config.StandardVolume;
                _sharedAudioPlayer.Stoptrack(true);
                _isMusicPlaying = false;
                Log.SendRaw("Stopping music...", ConsoleColor.DarkRed);
            }
            else
            {
                Log.Error("The music is not playing..");
            }

        }

        public string ListMusicFiles()
        {
            List<string> stringBuilder = new List<string>();
            if (Directory.Exists(_audioDirectory))
            {
                string[] musicFiles = Directory.GetFiles(_audioDirectory, "*.ogg");  // .ogg 파일만 가져옴
                if (musicFiles.Length > 0)
                {
                    foreach (string file in musicFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        stringBuilder.Add(fileName);
                    }
                }
                else
                {
                    Log.Warn("There has no music file in folder!");
                }
            }
            else
            {
                Log.Error($"Cannot find the music path: {_audioDirectory}.");
                EnsureMusicDirectoryExists();
            }
            string finalFileList = string.Join("\n", stringBuilder);
            return finalFileList;
        }
        
        public void PlaySpecificMusic(string filepath)
        {
            if (_sharedAudioPlayer == null)
            {
                _sharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                if (_sharedAudioPlayer == null)
                {
                    Log.Error("Failed to reset sharedAudioPlayer. You cannot play music.");
                    return;
                }
            }

            if (!File.Exists(filepath))
            {
                Log.Error($"Cannot find audio path: {filepath}");
                return;
            }
            
            StopLobbyMusic();
            
            _sharedAudioPlayer.CurrentPlay = filepath;
            _sharedAudioPlayer.Loop = false;  // 특정 곡은 반복하지 않음
            _isMusicPlaying = true;
            _sharedAudioPlayer.Volume = Config.StandardVolume;
            _sharedAudioPlayer.Play(-1);
            
            Log.Info($"Specific music is playing: {filepath}");
        }

        public void SetMusicVolume(float volume)
        {
            if (_sharedAudioPlayer == null)
            {
                _sharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                if (_sharedAudioPlayer == null)
                {
                    Log.Error("Failed to reset sharedAudioPlayer. You cannot play music.");
                    return;
                }
            }
            if (_sharedAudioPlayer != null)
            {
                _sharedAudioPlayer.Volume = volume;  // 볼륨 설정 (0.0 ~ 1.0)
                Log.Info($"The volume set {volume * 100}%");
            }
            else
            {
                Log.Warn("The music is not playing... You cannot set volume");
            }
        }

        private void EnsureMusicDirectoryExists()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED",
                "Plugins", "audio");

            if (!Directory.Exists(path))
            {
                Log.Warn($"Music folder doesn't exists. create a new: {path}.");
                Directory.CreateDirectory(path);
            }
            else
            {
                Log.Info("The music folder is already exists.");
            }
        }
    }
}