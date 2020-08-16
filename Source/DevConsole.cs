using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AstrayEngine;

namespace DeveloperConsole
{
    public static class DevConsole
    {
	    public static bool IsOpen => consoleOutput?.IsOpen ?? false;
	    
	    private static IConsoleOutput consoleOutput;
        private static Dictionary<string, ConVarData> registeredConVars;
        private static Dictionary<string, CommandData> registeredCommands;
        
        public static void Initialize()
        {
	        registeredConVars  = DevConsoleHelper.RegisterConVars();
	        registeredCommands = DevConsoleHelper.RegisterCommands();
        }
		
        /// <summary>
        /// Gets an array of existing console command.
        /// </summary>
        public static IEnumerable<CommandData> GetCommands() => registeredCommands.Values.ToArray();
        
        /// <summary>
        /// Gets an array of existing console variables.
        /// </summary>
        public static IEnumerable<ConVarData> GetConVars() => registeredConVars.Values.ToArray();
        
        /// <summary>
        /// Sets the console output. If null no output will be displayed.
        /// </summary>
        public static void SetOutput(IConsoleOutput output) => consoleOutput = output;

        /// <summary>
        /// Write a message to the console GUI.
        /// </summary>
        public static void LogMessage(string message) => consoleOutput?.LogMessage(message);

        /// <summary>
        /// Writes a warning to the console GUI
        /// </summary>
        public static void LogWarning(string message) => consoleOutput?.LogWarning(message);

        /// <summary>
        /// Writes an error to the console GUI
        /// </summary>
        public static void LogError(string message) => consoleOutput?.LogError(message);
        
        /// <summary>
        /// Takes a string input and executes that as a command.
        /// </summary>
        public static void ExecuteCommand(string input)
        {
	        consoleOutput?.LogCommand(input);
	        
            if(string.IsNullOrWhiteSpace(input))
                return;
            
            var args = DevConsoleHelper.ParseCommand(input);
            var arg = args[0].ToLower();
            
            if (registeredConVars.ContainsKey(arg))
            {
	            if (args.Length == 1)
	            {
		            LogMessage($"{arg}: {registeredConVars[arg].FieldInfo.GetValue(null)}");
	            }
	            else
	            {
		            SetConVarValue(registeredConVars[arg].FieldInfo, args[1]);
	            }
            }
            else if (registeredCommands.ContainsKey(arg))
            {
	            registeredCommands[arg].Execute(args);
            }
            else
            {
	            consoleOutput?.LogError($"Invalid Command: {args[0]}");
            }
        }

        private static void SetConVarValue(FieldInfo conVar, string arg)
        {
	        if (DevConsoleHelper.TryParseBool(conVar.FieldType, arg, out var boolVal))
	        {
		        conVar.SetValue(null, boolVal);
		        LogMessage($"Set {conVar.Name} value to {boolVal}");
	        }

	        if (DevConsoleHelper.TryParseByte(conVar.FieldType, arg, out var byteVal))
	        {
		        conVar.SetValue(null, byteVal);
		        LogMessage($"Set {conVar.Name} value to {byteVal}");
	        }

	        if (DevConsoleHelper.TryParseSByte(conVar.FieldType, arg, out var sbyteVal))
	        {
		        conVar.SetValue(null, sbyteVal);
		        LogMessage($"Set {conVar.Name} value to {sbyteVal}");
	        }

	        if (DevConsoleHelper.TryParseSByte(conVar.FieldType, arg, out var charVal))
	        {
		        conVar.SetValue(null, charVal);
		        LogMessage($"Set {conVar.Name} value to {charVal}");
	        }

	        if (DevConsoleHelper.TryParseDecimal(conVar.FieldType, arg, out var decimalVal))
	        {
		        conVar.SetValue(null, decimalVal);
		        LogMessage($"Set {conVar.Name} value to {decimalVal}");
	        }

	        if (DevConsoleHelper.TryParseDecimal(conVar.FieldType, arg, out var doubleVal))
	        {
		        conVar.SetValue(null, doubleVal);
		        LogMessage($"Set {conVar.Name} value to {doubleVal}");
	        }

	        if (DevConsoleHelper.TryParseFloat(conVar.FieldType, arg, out var floatVal))
	        {
		        conVar.SetValue(null, floatVal);
		        LogMessage($"Set {conVar.Name} value to {floatVal}");
	        }

	        if (DevConsoleHelper.TryParseInt(conVar.FieldType, arg, out var intVal))
	        {
		        conVar.SetValue(null, intVal);
		        LogMessage($"Set {conVar.Name} value to {intVal}");
	        }

	        if (DevConsoleHelper.TryParseUInt(conVar.FieldType, arg, out var uintVal))
	        {
		        conVar.SetValue(null, uintVal);
		        LogMessage($"Set {conVar.Name} value to {uintVal}");
	        }

	        if (DevConsoleHelper.TryParseLong(conVar.FieldType, arg, out var longVal))
	        {
		        conVar.SetValue(null, longVal);
		        LogMessage($"Set {conVar.Name} value to {longVal}");
	        }

	        if (DevConsoleHelper.TryParseULong(conVar.FieldType, arg, out var ulongVal))
	        {
		        conVar.SetValue(null, ulongVal);
		        LogMessage($"Set {conVar.Name} value to {ulongVal}");
	        }

	        if (DevConsoleHelper.TryParseShort(conVar.FieldType, arg, out var shortVal))
	        {
		        conVar.SetValue(null, shortVal);
		        LogMessage($"Set {conVar.Name} value to {shortVal}");
	        }

	        if (DevConsoleHelper.TryParseUshort(conVar.FieldType, arg, out var ushortVal))
	        {
		        conVar.SetValue(null, ushortVal);
		        LogMessage($"Set {conVar.Name} value to {ushortVal}");
	        }

	        if (DevConsoleHelper.TryParseString(conVar.FieldType))
	        {
		        conVar.SetValue(null, arg);
		        LogMessage($"Set {conVar.Name} value to {arg}");
	        }
        }
    }
}