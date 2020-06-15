using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.TextCore;

namespace DeveloperConsole
{
    public static class DevConsole
    {
	    public static bool IsOpen => consoleOutput?.IsOpen ?? false;
	    
	    private static IConsoleOutput consoleOutput;
        private static List<ConVarData> registeredConVars;
        private static List<CommandData> registeredCommands;
        
        public static void Initialize()
        {
	        registeredConVars = DevConsoleHelper.RegisterConVars();
	        registeredCommands = DevConsoleHelper.RegisterCommands();

            foreach (var conVar in registeredConVars)
            {
	            Console.WriteLine($"Registered ConVar: {conVar.Name}");
            }
        }

        public static void SetOutput(IConsoleOutput output)
        {
	        consoleOutput = output;
        }
        
        public static void LogMessage(string message)
        {
	        consoleOutput?.LogMessage(message);
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
	        consoleOutput?.LogCommand(input);
	        
            if(string.IsNullOrWhiteSpace(input))
                return;
            
            var args = DevConsoleHelper.ParseCommand(input);

            foreach (var conVar in registeredConVars)
            {
	            string conVarName = conVar.Name;
	            string arg = args[0].ToLower();
	            
	            if(!conVarName.Equals(arg))
		            continue;
	            
                if (args.Length == 1)
                {
	                LogConVarValue(conVar.FieldInfo, args[0]);
	                return;
                }

                SetConVarValue(conVar.FieldInfo, args[1]);
                return;
            }
            
            foreach (var command in registeredCommands.Where(command => ((IList) command.Aliases).Contains(args[0])))
            {
	            command.Execute(args);
	            return;
            }
			
            consoleOutput?.LogError($"Invalid Command: {args[0]}");
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

        private static void LogConVarValue(FieldInfo conVar, string arg)
        {
	        if (conVar.FieldType == typeof(bool))
	        {
		        bool value = (bool) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(byte))
	        {
		        byte value = (byte) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(sbyte))
	        {
		        sbyte value = (sbyte) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(char))
	        {
		        char value = (char) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(decimal))
	        {
		        decimal value = (decimal) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(double))
	        {
		        double value = (double) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(float))
	        {
		        float value = (float) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(int))
	        {
		        int value = (int) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(uint))
	        {
		        uint value = (uint) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(long))
	        {
		        long value = (long) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(ulong))
	        {
		        ulong value = (ulong) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(short))
	        {
		        short value = (short) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(ushort))
	        {
		        ushort value = (ushort) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }

	        if (conVar.FieldType == typeof(string))
	        {
		        string value = (string) conVar.GetValue(null);
		        LogMessage($"{arg} = {value}");
	        }
        }

        public static IEnumerable GetCommands()
        {
	        return registeredCommands.ToArray();
        }

        public static IEnumerable GetConVars()
        {
	        return registeredConVars;
        }
    }

   
}