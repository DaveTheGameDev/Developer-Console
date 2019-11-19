using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Debugging.DeveloperConsole
{
    public class UnityUserInterfaceOutput : MonoBehaviour, IConsoleOutput
    {
       
        [SerializeField] private bool openOnStart;
    
        [SerializeField] private bool logToUnityConsole;
        [SerializeField] private bool showUnityLogs;
        [SerializeField] private bool logToFile;
        [SerializeField] private bool hideCursorOnClose;
        [SerializeField]  private CursorLockMode cursorLockModeOnClose;
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private Transform panel;
        [SerializeField] private TextFitter element;
        [SerializeField] private string commandPrefix = ">";
        [SerializeField] private Color logColor = Color.white;
        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color errorColor = Color.red;
        [SerializeField] private Color commandColor = Color.green;

        private List<TextFitter> elements = new List<TextFitter>();
        public bool IsOpen { get; set; }
        
        private void Awake()
        {
            element.SetText("", 16, logColor);
            IsOpen = openOnStart;
            ConsoleSystem.Initialize(this);

            if (showUnityLogs)
            {
                Application.logMessageReceived += ApplicationOnLogMessageReceived;
            }

            if (inputField)
            {
                inputField.onSubmit.AddListener((text) =>
                {
                    //inputField.DeactivateInputField(true);
                    SelectInputField();
                    ExecuteCommand(text);
                });
                
                if(IsOpen)
                    SelectInputField();
            }
        }

        private void ExecuteCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                ConsoleSystem.ExecuteCommand(command);
            }
        }

        private void OnDisable()
        {
            if (showUnityLogs)
            {
                Application.logMessageReceived -= ApplicationOnLogMessageReceived;
            }
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

        private void OnValidate()
        {
            if(canvas)
                canvas.enabled = openOnStart;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote))
            {
                ToggleConsole();
            }
        }

        private void ToggleConsole()
        {
            IsOpen = !IsOpen;

            if (canvas)
                canvas.enabled = IsOpen;
            
            Cursor.visible = IsOpen || !hideCursorOnClose;
            Cursor.lockState = IsOpen ? CursorLockMode.Confined : cursorLockModeOnClose;
            
            if(IsOpen)
            {
                SelectInputField();
                ConsoleSystem.ConsoleOpened();
            }
            else
            {
                inputField.DeactivateInputField(true);
                ConsoleSystem.ConsoleClosed();
            }
        }

        public void Log(string message)
        {
            if(logToUnityConsole)
                Debug.Log(message);
            
            CreateText(message, LogLevel.Log);
        }

        public void LogWarning(string message)
        {
            if(logToUnityConsole)
                Debug.LogWarning(message);
            
            CreateText(message, LogLevel.Warning);
        }

        public void LogError(string message)
        {
            if(logToUnityConsole)
                Debug.LogError(message);
            
            CreateText(message, LogLevel.Error);
        }

        public void LogCommand(string message)
        {
            if(logToUnityConsole)
                Debug.Log(message);

            CreateText(commandPrefix + message, LogLevel.Command);
        }


        private void CreateText(string message, LogLevel logLevel)
        {
            Color color = Color.white;

            switch (logLevel)
            {
                case LogLevel.Log:
                    color = logColor;
                    break;
                case LogLevel.Warning:
                    color = warningColor;
                    break;
                case LogLevel.Error:
                    color = errorColor;
                    break;
                case LogLevel.Command:
                    color = commandColor;
                    break;
            }

            if (elements.Count == 1)
            {
                element.SetText(message, 16, color);
                elements.Add(element);
            }
            else
            {
                var newElement = Instantiate(element.gameObject, panel).GetComponent<TextFitter>();
                newElement.SetText(message, 16, color);
                elements.Add(newElement);
            }
        }
        
        public void SetSelectedEntityText(string input)
        {
            
        }

        public void SelectInputField()
        {
            if (!inputField)
            {
                return;
            }
			
            EventSystem.current.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnSelect(null);
            inputField.text = null;
        }

        public void Clear()
        {
            for (int i = panel.childCount - 1; i >= 1; i--)
            {
                Destroy(panel.GetChild(i).gameObject);
            }
            
            elements?.Clear();
            elements?.Add(element);
            element.SetText("", 16, logColor);
        }

        private enum LogLevel
        {
            Log,
            Warning,
            Error,
            Command
        }
    }
}
