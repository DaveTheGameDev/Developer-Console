namespace DeveloperConsole
{
    public interface IConsoleOutput
    {
        bool IsOpen { get; }
        void LogMessage(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogCommand(string command);
        void Clear();
    }
}