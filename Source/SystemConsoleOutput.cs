namespace DeveloperConsole
{
    public class SystemConsoleOutput : IConsoleOutput
    {
        public bool IsOpen { get; } = true;
        
        public void LogMessage(string value) => System.Console.WriteLine(value);

        public void LogWarning(string value) => System.Console.WriteLine(value);

        public void LogError(string value)   => System.Console.WriteLine(value);

        public void LogCommand(string value) => System.Console.WriteLine($"> {value}");

        public void Clear()                  => System.Console.Clear();
    }
}