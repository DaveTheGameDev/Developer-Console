namespace Lyrebird.Debugging.Console
{
	/// <summary>
	/// Used to route messages to a console GUI.
	/// </summary>
	public interface IConsoleOutput
	{
		bool IsOpen { get; set; }
		
		/// <summary>
		/// Writes a message to the console.
		/// </summary>
		/// <param name="logMessage"></param>
		void Log(string logMessage);
		
		/// <summary>
		/// Writes a warning message to the console.
		/// </summary>
		/// <param name="warningMessage"></param>
		void LogWarning(string warningMessage);
		
		/// <summary>
		/// Writes an error message to the console.
		/// </summary>
		/// <param name="errorMessage"></param>
		void LogError(string errorMessage);
		
		/// <summary>
		/// Writes the users command input to the console.
		/// </summary>
		/// <param name="command"></param>
		void LogCommand(string command);
		
		void SetSelectedEntityText(string input);
		void SelectInputField();
		
		/// <summary>
		/// Clear the console GUI of messages.
		/// </summary>
		void Clear();
	}
}