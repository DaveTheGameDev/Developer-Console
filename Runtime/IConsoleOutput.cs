namespace Debugging.DeveloperConsole
{
	public interface IConsoleOutput
	{
		bool IsOpen { get; }
		void LogMessage(string message, bool showTime);
		void LogWarning(string message);
		void LogError(string message);
		void LogCommand(string command);
		void Clear();
	}
}