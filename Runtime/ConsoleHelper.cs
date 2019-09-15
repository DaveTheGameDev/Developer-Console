using System;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lyrebird.Debugging.Console
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
		private static readonly Type Vector3Type = typeof(Vector3);
		private static readonly Type ColorType = typeof(Color);
		internal static readonly Type GameObjectType = typeof(GameObject);
		
		private static string SysInfo;
		
		[RuntimeInitializeOnLoadMethod]
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
		
		public static bool TryParseGameObject(Type param, out GameObject value)
		{
			value = null;
			
			if (param != GameObjectType)
			{
				return false;
			}
			value = DebugObjectSelector.SelectedGameObject;
			return value;
		}
		
		public static bool TryParseBool(Type param, string input, out bool value)
		{
			value = false;
			return param == BoolType && bool.TryParse(input, out value);
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
		
		public static bool TryParseVector3(Type param, string input, out Vector3 vector3)
		{
			vector3 = default;

			if (param != Vector3Type)
			{
				return false;
			}
			if (!input.Contains(","))
			{
				ConsoleSystem.LogError("Invalid syntax (Vector3)");
				return false;
			}
			
			var xyz = input.Split(',');

			if (!float.TryParse(xyz[0], out float x))
			{
				ConsoleSystem.LogError("Invalid syntax (Vector3.x)");
				return false;
			}
			
			if (!float.TryParse(xyz[1], out float y))
			{
				ConsoleSystem.LogError("Invalid syntax (Vector3.y)");
				return false;
			}
			
			if (!float.TryParse(xyz[2], out float z))
			{
				ConsoleSystem.LogError("Invalid syntax (Vector3.z)");
				return false;
			}
			
			vector3 = new Vector3(x,y,z);
			return true;
		}

		public static bool TryParseColor(Type param, string input, out Color color)
		{
			color = default;

			if (param != ColorType)
			{
				return false;
			}
			
			if (input.ToLower().Equals("red"))
			{
				color = Color.red;
				return true;
			}
			if (input.ToLower().Equals("green"))
			{
				color = Color.green;
				return true;
			}
			if (input.ToLower().Equals("blue"))
			{
				color = Color.blue;
				return true;
			}
			if (input.ToLower().Equals("white"))
			{
				color = Color.white;
				return true;
			}
			if (input.ToLower().Equals("black"))
			{
				color = Color.black;
				return true;
			}
			if (input.ToLower().Equals("yellow"))
			{
				color = Color.yellow;
				return true;
			}
			if (input.ToLower().Equals("cyan"))
			{
				color = Color.cyan;
				return true;
			}
			if (input.ToLower().Equals("magenta"))
			{
				color = Color.magenta;
				return true;
			}
			if (input.ToLower().Equals("gray"))
			{
				color = Color.gray;
				return true;
			}
			if (input.ToLower().Equals("grey"))
			{
				color = Color.grey;
				return true;
			}

			if(input.ToLower().Contains("#"))
			{
				return ColorUtility.TryParseHtmlString(input, out color);
			}

			return false;
		}

		[ConsoleCommand("help", "Prints out all commands and their descriptions")]
		private static void Help()
		{
			StringBuilder builder = new StringBuilder("Available Commands \n");

			foreach (var value in ConsoleSystem.RegisteredCommands.Values)
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
#else
			Application.Quit();
#endif
		}
		
		[ConsoleCommand("print-colors", "prints preset color names")]
		private static void PrintColors()
		{
			ConsoleSystem.Log(colorsMessage);
		}
		
		[ConsoleCommand("sys-info", "prints system information")]
		private static void PrintSystemInfo()
		{
			ConsoleSystem.Log(SysInfo);
		}
	}
}