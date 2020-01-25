using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeveloperConsole
{
    public class UnityUserInterfaceOutput : MonoBehaviour, IConsoleOutput
    {
        private static UnityUserInterfaceOutput instance;
        
        [SerializeField] private bool openOnStart;
    
        [SerializeField] private bool logToUnityConsole;
        [SerializeField] private bool showUnityLogs;
        [SerializeField] private bool logToFile;
        [SerializeField] private bool hideCursorOnClose;
        [SerializeField]  private CursorLockMode cursorLockModeOnClose;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Canvas canvas;
        [SerializeField] private InputField inputField;
        [SerializeField] private Transform panel;
        [SerializeField] private TextFitter element;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private string commandPrefix = ">";
        [SerializeField] private Color logColor = Color.white;
        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color errorColor = Color.red;
        [SerializeField] private Color commandColor = Color.green;
        [SerializeField] private int maxLogs = 50;
       
        
        private List<TextFitter> elements = new List<TextFitter>();
        private List<string> history = new List<string>();
        private int historyIndex = -1;
        
        public bool IsOpen { get; set; }
        public bool LogToFile { get; set; }

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject.transform.root.gameObject);
                return;
            }
            instance = this;
            
            historyIndex = -1;
            DontDestroyOnLoad(transform.root.gameObject);
            element.SetText("", 16, logColor);
            IsOpen = openOnStart;
            LogToFile = logToFile;
            
            if(!ConsoleSystem.IsInitialised())
                ConsoleSystem.Initialize(this);

            if (showUnityLogs)
            {
                Application.logMessageReceived += ApplicationOnLogMessageReceived;
            }

            if (inputField)
            {
                inputField.onEndEdit.AddListener((text) =>
                {
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
            string message = condition;
            switch (type)
            {
                case LogType.Error:
                    message = $"{condition}\n{stacktrace}";
                    ConsoleSystem.LogError(message);
                    break;
                case LogType.Warning:
                    ConsoleSystem.LogWarning(message);
                    break;
                case LogType.Log:
                    ConsoleSystem.Log(message);
                    break;
                case LogType.Exception:
                    message = $"{condition}\n{stacktrace}";
                    ConsoleSystem.LogError(message);
                    break;
            }
            
            if (LogToFile)
            {
                using (var file = File.AppendText(
                    Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Logs/output.log")))
                {
                    file.WriteLineAsync(message);
                }
            }
        }

        private void OnValidate()
        {
            if(canvas)
                canvas.enabled = openOnStart;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.F3))
            {
                ToggleConsole();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Prevents overlap to start of history.
                if (historyIndex < history.Count - 1)
                    historyIndex += 1;
                else
                    return;
                
                ApplyHistory();
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow) && historyIndex > 0)
            {
                historyIndex -= 1;
                ApplyHistory();
                return;
            }

            if (Input.anyKeyDown)
            {
                historyIndex = -1;
            }
        }

        private void ApplyHistory()
        {
            inputField.text = history[historyIndex];
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
                inputField.DeactivateInputField();
                inputField.text = null;
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

            history.Add(message);
            
            if(history.Count == maxLogs)
                history.RemoveAt(history.Count -1);

            historyIndex = -1;
            
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

            if (elements.Count < 1)
            {
                element.SetText(message, 16, color);
                elements.Add(element);
            }
            else if(elements.Count < maxLogs)
            {
                var newElement = Instantiate(element.gameObject, panel).GetComponent<TextFitter>();
                newElement.SetText(message, 16, color);
                elements.Add(newElement);
            }
            else
            {
                var newElement = elements[0];
                newElement.SetText(message, 16, color);
                elements.Remove(newElement);
                elements.Add(newElement);
                newElement.transform.SetAsLastSibling();
            }
            
            Canvas.ForceUpdateCanvases();
			
            if (scrollRect != null)
            {
                scrollRect.verticalScrollbar.value = 1;
                scrollRect.verticalNormalizedPosition = 0;
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
			
            eventSystem.SetSelectedGameObject(inputField.gameObject, null);
            inputField.OnSelect(null);
            inputField.text = null;
        }

        public void Clear()
        {
            for (int i = elements.Count - 1; i >= 1; i--)
            {
                Destroy(elements[i].gameObject);
            }
            
            elements?.Clear();
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
