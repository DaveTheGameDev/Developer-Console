using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = System.Object;

//TODO: Add Command Auto Complete
namespace Lyrebird.Debugging.Console
{
	[RequireComponent(typeof(Canvas))]
	public class ConsoleGui : MonoBehaviour, IConsoleOutput
	{
		[SerializeField] private ConsoleSettings consoleSettings = null;
		[SerializeField] private Canvas canvas = null;
		[SerializeField] private Image backgroundImage = null;
		[SerializeField] private GameObject consoleElement = null;
		[SerializeField] private Transform outputPanel = null;
		[SerializeField] private TextMeshProUGUI selectedEntityText = null;
		[SerializeField] private TMP_InputField inputField = null;
		[SerializeField] private ScrollRect scrollRect = null;
		
		private readonly List<GameObject> elements = new List<GameObject>();
		private readonly List<string> history = new List<string>();
		private int historyIndex = 0;

		public bool IsOpen { get; set; }
		
		private void Awake()
		{
			if (ConsoleSystem.IsInitialised())
			{
				return;
			}
			
			if (consoleSettings == null)
			{
				Debug.LogError("Please provide a console settings object.");
				return;
			}

			ConsoleSystem.Initialize(this);
			canvas.enabled = consoleSettings.showOnStart;

			if (canvas.enabled)
			{
				ConsoleSystem.ConsoleOpened();
			}
			else
			{
				ConsoleSystem.ConsoleClosed();
			}

			IsOpen = canvas.enabled;
			
			if (backgroundImage != null)
			{
				backgroundImage.color = consoleSettings.consoleBackground;
			}
			
			if (canvas.enabled)
			{
				SelectInputField(true);
			}
			
			if (consoleSettings.logUnityLogs)
			{
				Application.logMessageReceived += ApplicationOnLogMessageReceived;
			}
		}

		private void OnDisable()
		{
			if (consoleSettings.logUnityLogs)
			{
				Application.logMessageReceived -= ApplicationOnLogMessageReceived;
			}
			EventSystem.current?.SetSelectedGameObject(null, null);
		}

		private void ApplicationOnLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			switch (type)
			{
				case LogType.Error:
					ConsoleSystem.LogError($"{condition}\n{stacktrace}");
					break;
				case LogType.Warning:
					ConsoleSystem.LogWarning(condition);
					break;
				case LogType.Log:
					ConsoleSystem.Log(condition);
					break;
				case LogType.Exception:
					ConsoleSystem.LogError($"{condition}\n{stacktrace}");
					break;
			}
		}

		private void Update()
		{
			if (Input.GetKeyDown(consoleSettings.toggleKey))
			{
				ToggleConsole();
			}

			if (Input.GetKeyDown(consoleSettings.submitKey) || Input.GetKeyDown(consoleSettings.alternateSubmitKey))
			{
				ExecuteCommand();
			}

			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				ApplyHistory(KeyCode.UpArrow);
			}

			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				ApplyHistory(KeyCode.DownArrow);
			}
		}

		private void ToggleConsole()
		{
			IsOpen = !canvas.enabled;
			canvas.enabled = IsOpen; 

			inputField.DeactivateInputField(true);

			Cursor.visible = IsOpen || !consoleSettings.hideCursorOnClose;
			Cursor.lockState = IsOpen ? CursorLockMode.Confined : consoleSettings.cursorLockModeOnClose;

			if (canvas.enabled)
			{
				SelectInputField(true);
				ConsoleSystem.ConsoleOpened();
			}
			else
			{
				inputField.DeactivateInputField(true);
				ConsoleSystem.ConsoleClosed();
			}
		}

		private void ExecuteCommand()
		{
			string command = inputField.text;
			history.Add(command);

			if (!string.IsNullOrEmpty(command))
			{
				ConsoleSystem.ExecuteCommand(command);
			}

			SelectInputField(true);
		}

		private void SelectInputField(bool clear)
		{
			if (!inputField)
			{
				return;
			}
			
			EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
			inputField.OnSelect(null);

			if (clear)
			{
				inputField.text = null;
			}
		}

		

		public void Log(string logMessage)
		{
			WriteToConsole(LogLevel.Log, logMessage);
		}

		public void LogWarning(string warningMessage)
		{
			WriteToConsole(LogLevel.Warning, warningMessage);
		}

		public void LogError(string errorMessage)
		{
			WriteToConsole(LogLevel.Error, errorMessage);
		}

		public void LogCommand(string command)
		{
			WriteToConsole(LogLevel.Command, $"{consoleSettings.commandPrefix} {command}");
		}

		public void SetSelectedEntityText(string input)
		{
			if (selectedEntityText)
			{
				selectedEntityText.text = input;
			}
		}

		public void SelectInputField()
		{
			SelectInputField(false);
		}

		public void Clear()
		{
			for (int i = elements.Count - 1; i >= 1; i--)
			{
				Destroy(elements[i]);
			}

			consoleElement.GetComponent<TextMeshProUGUI>().text = "";
			elements.Clear();
		}

		private void WriteToConsole(LogLevel logLevel, string message)
		{
			var element = consoleElement;
			
			// Create text element
			if (elements.Count > 0)
			{
				element = Instantiate(consoleElement, outputPanel);
			}

			if (element == null)
			{
				return;
			}

			var textFitter = element.GetComponent<TextFitter>();

			// Change element color and text to reflect message type
			switch (logLevel)
			{
				case LogLevel.Log:
					textFitter.SetText(message, consoleSettings.fontSize, consoleSettings.logColor);
					break;
				case LogLevel.Warning:
					textFitter.SetText(message, consoleSettings.fontSize,consoleSettings.warningColor);
					break;
				case LogLevel.Error:
					textFitter.SetText(message, consoleSettings.fontSize,consoleSettings.errorColor);
					break;
				case LogLevel.Command:
					textFitter.SetText(message, consoleSettings.fontSize,consoleSettings.commandColor);
					break;
			}

			elements.Add(element.gameObject);
			
			Canvas.ForceUpdateCanvases();
			
			if (scrollRect != null)
			{
				scrollRect.verticalScrollbar.value = 1;
				scrollRect.verticalNormalizedPosition = 0;
			}
		}

		private void ApplyHistory(KeyCode keyCode)
		{
			if (history.Count == 0)
			{
				return;
			}

			//Reset index to most recent index if input text does not match history at index.
			if (historyIndex < history.Count && inputField.text != history[historyIndex])
			{
				historyIndex = history.Count;
			}

			if (keyCode == KeyCode.UpArrow)
			{
				if (historyIndex > 0)
				{
					--historyIndex;
				}
			}

			if (keyCode == KeyCode.DownArrow)
			{
				if (historyIndex < history.Count - 1)
				{
					++historyIndex;
				}
			}

			//Set text if index is valid.
			if (historyIndex >= 0 && historyIndex < history.Count)
			{
				inputField.text = history[historyIndex];
				SelectInputField(false);
			}
		}


#if UNITY_EDITOR
		private void OnValidate()
		{
			if (canvas.enabled != consoleSettings.showOnStart)
			{
				canvas.enabled = consoleSettings.showOnStart;
			}
		}
#endif

		private enum LogLevel
		{
			Log,
			Warning,
			Error,
			Command
		}
	}
}