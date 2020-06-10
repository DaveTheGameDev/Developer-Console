using System;
using Cysharp.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

namespace Debugging.DeveloperConsole
{
	public class ConsoleUI : MonoBehaviour, IConsoleOutput
	{
		public bool IsOpen => consoleRoot.activeSelf;
		
		[SerializeField] private GameObject consoleRoot = null;
		[SerializeField] private GameObject contentPanel = null;
		[SerializeField] private TMP_InputField inputField = null;
		[SerializeField] private TextMeshProUGUI tmp = null;
		

		[SerializeField] private Color logColor = Color.white;
		[SerializeField] private Color logWarningColor = Color.yellow;
		[SerializeField] private Color logErrorColor = Color.red;
		[SerializeField] private Color commandColor = Color.green;

		[SerializeField] private bool   prefixTime = true;
		[SerializeField] private bool   prefixLabels = true;
		[SerializeField] private string logPrefix = "[LOG]";
		[SerializeField] private string warningPrefix = "[WARNING]";
		[SerializeField] private string errorPrefix = "[ERROR]";
		[SerializeField] private string commandPrefix = ">";
		
		private void Awake()
		{
			DevConsole.SetOutput(this);
			inputField.onSubmit.AddListener(SubmitInput);
			DontDestroyOnLoad(gameObject);
			
			DevConsole.LogMessage("Developer Console Initialized...", false);
			
			Application.logMessageReceived += UnityLogMessageReceived;
			Application.logMessageReceivedThreaded += UnityLogMessageReceived;
		}

		private void UnityLogMessageReceived(string condition, string stacktrace, LogType type)
		{
			switch (type)
			{
				case LogType.Assert:
				case LogType.Exception:
				case LogType.Error:
				{
					LogError(condition);
					break;
				}
				case LogType.Warning:
				{
					LogWarning(condition);
					break;
				}
				case LogType.Log:
				{
					LogMessage(condition, true);
					break;
				}
			}
		}

		private void OnDisable()
		{
			inputField.onSubmit.RemoveListener(SubmitInput);
		}

		private void SubmitInput(string input)
		{
			DevConsole.ExecuteCommand(input);
			inputField.text = null;
			EventSystem.current.SetSelectedGameObject(null);
			inputField.Select();
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				consoleRoot.SetActive(!consoleRoot.activeSelf);

				if (IsOpen)
				{
					inputField.Select();
					inputField.text = null;
				}
				else
				{
					EventSystem.current.SetSelectedGameObject(null);
				}
			}
		}

		

		public void LogMessage(string message, bool showTime)
		{
			using(var sb = ZString.CreateStringBuilder())
			{
				if(showTime)
				{
					if (prefixTime)
					{
						sb.Append(DateTime.Now.ToShortTimeString());
						sb.Append(' ');
					}
					
					if(prefixLabels)
						sb.Append(logPrefix);
				}
				
				sb.Append(message);
				
				var text = InstantiateText();
				text.color = logColor;
				text.SetText(sb);
			}
		}

		public void LogWarning(string message)
		{
			using(var sb = ZString.CreateStringBuilder())
			{
				if (prefixTime)
				{
					sb.Append(DateTime.Now.ToShortTimeString());
					sb.Append(' ');
				}
				
				if(prefixLabels)
					sb.Append(warningPrefix);
				
				sb.Append(message);
				
				var text = InstantiateText();
				text.color = logWarningColor;
				text.SetText(sb);
			}
		}

		public void LogError(string message)
		{
			using(var sb = ZString.CreateStringBuilder())
			{
				if (prefixTime)
				{
					sb.Append(DateTime.Now.ToShortTimeString());
					sb.Append(' ');
				}

				if(prefixLabels)
					sb.Append(errorPrefix);
				
				sb.Append(message);
				
				var text = InstantiateText();
				text.color = logErrorColor;
				text.SetText(sb);
			}
		}

		public void LogCommand(string command)
		{
			using(var sb = ZString.CreateStringBuilder())
			{
				sb.Append(commandPrefix);
				sb.Append(command);
				
				var text = InstantiateText();
				text.color = commandColor;
				text.SetText(sb);
			}
		}

		public void Clear()
		{
			for (var i = contentPanel.transform.childCount - 1; i >= 1; i--)
			{
				Destroy(contentPanel.transform.GetChild(i).gameObject);
			}
		}

		private TextMeshProUGUI InstantiateText()
		{
			return Instantiate(tmp.gameObject, contentPanel.transform).GetComponent<TextMeshProUGUI>();
		}
	}
}