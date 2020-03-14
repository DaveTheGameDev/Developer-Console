using System;

namespace Debugging.DeveloperConsole
{
	public class CommandDescription : Attribute
	{
		public readonly string Description;

		public CommandDescription(string description)
		{
			Description = description;
		}
	}
}