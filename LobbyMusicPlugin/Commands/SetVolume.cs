using System;
using System.Linq;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    public class SetVolume : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "사용법: SetVolume <0.0 ~ 1.0>";
                return false;
            }

            if (!float.TryParse(arguments.ElementAt(0), out float volume) || volume < 0.0f || volume > 1.0f)
            {
                response = "볼륨은 0.0에서 1.0 사이의 값이어야 합니다.";
                return false;
            }

            if (Plugin.Instance == null || Plugin.Instance._sharedAudioPlayer == null)
            {
                response = "현재 재생 중인 음악이 없습니다.";
                return false;
            }
            Plugin.Instance.SetMusicVolume(volume);
            response = $"음악 볼륨이 {volume * 100}%로 설정되었습니다.";
            return true;
        }

        public string Command { get; } = "SetVolume";
        public string[] Aliases { get; } = new[] { "SetVolume", "SV", "setv" };
        public string Description { get; } = "Set the Audio's Volume";
    }
}