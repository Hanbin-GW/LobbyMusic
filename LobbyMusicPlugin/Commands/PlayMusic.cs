using System;
using System.IO;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class PlayMusic : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count == 0)
            {
                response = "How to use: Pm <Filename.ogg>";
                return false;
            }
            string fileName = arguments.At(0);  // 첫 번째 인자로 파일 이름을 가져옴
            string audioDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EXILED", "Plugins", "audio");
            string filePath = Path.Combine(audioDirectory, fileName);
            
            if (!File.Exists(filePath))
            {
                response = $"Cannot find file: {filePath}";
                return false;
            }

            Plugin.Instance.PlaySpecificMusic(filePath);
            response = $"Playing music: {fileName}";
            return true;
        }

        public string Command { get; } = "PlayMusic";
        public string[] Aliases { get; } = new[] { "Pm", "PlayMusic", "PlayM" };
        public string Description { get; } = "play the specific music!";
    }
}