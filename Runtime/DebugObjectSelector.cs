using UnityEngine;

namespace DeveloperConsole
{
	public class DebugObjectSelector : MonoBehaviour
	{
		public static GameObject SelectedGameObject { get; private set; }
		private bool active;
		private Camera cam;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void Init()
		{
			if (FindObjectOfType<DebugObjectSelector>() != null)
			{
				return;
			}
			
			var go = new GameObject("DebugObjectSelector", typeof(DebugObjectSelector));
			DontDestroyOnLoad(go);
		}
		
		private void Awake()
		{
			active = ConsoleSystem.IsOpen();
			cam = Camera.main;
			ConsoleSystem.ConsoleVisibilityChanged += ConsoleVisibilityChanged;
			ConsoleSystem.SetSelectedEntityText(null);
		}

		private void ConsoleVisibilityChanged(bool isOpen)
		{
			active = isOpen;
			DeselectObject();
			
			if (active && !cam)
			{
				cam = FindObjectOfType<Camera>();
			}
		}

		private void Update()
		{
			if (!active || !cam)
			{
				return;
			}

			// Deselect object if right mouse is pressed.
			if (Input.GetMouseButtonDown(1))
			{
				DeselectObject();
			}
			
			if (!Input.GetMouseButtonDown(0))
			{
				return;
			}
			
			ConsoleSystem.SelectInputField();
			
			// Check to see if we hit an object.
			if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit) || !hit.transform)
			{
				DeselectObject();
				return;
			}
			
			// Make sure we select the root object.
			var root = hit.transform.root;

			if (root && SelectedGameObject == root.gameObject)
			{
				SelectedGameObject = hit.transform.gameObject;
			}
			else
			{
				SelectedGameObject = root.gameObject;
			}

			if (SelectedGameObject != null)
			{
				ConsoleSystem.SetSelectedEntityText(SelectedGameObject.name);
			}
		}

		private void DeselectObject()
		{
			SelectedGameObject = null;
			ConsoleSystem.SetSelectedEntityText(null);
			ConsoleSystem.SelectInputField();
		}
	}
}