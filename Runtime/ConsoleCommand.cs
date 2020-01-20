using System;

namespace DeveloperConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ConsoleCommand : Attribute
    { 
        public readonly string Name;
        public readonly string Description;

        public ConsoleCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
	
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandUsage : Attribute
    { 
        public readonly string Usage;

        public CommandUsage(string usage)
        {
            Usage = usage;
        }
    }
}