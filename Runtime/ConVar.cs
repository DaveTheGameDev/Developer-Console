using System;

namespace DeveloperConsole
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ConVar : Attribute
    {
        public readonly string Description;

        public ConVar(string description = null)
        {
            Description = description;
        }
    }
}