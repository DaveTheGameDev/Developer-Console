using System;

namespace DeveloperConsole
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConCommand : Attribute
    {
        public string[] CommandAliases;

        public ConCommand(params string[] commandAliases)
        {
            CommandAliases = commandAliases;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method)]
    public class ConCommandDesc : Attribute
    {
        public readonly string Description;

        public ConCommandDesc(string description)
        {
            Description = description;
        }
    }
}