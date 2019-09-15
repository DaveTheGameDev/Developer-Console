using System;
using System.Reflection;
using UnityEngine;

namespace Lyrebird.Debugging.Console
{
	internal struct CommandData
	{
		/// <summary>
		/// The name of the command.
		/// </summary>
		public readonly string Name;
		
		/// <summary>
		/// The commands description.
		/// This should contain information on what the command does.
		/// </summary>
		public readonly string Description;
		
		/// <summary>
		/// The commands usage.
		/// This should contain information on how to use it.
		/// </summary>
		public readonly string Usage;
		
		/// <summary>
		/// A read only array of types for this command. Used to help parse user input args for method info.
		/// </summary>
		private readonly Type[] parameters;
		
		/// <summary>
		/// Method info for this command. Method info should be a static method.
		/// </summary>
		private readonly MethodInfo methodInfo;
		
		public CommandData(string name, string description, string usage, Type[] parameters, MethodInfo methodInfo)
		{
			Name = name.ToLower();
			Description = description;
			Usage = usage;
			this.parameters = parameters;
			this.methodInfo = methodInfo;
			
		}

		/// <summary>
		/// Parses command args 
		/// </summary>
		/// <param name="args"></param>
		public void Invoke(string[] args)
		{
			bool success = true;

			if (parameters == null || parameters.Length == 0)
			{
				methodInfo.Invoke(null, null);
				return;
			}
			
			object[] objectParams = new object[parameters.Length];

			int nextArg = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				// Check if this is a game object
				if (parameters[i] == ConsoleHelper.GameObjectType)
				{
					//Since selected objects don't need to be passed in the command we grab it from the selector -
					// and set next index to be i -1 (example command "setcolor red") red index = 1
					
					objectParams[i] = DebugObjectSelector.SelectedGameObject;
					nextArg = i -1;
					nextArg = Mathf.Max(nextArg, 0);
					continue;
				}

				// Increase next arg index here to make sure indexing order does not go out of sync.
				nextArg++;
				
				// If not enough args have been passed parse failed. Inform user.
				if (nextArg >= args.Length)
				{
					success = InformFailedParse(i, FailReason.NotEnoughArgs);
					break;
				}

				if (ConsoleHelper.TryParseVector3(parameters[i], args[nextArg], out Vector3 vector3))
				{
					objectParams[i] = vector3;
					continue;
				}
				
				if (ConsoleHelper.TryParseColor(parameters[i], args[nextArg], out Color color))
				{
					objectParams[i] = color;
					continue;
				}

				if (ConsoleHelper.TryParseBool(parameters[i], args[nextArg], out bool boolean))
				{
					objectParams[i] = boolean;
					continue;
				}

				if (ConsoleHelper.TryParseByte(parameters[i], args[nextArg], out byte byteVal))
				{
					objectParams[i] = byteVal;
					continue;
				}

				if (ConsoleHelper.TryParseSByte(parameters[i], args[nextArg], out sbyte sbyteVal))
				{
					objectParams[i] = sbyteVal;
					continue;
				}

				if (ConsoleHelper.TryParseChar(parameters[i], args[nextArg], out char charVal))
				{
					objectParams[i] = charVal;
					continue;
				}

				if (ConsoleHelper.TryParseDecimal(parameters[i], args[nextArg], out decimal decimalValue))
				{
					objectParams[i] = decimalValue;
					continue;
				}

				if (ConsoleHelper.TryParseFloat(parameters[i], args[nextArg], out float floatValue))
				{
					objectParams[i] = floatValue;
					continue;
				}

				if (ConsoleHelper.TryParseInt(parameters[i], args[nextArg], out int intValue))
				{
					objectParams[i] = intValue;
					continue;
				}

				if (ConsoleHelper.TryParseUInt(parameters[i], args[nextArg], out uint uintValue))
				{
					objectParams[i] = uintValue;
					continue;
				}

				if (ConsoleHelper.TryParseLong(parameters[i], args[nextArg], out long longValue))
				{
					objectParams[i] = longValue;
					continue;
				}

				if (ConsoleHelper.TryParseULong(parameters[i], args[nextArg], out ulong ulongValue))
				{
					objectParams[i] = ulongValue;
					continue;
				}

				if (ConsoleHelper.TryParseShort(parameters[i], args[nextArg], out short shortValue))
				{
					objectParams[i] = shortValue;
					continue;
				}

				if (ConsoleHelper.TryParseUshort(parameters[i], args[nextArg], out ushort ushortValue))
				{
					objectParams[i] = ushortValue;
					continue;
				}

				if (ConsoleHelper.TryParseString(parameters[i], args[nextArg]))
				{
					objectParams[i] = args[nextArg];
					continue;
				}

				
				
				// Failed to parse command.
				success = InformFailedParse(i, FailReason.ParseFailed);
			}

			if (success)
			{
				methodInfo.Invoke(null, objectParams);
			}
		}

		private bool InformFailedParse(int argPos, FailReason reason)
		{
			switch (reason)
			{
				case FailReason.ParseFailed:
					ConsoleSystem.LogError($"Failed to run command. Failed to parse arg at pos {argPos}.");
					break;
				case FailReason.NoArgsPassed:
					ConsoleSystem.LogError("Failed to run command. This command requires args to be passed.");
					break;
				case FailReason.NotEnoughArgs:
					ConsoleSystem.LogError("Failed to run command. Not enough args passed.");
					break;
				default:
					ConsoleSystem.LogError($"Failed to run command. Arg type unknown or failed to parse an arg at pos {argPos}.");
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
	}
}