using System;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class CurrentPlay : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (Plugin.Instance == null || Plugin.Instance.SharedAudioPlayer == null)
            {
                response = "he music is not playing...";
                return false;
            }
            string currentSong = Plugin.Instance.SharedAudioPlayer.CurrentPlay;
            if (string.IsNullOrEmpty(currentSong))
            {
                response = "The music is not playing...";
                return false;
            }
            response = $"Current music: {currentSong}";
            return true;
        }

        public string Command { get; } = "CurrentPlay";
        public string[] Aliases { get; } = new[] { "cp" };
        public string Description { get; } = "Prints a current song play";
    }
}