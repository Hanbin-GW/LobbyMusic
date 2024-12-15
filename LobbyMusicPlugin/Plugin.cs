using Exiled.API.Features;
using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using System.IO;
using SCPSLAudioApi.AudioCore;

namespace LobbyMusicPlugin
{
    public class Plugin : Plugin<Config>
    {
        public override string Name { get; } = "LobbyMusicPlugin";
        public override string Author { get; } = "Hanbin-GW";
        public override Version Version { get; } = new Version(0, 5, 2);
        public AudioPlayerBase SharedAudioPlayer;
        public override PluginPriority Priority => PluginPriority.Default;
        public static Plugin Instance { get; private set; }
        private readonly string _audioDirectory;
        private bool _isMusicPlaying = false;

        public Plugin()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _audioDirectory = Path.Combine(appDataPath, "EXILED", "Plugins", "audio");
        }

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeft;
            Exiled.Events.Handlers.Player.Verified += OnPlayerVerified;
            Log.SendRaw(_audioDirectory,ConsoleColor.Green);
            Log.SendRaw("[AudioPlugin] Custom Music Plugin Active",ConsoleColor.DarkYellow);
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeft;
            Exiled.Events.Handlers.Player.Verified -= OnPlayerVerified;
            Instance = null;
            Log.SendRaw("[AudioPlugin] Custom Lobby Music Plugin Disabled", ConsoleColor.DarkYellow);
            base.OnDisabled();
        }
        
        private void OnPlayerVerified(VerifiedEventArgs ev)
        {
            if (Config.IsPlayMusicNoPerson)
            {
                if (Round.IsStarted)
                {
                    return;
                }

                if (_isMusicPlaying && !Round.IsStarted)
                {
                    PlayLobbyMusic();
                }
            }
            
            if (Config.IsPlayMusicNoPerson == false)
            {
                if (Round.IsStarted)
                {
                    return;
                }
                if (!_isMusicPlaying && Server.PlayerCount >= 1 && !Round.IsStarted)
                {
                    PlayLobbyMusic();
                }
            }
        }

        private void OnRoundStarted()
        {
            StopLobbyMusic();
            Log.Debug("Round is started... stopping the music");
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            //PlayLobbyMusic();
            StopLobbyMusic();
        }
        private void OnPlayerLeft(LeftEventArgs ev)
        {
            if (Server.PlayerCount == 0)
            {
                StopLobbyMusic();
            }
        }
        private void OnWaitingForPlayers()
        {
            EnsureMusicDirectoryExists();
            PlayLobbyMusic();
            if (Server.PlayerCount == 0)
            {
                StopLobbyMusic();
            }
        }

        private void PlayLobbyMusic()
        {
            if (SharedAudioPlayer == null)
            {
                Log.Info("reset sharedAudioPlayer...");
                // AudioPlayerBase 객체를 초기화
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);

                // 초기화에 실패한 경우 오류 메시지 출력
                if (SharedAudioPlayer == null)
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
                SharedAudioPlayer.CurrentPlay = Path.Combine(_audioDirectory, Config.SingleSongName); 
                string songPath = Path.Combine(_audioDirectory, Config.SingleSongName); 
                //string songPath = Config.TempSingleSongPath;
                if (!File.Exists(songPath)) 
                { 
                    Log.Error($"Cannot find audio file: {songPath}"); 
                    return;
                }
                SharedAudioPlayer.Loop = true; 
                SharedAudioPlayer.Volume = Config.StandardVolume;
                SharedAudioPlayer.Play(-1);
                _isMusicPlaying = true;
                Log.Info($"Music Path: {SharedAudioPlayer.CurrentPlay}"); 
                Map.Broadcast(5,$"playing...{Config.LoopSingleSong}");
            }
            else if (Config.LoopSingleSong == false) 
            {
                foreach (string song in Config.QueueSongs)
                {
                    string songPath = Path.Combine(_audioDirectory, song);
                    if (!File.Exists(songPath))
                    {
                        Log.Error($"Cannot find audio file: {songPath}");
                        continue;
                    }
                    SharedAudioPlayer.Enqueue(songPath, 0);
                }

                SharedAudioPlayer.Loop = false; 
                SharedAudioPlayer.Volume = Config.StandardVolume;
                SharedAudioPlayer.Play(0); 
                _isMusicPlaying = true;
                BroadcastFileNameToPlayers(Config.QueueSongs[0]);
                Log.Info($"Music Path: {SharedAudioPlayer.CurrentPlay}"); 
                Map.Broadcast(5,$"playing...{SharedAudioPlayer.CurrentPlay}");


            }
        
        }
        
        public void SetMusicVolume(float volume)
        {
            if (SharedAudioPlayer == null)
            {
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                if (SharedAudioPlayer == null)
                {
                    Log.Error("Failed to reset sharedAudioPlayer. You cannot play music.");
                    return;
                }
            }
            if (SharedAudioPlayer != null)
            {
                SharedAudioPlayer.Volume = volume;  // 볼륨 설정 (0.0 ~ 1.0)
                Log.Info($"The volume set {volume * 100}%");
            }
            else
            {
                Log.Warn("The music is not playing... You cannot set volume");
            }
        }
        
        private void BroadcastFileNameToPlayers(string filePath)
        {
            // 파일 경로에서 파일 이름만 추출
            string fileName = Path.GetFileName(filePath);
            // 모든 플레이어에게 브로드캐스트 (5초 동안 표시)
            Map.Broadcast(5, $"<size=30><color=aqua>Now Playing: {fileName}</color></size>");
        }

        public void StopLobbyMusic()
        {
            if (SharedAudioPlayer != null && _isMusicPlaying == true)
            {
                //audioSource.Stop();
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                SharedAudioPlayer.Loop = false;
                SharedAudioPlayer.Stoptrack(true);
                _isMusicPlaying = false;
                Log.SendRaw("Stopping the music",ConsoleColor.DarkRed);
            }
            else
            {
                Log.Error("There has a no music current playing.");
            }

        }
        public string ListMusicFiles()
        {
            List<string> stringBuilder = new List<string>();
            //StringBuilder fileListBuilder = new StringBuilder();
            if (Directory.Exists(_audioDirectory))
            {
                string[] musicFiles = Directory.GetFiles(_audioDirectory, "*.ogg");  // .ogg 파일만 가져옴
                if (musicFiles.Length > 0)
                {

                    //Log.Info($"음악 파일 목록 (총 {musicFiles.Length}개):");
                    foreach (string file in musicFiles)
                    {
                        string fileName = Path.GetFileName(file);
                        stringBuilder.Add(fileName);
                    }
                }
                else
                {
                    Log.Warn("Cannot find file in folder.");
                }
            }
            else
            {
                Log.Error($"Cannot find music folder: {_audioDirectory}.");
                //Directory.CreateDirectory(audioDirectory);
            }
            string finalFileList = string.Join("\n", stringBuilder);
            return finalFileList;
        }
        
        private void EnsureMusicDirectoryExists()
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

        public void PlaySpecificMusic(string filepath)
        {
            if (SharedAudioPlayer == null)
            {
                SharedAudioPlayer = AudioPlayerBase.Get(Server.Host.ReferenceHub);
                if (SharedAudioPlayer == null)
                {
                    Log.Error("Failed to reset sharedAudioPlayer. Cannot play music.");
                    return;
                }
            }

            if (!File.Exists(filepath))
            {
                Log.Error($"Cannot find audio file: {filepath}");
                return;
            }
            
            StopLobbyMusic();
            
            SharedAudioPlayer.CurrentPlay = filepath;
            SharedAudioPlayer.Loop = false;  // 특정 곡은 반복하지 않음
            SharedAudioPlayer.Volume = Config.StandardVolume;
            _isMusicPlaying = true;
            SharedAudioPlayer.Play(-1);
            
            Log.Info($"Specific song is playing: {filepath}");
        }
    }
}
