using UnityEngine;

namespace DeveloperConsole
{
	[CreateAssetMenu(fileName = "ConsoleSettings", menuName = "Lyrebird/Console Settings", order = 0)]
	public class ConsoleSettings : ScriptableObject
	{
		public bool showOnStart = true;
		public bool logUnityLogs = false;
		public bool hideCursorOnClose = false;
		public CursorLockMode cursorLockModeOnClose = CursorLockMode.Confined;
		public KeyCode toggleKey = KeyCode.BackQuote;
		public KeyCode submitKey = KeyCode.Return;
		public KeyCode alternateSubmitKey = KeyCode.KeypadEnter;
		public string commandPrefix = ">>";
		public float fontSize = 15;
		public Color logColor = Color.white;
		public Color warningColor = Color.yellow;
		public Color errorColor = Color.red;
		public Color commandColor = Color.green;
		public Color consoleBackground = new Color(0.06f, 0.06f, 0.06f, 0.6f);
	}
}