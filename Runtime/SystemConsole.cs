using System;

namespace Debugging.DeveloperConsole
{
    public class SystemConsole : IConsoleOutput
    {
        public bool IsOpen { get; set; } = true;
        public void Log(string logMessage)
        {
            System.Console.WriteLine(logMessage);
        }

        public void LogWarning(string warningMessage)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(warningMessage);
            System.Console.ResetColor();
        }

        public void LogError(string errorMessage)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(errorMessage);
            System.Console.ResetColor();
        }

        public void LogCommand(string command)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(command);
            System.Console.ResetColor();
        }

        public void SetSelectedEntityText(string input)
        {
            
        }

        public void SelectInputField()
        {
            
        }

        public void Clear()
        {
            
        }
    }
}