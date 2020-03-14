using System.Text;
using Cysharp.Text;
using UnityEngine;

namespace Debugging.DeveloperConsole
{
    public static class Commands
    {
        private static string SysInfo { get; set; }
        private static string HelpMessage { get; set; }

        public static void Init()
        {
            GetSystemInfo();
            GetHelpInfo();
        }
        
        private static void GetSystemInfo()
        {
            StringBuilder builder = new StringBuilder("System Information \n");

            builder.AppendLine($"OS: {SystemInfo.operatingSystem}");
            builder.AppendLine($"Graphics: {SystemInfo.graphicsDeviceName} - {SystemInfo.graphicsDeviceVersion} - Shader Level {SystemInfo.graphicsShaderLevel}");
            builder.AppendLine($"CPU: {SystemInfo.processorType} - Cores {SystemInfo.processorCount}");
            builder.AppendLine($"VRAM: {SystemInfo.graphicsMemorySize}MB");
            builder.AppendLine($"RAM: {SystemInfo.systemMemorySize}MB");

            SysInfo = builder.ToString();
        }
        
        private static void GetHelpInfo()
        {
            using(var sb = ZString.CreateStringBuilder())
            {
                foreach (CommandData data in DevConsole.GetCommands())
                {
                    for (var i = 0; i < data.Aliases.Length; i++)
                    {
                        string alias = data.Aliases[i];

                        if (i != data.Aliases.Length -1)
                        {
                            sb.Append(ZString.Concat(alias, ", "));
                        }
                        else
                        {
                            sb.Append(ZString.Concat(alias, ": "));
                        }
                    }

                    sb.Append(data.Description);
                    sb.AppendLine();
                }

                HelpMessage = sb.ToString();
            }
        }

        [ConsoleCommand]
        [CommandDescription("Displays information for all commands")]
        public static void Help()
        {
            DevConsole.LogMessage(HelpMessage, false);
        }
        
        [ConsoleCommand("sys")]
        [CommandDescription("prints system information")]
        private static void PrintSystemInfo()
        {
            DevConsole.LogMessage(SysInfo, false);
        }
        
        [ConsoleCommand("quit", "exit", "q")]
        [CommandDescription("Exits application")]
        private static void Quit()
        {
            Application.Quit();
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
        
        [ConsoleCommand("clear", "cls")]
        [CommandDescription("Clears the console window")]
        private static void Clear()
        {
            DevConsole.Clear();
        }
    }
}