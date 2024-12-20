using System;
using CommandSystem;

namespace LobbyMusicPlugin.Commands
{
	[CommandHandler(typeof(ClientCommandHandler))]
	[CommandHandler(typeof(RemoteAdminCommandHandler))]
	public class List : ICommand
	{
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			response = $"Successfully music list has been printed!\n{Plugin.Instance.ListMusicFiles()}";
			return true;
		}

		public string Command { get; } = "MusicList";
		public string[] Aliases { get; } = new[] {"MusicList","ML","mli"};
		public string Description { get; } = "Print a list of Music";
	}
}
