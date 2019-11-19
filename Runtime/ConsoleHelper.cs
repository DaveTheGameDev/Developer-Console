using System;
using System.Text;
using UnityEngine;

namespace Debugging.DeveloperConsole
{
	public static class ConsoleHelper
	{
		private static readonly string colorsMessage = "Available Colors: red, green, blue, white, black, yellow, cyan, magenta, gray, grey. Hex colors are also supported (#ff0000)";

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
		
		private static string sysInfo;
		
		[RuntimeInitializeOnLoadMethod]
		private static void GetSystemInfo()
		{
			StringBuilder builder = new StringBuilder("System Information \n");

			builder.AppendLine($"OS: {SystemInfo.operatingSystem}");
			builder.AppendLine($"Graphics: {SystemInfo.graphicsDeviceName} - {SystemInfo.graphicsDeviceVersion} - Shader Level {SystemInfo.graphicsShaderLevel}");
			builder.AppendLine($"CPU: {SystemInfo.processorType} - Cores {SystemInfo.processorCount}");
			builder.AppendLine($"VRAM: {SystemInfo.graphicsMemorySize}MB");
			builder.AppendLine($"RAM: {SystemInfo.systemMemorySize}MB");

			sysInfo = builder.ToString();
		}

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
		
		public static bool TryParseString(Type param, string input)
		{
			return param == StringType;
		}
		

		[ConsoleCommand("help", "Prints out all commands and their descriptions")]
		private static void Help()
		{
			StringBuilder builder = new StringBuilder("Available Commands \n");

			foreach (var value in ConsoleSystem.GetCommands())
			{
				string usage = string.IsNullOrWhiteSpace(value.Usage) ? "" : $"Usage = {value.Usage}";
				builder.AppendLine($"{value.Name} - {value.Description}. {usage}");
			}
			
			ConsoleSystem.Log(builder.ToString());
		}
		
		[ConsoleCommand("time", "prints the current time to the console")]
		private static void Time()
		{
			ConsoleSystem.Log(DateTime.Now.ToLongDateString());
		}

		[ConsoleCommand("hello", "world")]
		private static void HelloWorld()
		{
			ConsoleSystem.Log("Hello World!");
		}

		[ConsoleCommand("clear", "clears the console window")]
		[ConsoleCommand("cls", "clears the console window")]
		private static void Clear()
		{
			ConsoleSystem.Clear();
		}

		[ConsoleCommand("quit", "quits the game")]
		private static void Quit()
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#endif
			Application.Quit();
		}
		
		[ConsoleCommand("print-colors", "prints preset color names")]
		private static void PrintColors()
		{
			ConsoleSystem.Log(colorsMessage);
		}
		
		[ConsoleCommand("sys", "prints system information")]
		private static void PrintSystemInfo()
		{
			ConsoleSystem.Log(sysInfo);
		}
	}
}