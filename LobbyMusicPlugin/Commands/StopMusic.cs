using System;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    //error in somewhere...
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class StopMusic : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance == null || Plugin.Instance.SharedAudioPlayer == null)
            {
                response = "there has a no music playing now";
                return false;
            }

            // 음악을 중지하는 로직 호출
            Plugin.Instance.StopLobbyMusic();
            response = "The Music has been stopped";
            return true;
        }

        public string Command { get; } = "StopMusic";
        public string[] Aliases { get; } = new[] { "sm" , "stopmusic"};
        public string Description { get; } = "Force to stop music";
    }
}