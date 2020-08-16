using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AstrayEngine;

namespace DeveloperConsole
{
     internal static class DevConsoleHelper
    {
        internal static Dictionary<string, ConVarData> RegisterConVars()
        {
            var conVars = new Dictionary<string, ConVarData>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
	            foreach (var type in assembly.GetTypes())
                {
	                foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
	                {
		                var conVar = fieldInfo.GetCustomAttribute<ConVar>();

		                if (conVar != null)
		                {
			                var cVar = new ConVarData(fieldInfo, conVar.Description);
			                conVars.Add(cVar.Alias, cVar);
		                }
	                }
                }
            }
			
            return conVars;
        }
        
        public static Dictionary<string, CommandData> RegisterCommands()
        {
	        var commands = new Dictionary<string, CommandData>();

	        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
	        {
		        foreach (var type in assembly.GetTypes())
		        {
			        foreach (var method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			        {
				        var command = method.GetCustomAttribute<ConCommand>();
				        var commandDesc = method.GetCustomAttribute<ConCommandDesc>();
				        
				        ParameterInfo[] parameters = method.GetParameters();
				        Type[] paramList = parameters.Select(parameter => parameter.ParameterType).ToArray();

				        if(command == null)
					        continue;

				        var data = CreateCommandData(command.Alias, commandDesc?.Description, paramList, method);

				        if (commands.ContainsKey(data.Alias))
					        throw new Exception("Console command alias \"{alias}\" already in use");

				        commands.Add(data.Alias.ToLower(), data);
			        }
		        }
	        }
			
	        return commands;
        }
        
        private static CommandData CreateCommandData(string alias, string description, Type[] paramList, MethodInfo method)
        {
	        return new CommandData(alias, description, method, paramList);
        }
        
        public static string[] ParseCommand(string commandInput)
        {
            var result = new List<string>();
            var index = 0;

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
        
        private static readonly Type BoolType = typeof(bool);
		private static readonly Type ByteType = typeof(byte);
		private static readonly Type SbyteType = typeof(sbyte);
		private static readonly Type CharType = typeof(char);
		private static readonly Type DecimalType = typeof(decimal);
		private static readonly Type DoubleType = typeof(double);
		private static readonly Type FloatType = typeof(float);
		private static readonly Type IntType = typeof(int);
		private static readonly Type UintType = typeof(uint);
		private static readonly Type LongType = typeof(long);
		private static readonly Type UlongType = typeof(ulong);
		private static readonly Type ShortType = typeof(short);
		private static readonly Type UshortType = typeof(ushort);
		private static readonly Type StringType = typeof(string);
		
		public static bool TryParseBool(Type param, string input, out bool value)
		{
			input = input.ToLower();
			bool isValid = (input == "1" || input == "true" || input == "0" || input == "false");
			
			value = (input == "1" || input == "true");
			return param == BoolType && isValid;
		} 
		
		public static bool TryParseByte(Type param, string input, out byte value)
		{
			value = default;
			return param == ByteType && byte.TryParse(input, out value);
		}
		
		public static bool TryParseSByte(Type param, string input, out sbyte value)
		{
			value = default;
			return param == SbyteType && sbyte.TryParse(input, out value);
		}
		
		public static bool TryParseChar(Type param, string input, out char value)
		{
			value = default;
			return param == CharType && char.TryParse(input, out value);
		}
		
		public static bool TryParseDecimal(Type param, string input, out decimal value)
		{
			value = default;
			return param == DecimalType && decimal.TryParse(input, out value);
		}
		
		public static bool TryParseDouble(Type param, string input, out double value)
		{
			value = default;
			return param == DoubleType && double.TryParse(input, out value);
		}
		
		public static bool TryParseFloat(Type param, string input, out float value)
		{
			value = default;
			return param == FloatType && float.TryParse(input, out value);
		}
		
		public static bool TryParseInt(Type param, string input, out int value)
		{
			value = default;
			return param == IntType && int.TryParse(input, out value);
		}
		
		public static bool TryParseUInt(Type param, string input, out uint value)
		{
			value = default;
			return param == UintType && uint.TryParse(input, out value);
		}
		
		public static bool TryParseLong(Type param, string input, out long value)
		{
			value = default;
			return param == LongType && long.TryParse(input, out value);
		}
		
		public static bool TryParseULong(Type param, string input, out ulong value)
		{
			value = default;
			return param == UlongType && ulong.TryParse(input, out value);
		}
		
		public static bool TryParseShort(Type param, string input, out short value)
		{
			value = default;
			return param == ShortType && short.TryParse(input, out value);
		}
		
		public static bool TryParseUshort(Type param, string input, out ushort value)
		{
			value = default;
			return param == UshortType && ushort.TryParse(input, out value);
		}
		
		public static bool TryParseString(Type param)
		{
			return param == StringType;
		}

		
    }
}