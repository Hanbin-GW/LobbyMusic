using System;
using System.Linq;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SetVolume : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "How to using: SetVolume <0.0 ~ 1.0>";
                return false;
            }

            if (!float.TryParse(arguments.ElementAt(0), out float volume) || volume < 0.0f || volume > 1.0f)
            {
                response = "The volume set Between 0.0 to 1.0.";
                return false;
            }

            if (Plugin.Instance == null || Plugin.Instance.SharedAudioPlayer == null)
            {
                response = "there has no playing music now.";
                return false;
            }
            Plugin.Instance.SetMusicVolume(volume);
            response = $"The music volume set {volume * 100}%.";
            return true;
        }

        public string Command { get; } = "SetVolume";
        public string[] Aliases { get; } = new[] { "SetVolume", "SV", "setv" };
        public string Description { get; } = "Set the Audio's Volume";
    }
}