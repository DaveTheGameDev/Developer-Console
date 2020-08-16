using System;
using System.Reflection;
using System.Text;
using AstrayEngine;

namespace DeveloperConsole
{
    public readonly struct CommandData
    {
        public readonly string Alias;
        public readonly string Description;
        private readonly MethodInfo methodInfo;
        private readonly Type[] parameters;
        
        public CommandData(string alias, string description, MethodInfo methodInfo, Type[] parameters)
        {
            Alias = alias;
            this.Description = description;
            this.methodInfo = methodInfo;
            this.parameters = parameters;
        }
        
        public void Execute(string[] args)
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
				// Increase next arg index here to make sure indexing order does not go out of sync.
				nextArg++;
				
				// If not enough args have been passed parse failed. Inform user.
				if (nextArg >= args.Length)
				{
					success = InformFailedParse(i, FailReason.NotEnoughArgs);
					break;
				}

				if (DevConsoleHelper.TryParseBool(parameters[i], args[nextArg], out var boolValue))
				{
					objectParams[i] = boolValue;
					continue;
				}

				if (DevConsoleHelper.TryParseByte(parameters[i], args[nextArg], out var byteValue))
				{
					objectParams[i] = byteValue;
					continue;
				}

				if (DevConsoleHelper.TryParseSByte(parameters[i], args[nextArg], out var sbyteValue))
				{
					objectParams[i] = sbyteValue;
					continue;
				}

				if (DevConsoleHelper.TryParseChar(parameters[i], args[nextArg], out var charValue))
				{
					objectParams[i] = charValue;
					continue;
				}

				if (DevConsoleHelper.TryParseDecimal(parameters[i], args[nextArg], out var decimalValue))
				{
					objectParams[i] = decimalValue;
					continue;
				}

				if (DevConsoleHelper.TryParseFloat(parameters[i], args[nextArg], out var floatValue))
				{
					objectParams[i] = floatValue;
					continue;
				}
				
				if (DevConsoleHelper.TryParseDouble(parameters[i], args[nextArg], out var doubleValue))
				{
					objectParams[i] = doubleValue;
					continue;
				}

				if (DevConsoleHelper.TryParseInt(parameters[i], args[nextArg], out var intValue))
				{
					objectParams[i] = intValue;
					continue;
				}

				if (DevConsoleHelper.TryParseUInt(parameters[i], args[nextArg], out var uintValue))
				{
					objectParams[i] = uintValue;
					continue;
				}

				if (DevConsoleHelper.TryParseLong(parameters[i], args[nextArg], out var longValue))
				{
					objectParams[i] = longValue;
					continue;
				}

				if (DevConsoleHelper.TryParseULong(parameters[i], args[nextArg], out var ulongValue))
				{
					objectParams[i] = ulongValue;
					continue;
				}

				if (DevConsoleHelper.TryParseShort(parameters[i], args[nextArg], out var shortValue))
				{
					objectParams[i] = shortValue;
					continue;
				}

				if (DevConsoleHelper.TryParseUshort(parameters[i], args[nextArg], out var ushortValue))
				{
					objectParams[i] = ushortValue;
					continue;
				}

				if (DevConsoleHelper.TryParseString(parameters[i]))
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
			var sb = new StringBuilder();
			sb.Append(Alias);
			sb.Append(": ");
			sb.Append(Description);
			return sb.ToString();
		}
    }
}