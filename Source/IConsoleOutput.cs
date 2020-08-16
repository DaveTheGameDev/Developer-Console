namespace DeveloperConsole
{
    public interface IConsoleOutput
    {
        bool IsOpen { get; }
        void LogMessage(string value);
        void LogWarning(string value);
        void LogError(string value);
        void LogCommand(string value);
        void Clear();
    }
}