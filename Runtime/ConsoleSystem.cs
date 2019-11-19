﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Debugging.DeveloperConsole
{
	//TODO: Add string parsing for args "example arg"
	/// <summary>
	/// Used to write information to a text based output (Unity Console / Custom In Game UI).
	/// Also used to execute commands.
	/// </summary>
	public static class ConsoleSystem
	{
		public static event Action<bool> ConsoleVisibilityChanged;
		
		private static Dictionary<string, CommandData> registeredCommands = new Dictionary<string, CommandData>();
		
		private static bool initialized;
		private static IConsoleOutput consoleOutput;

		private static List<string> preLogQueue;
		private static List<string> preWarningQueue;
		private static List<string> preErrorQueue;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Init()
		{
			registeredCommands?.Clear();
			ConsoleVisibilityChanged = null;
			preLogQueue?.Clear();
			preWarningQueue?.Clear();
			preErrorQueue?.Clear();
			initialized = false;
			consoleOutput = null;
		}
		
		private static void RegisterCommands()
		{
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembly.GetTypes())
				{
					foreach (var method in type.GetMethods(
						BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (!method.IsStatic)
						{
							continue;
						}
						
						var commands = method.GetCustomAttributes<ConsoleCommand>();
						var commandUsage = method.GetCustomAttribute<CommandUsage>();

						string usage = commandUsage != null ? commandUsage.Usage : "";
						
						var parameters = method.GetParameters();
						Type[] paramList = parameters.Select(parameter => parameter.ParameterType).ToArray();

						foreach (var command in commands)
						{
							if (command != null)
							{
								AddCommand(command.Name, command.Description, usage, paramList, method);
							}
						}
					}
				}
			}

			registeredCommands = registeredCommands.OrderBy(key => key.Key).ToDictionary(obj => obj.Key, obj => obj.Value);
		}

		/// <summary>
		/// Initialise the console with an output object.
		/// </summary>
		/// <param name="output"></param>
		public static void Initialize(IConsoleOutput output)
		{
			initialized = output != null;
			consoleOutput = output;

			RegisterCommands();

			for (var i = 0; i < preLogQueue.Count; i++)
			{
				string log = preLogQueue[i];
				Log(log);
			}

			for (var i = 0; i < preWarningQueue.Count; i++)
			{
				string warning = preWarningQueue[i];
				Log(warning);
			}

			for (var i = 0; i < preErrorQueue.Count; i++)
			{
				string error = preErrorQueue[i];
				Log(error);
			}
		}

		/// <summary>
		/// Writes a log message to the console UI.
		/// </summary>
		/// <param name="logMessage"></param>
		public static void Log(string logMessage)
		{
			if (IsInitialised())
			{
				consoleOutput?.Log($"{DateTime.Now.ToShortTimeString()}: {logMessage}");
			}
			else
			{
				preLogQueue.Add(logMessage);
			}
		}

		/// <summary>
		/// Writes a warning message to the console UI.
		/// </summary>
		/// <param name="warningMessage"></param>
		public static void LogWarning(string warningMessage)
		{
			if (IsInitialised())
			{
				consoleOutput?.LogWarning($"{DateTime.Now.ToShortTimeString()}: {warningMessage}");
			}
			else
			{
				preWarningQueue.Add(warningMessage);
			}
		}

		/// <summary>
		/// Writes an error message to the console UI.
		/// </summary>
		/// <param name="errorMessage"></param>
		public static void LogError(string errorMessage)
		{
			if (IsInitialised())
			{
				consoleOutput?.LogError($"{DateTime.Now.ToShortTimeString()}:{errorMessage}");
			}
			else
			{
				preErrorQueue.Add(errorMessage);
			}
		}

		/// <summary>
		/// Add text to input field.
		/// Useful for auto complete.
		/// </summary>
		/// <param name="input"></param>
		public static void SelectInputField()
		{
			consoleOutput?.SelectInputField();
		}
		
		/// <summary>
		/// Sets the text for the selected objects text field in the console GUI.
		/// </summary>
		/// <param name="input"></param>
		public static void SetSelectedEntityText(string input)
		{
			consoleOutput?.SetSelectedEntityText(input);
		}
		
		/// <summary>
		/// Clears the console.
		/// </summary>
		public static void Clear()
		{
			consoleOutput?.Clear();
		}

		public static void ConsoleOpened()
		{
			ConsoleVisibilityChanged?.Invoke(true);
		}
		
		public static void ConsoleClosed()
		{
			ConsoleVisibilityChanged?.Invoke(false);
		}

		/// <summary>
		/// Execute a console command write output to the console.
		/// </summary>
		/// <param name="command"></param>
		public static void ExecuteCommand(string command)
		{
			if (!IsInitialised())
			{
				return;
			}

			string[] args = ParseCommand(command);

			// arg 0 is command name
			// anything after is an arg for that command
			string commandName = args[0];

			// Show user their input in the console
			consoleOutput?.LogCommand(command);

			// Invoke command listener if it exists
			if (registeredCommands.ContainsKey(commandName))
			{
				registeredCommands[commandName].Invoke(args);
				return;
			}

			// We failed to run command! Let the user know.
			LogError("Invalid Command");
		}

		/// <summary>
		/// Manually add a command to the console system.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="listenerEvent"></param>
		private static void AddCommand(string name, string description, string usage, Type[] parameters, MethodInfo methodInfo)
		{
			var data = new CommandData(name, description, usage, parameters, methodInfo);

			if (registeredCommands.ContainsKey(name.ToLower()))
			{
				LogError($"A command with name {name.ToLower()} already exists");
				return;
			}

			registeredCommands.Add(name, data);
		}

		private static string[] ParseCommand(string commandInput)
		{
			List<string> result = new List<string>();
			int index = 0;

			while (index < commandInput.Length)
			{
				SkipWhite(commandInput, ref index);
				
				if (index == commandInput.Length)
				{
					break;
				}
				
				if ((commandInput[index] == '"' || commandInput[index] == '{' ) && (index == 0 || commandInput[index - 1] != '\\'))
				{
					result.Add(ParseQuoted(commandInput, ref index));
				}
				else
				{
					result.Add(Parse(commandInput, ref index));
				}
			}

			return result.ToArray();
		}

		private static void SkipWhite(string commandInput, ref int pos)
		{
			while (pos < commandInput.Length && " \t".IndexOf(commandInput[pos]) > -1)
			{
				pos++;
			}
		}

		private static string ParseQuoted(string commandInput, ref int index)
		{
			index++;
			int startPos = index;
			while (index < commandInput.Length)
			{
				if ((commandInput[index] == '"' || commandInput[index] == '}') && commandInput[index - 1] != '\\')
				{
					index++;
					return commandInput.Substring(startPos, index - startPos - 1);
				}
				index++;
			}
			return commandInput.Substring(startPos);
		}

		private static string Parse(string commandInput, ref int index)
		{
			int startPos = index;
			while (index < commandInput.Length)
			{
				if (" \t".IndexOf(commandInput[index]) > -1)
				{
					return commandInput.Substring(startPos, index - startPos);
				}
				index++;
			}
			return commandInput.Substring(startPos);
		}

		/// <summary>
		/// Check if the console has been initialised.
		/// </summary>
		/// <returns>Returns true if console is ready to be written to and false if not.(Also throws a unity log error if false)</returns>
		public static bool IsInitialised()
		{
			if(preLogQueue == null)
				preLogQueue = new List<string>();
			
			if(preWarningQueue == null)
				preWarningQueue = new List<string>();
			
			if(preErrorQueue == null)
				preErrorQueue = new List<string>();
			
			if (initialized && consoleOutput != null)
			{
				return true;
			}
			return false;
		}

		public static CommandData[] GetCommands()
		{
			return registeredCommands.Values.ToArray();
		}

		public static bool IsOpen()
		{
			return consoleOutput != null && consoleOutput.IsOpen;
		}
	}
}