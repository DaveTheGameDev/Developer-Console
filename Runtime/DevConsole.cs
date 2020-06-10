using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Debugging.DeveloperConsole
{
	public static class DevConsole
	{
		public static bool IsOpen => consoleOutput?.IsOpen ?? false;
		
		private static List<CommandData> commands;
		private static bool initialised;
		private static IConsoleOutput consoleOutput;
		
		[RuntimeInitializeOnLoadMethod]
		private static void Initialize()
		{
			// Register commands
			if(initialised)
				return;

			commands = ConsoleHelper.GetCommands();
			Commands.Init();
			initialised = true;
			
			Application.logMessageReceived += UnityLogReceived;
		}

		private static void UnityLogReceived(string condition, string stacktrace, LogType type)
		{
			switch (type)
			{
				case LogType.Error:
					LogError(condition);
					LogError(stacktrace);
					break;
				case LogType.Assert:
					LogError(condition);
					LogError(stacktrace);
					break;
				case LogType.Warning:
					LogError(condition);
					break;
				case LogType.Log:
					LogError(condition);
					break;
				case LogType.Exception:
					LogError(condition);
					LogError(stacktrace);
					break;
			}
		}

		public static CommandData[] GetCommands()
		{
			return commands.ToArray();
		}

		public static void SetOutput(IConsoleOutput output)
		{
			consoleOutput = output;
		}

		public static void LogMessage(string message)
		{
			consoleOutput?.LogMessage(message, false);
		}
		
		public static void LogMessage(string message, bool showTime)
		{
			consoleOutput?.LogMessage(message, showTime);
		}
		
		public static void LogWarning(string message)
		{
			consoleOutput?.LogWarning(message);
		}
		
		public static void LogError(string message)
		{
			consoleOutput?.LogError(message);
		}
		
		public static void ExecuteCommand(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return;	
			}
			
			string[] args = ConsoleHelper.ParseCommand(input);
			consoleOutput?.LogCommand(args[0]);
			
			foreach (CommandData command in commands)
			{
				if (command.Aliases.Contains(args[0]))
				{
					command.Execute(args);
					return;
				}
			}
			
			consoleOutput?.LogError($"Invalid Command: {args[0]}");
		}

		public static void Clear()
		{
			consoleOutput?.Clear();
		}

		public static event Action<bool> OnToggleVisibility;

		private static void OnOnToggleVisibility(bool obj)
		{
			OnToggleVisibility?.Invoke(obj);
		}
	}
}