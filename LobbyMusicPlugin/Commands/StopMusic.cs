using System;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    public class StopMusic : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance == null || Plugin.Instance._sharedAudioPlayer == null)
            {
                response = "현재 재생 중인 음악이 없습니다.";
                return false;
            }

            // 음악을 중지하는 로직 호출
            Plugin.Instance.StopLobbyMusic();
            response = "음악이 중지되었습니다.";
            return true;
        }

        public string Command { get; } = "StopMusic";
        public string[] Aliases { get; } = new[] { "sm" , "stopmusic"};
        public string Description { get; } = "Force to stop music";
    }
}