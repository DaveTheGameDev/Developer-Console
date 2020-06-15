using System.Reflection;

namespace DeveloperConsole
{
    public readonly struct ConVarData
    {
        public readonly string Name;
        public readonly string Description;
        public readonly FieldInfo FieldInfo;

        public ConVarData( FieldInfo fieldInfo, string description)
        {
            Name = fieldInfo.Name.ToLower();
            Description = description;
            FieldInfo = fieldInfo;
        }

        public override string ToString()
        {
            return $"{Name}: {Description}";
        }
    }
}