using System;

namespace Debugging.DeveloperConsole
{
	public class ConsoleCommand : Attribute
	{
		public string[] CommandAliases;

		public ConsoleCommand(params string[] commandAliases)
		{
			CommandAliases = commandAliases;
		}
	}
}