using System;
using System.Reflection;
using Cysharp.Text;
using UnityEngine;

namespace Debugging.DeveloperConsole
{
	public struct CommandData
	{
		public readonly string[] Aliases;
		public readonly string Description;
		public readonly Type[] Parameters;
		public readonly MethodInfo MethodInfo;

		public CommandData(string[] aliases, string description, Type[] parameters, MethodInfo methodInfo)
		{
			Aliases = aliases;
			Description = description;
			Parameters = parameters;
			MethodInfo = methodInfo;
		}

		public void Execute(string[] args)
		{
			bool success = true;
			
			if (Parameters == null || Parameters.Length == 0)
			{
				MethodInfo.Invoke(null, null);
				return;
			}
			
			object[] objectParams = new object[Parameters.Length];

			int nextArg = 0;
			for (int i = 0; i < Parameters.Length; i++)
			{
				// Increase next arg index here to make sure indexing order does not go out of sync.
				nextArg++;
				
				// If not enough args have been passed parse failed. Inform user.
				if (nextArg >= args.Length)
				{
					success = InformFailedParse(i, FailReason.NotEnoughArgs);
					break;
				}

				if (ConsoleHelper.TryParseBool(Parameters[i], args[nextArg], out var boolValue))
				{
					objectParams[i] = boolValue;
					continue;
				}

				if (ConsoleHelper.TryParseByte(Parameters[i], args[nextArg], out var byteValue))
				{
					objectParams[i] = byteValue;
					continue;
				}

				if (ConsoleHelper.TryParseSByte(Parameters[i], args[nextArg], out var sbyteValue))
				{
					objectParams[i] = sbyteValue;
					continue;
				}

				if (ConsoleHelper.TryParseChar(Parameters[i], args[nextArg], out var charValue))
				{
					objectParams[i] = charValue;
					continue;
				}

				if (ConsoleHelper.TryParseDecimal(Parameters[i], args[nextArg], out var decimalValue))
				{
					objectParams[i] = decimalValue;
					continue;
				}

				if (ConsoleHelper.TryParseFloat(Parameters[i], args[nextArg], out var floatValue))
				{
					objectParams[i] = floatValue;
					continue;
				}
				
				if (ConsoleHelper.TryParseDouble(Parameters[i], args[nextArg], out var doubleValue))
				{
					objectParams[i] = doubleValue;
					continue;
				}

				if (ConsoleHelper.TryParseInt(Parameters[i], args[nextArg], out var intValue))
				{
					objectParams[i] = intValue;
					continue;
				}

				if (ConsoleHelper.TryParseUInt(Parameters[i], args[nextArg], out var uintValue))
				{
					objectParams[i] = uintValue;
					continue;
				}

				if (ConsoleHelper.TryParseLong(Parameters[i], args[nextArg], out var longValue))
				{
					objectParams[i] = longValue;
					continue;
				}

				if (ConsoleHelper.TryParseULong(Parameters[i], args[nextArg], out var ulongValue))
				{
					objectParams[i] = ulongValue;
					continue;
				}

				if (ConsoleHelper.TryParseShort(Parameters[i], args[nextArg], out var shortValue))
				{
					objectParams[i] = shortValue;
					continue;
				}

				if (ConsoleHelper.TryParseUshort(Parameters[i], args[nextArg], out var ushortValue))
				{
					objectParams[i] = ushortValue;
					continue;
				}

				if (ConsoleHelper.TryParseString(Parameters[i]))
				{
					objectParams[i] = args[nextArg];
					continue;
				}
				
				// Failed to parse command.
				success = InformFailedParse(i, FailReason.ParseFailed);
			}

			if (success)
			{
				MethodInfo.Invoke(null, objectParams);
			}
		}
		
		private bool InformFailedParse(int argPos, FailReason reason)
		{
			switch (reason)
			{
				case FailReason.ParseFailed:
					DevConsole.LogError($"Failed to run command. Failed to parse arg at pos {argPos}.");
					break;
				case FailReason.NoArgsPassed:
					DevConsole.LogError("Failed to run command. This command requires args to be passed.");
					break;
				case FailReason.NotEnoughArgs:
					DevConsole.LogError("Failed to run command. Not enough args passed.");
					break;
				default:
					DevConsole.LogError($"Failed to run command. Arg type unknown or failed to parse an arg at pos {argPos}.");
					break;
			}
			
			return false;
		}
		
		private enum FailReason
		{
			/// <summary>
			/// Used when an argument is invalid.
			/// </summary>
			ParseFailed,
			/// <summary>
			/// Used when a command needs args to be passed but the user passed none.
			/// </summary>
			NoArgsPassed,
			/// <summary>
			/// Used when no enough args have been passed by the user.
			/// </summary>
			NotEnoughArgs
		}

		public override string ToString()
		{
			using(var sb = ZString.CreateStringBuilder())
			{
				foreach (var alias in Aliases)
				{
					sb.Append(alias);
				}
				
				sb.Append(Description);

				return sb.ToString();
			} 
		}
	}
}