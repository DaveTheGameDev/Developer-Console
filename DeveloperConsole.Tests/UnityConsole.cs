using Debugging.DeveloperConsole;
using UnityEngine;

namespace DeveloperConsole.Tests
{
    public class UnityConsole : IConsoleOutput
    {
        public bool IsOpen { get; set; } = true;
        public bool LogToFile { get; set; }

        public void Log(string logMessage)
        {
            Debug.Log(logMessage);
        }

        public void LogWarning(string warningMessage)
        {
            Debug.LogWarning(warningMessage);
        }

        public void LogError(string errorMessage)
        {
            Debug.LogWarning(errorMessage);
        }

        public void LogCommand(string command)
        {
            Debug.Log(command);
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