using System;

namespace DeveloperConsole
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ConCommand : Attribute
    {
        public string Alias;

        public ConCommand(string alias)
        {
            Alias = alias;
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