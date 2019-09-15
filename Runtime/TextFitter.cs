using System;
using TMPro;
using UnityEngine;

namespace Lyrebird.Debugging.Console
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextFitter : MonoBehaviour
	{
		private TextMeshProUGUI textComponent;
		private RectTransform rectTransform;

		public void SetText(string text, float fontSize, Color color)
		{
			textComponent = GetComponent<TextMeshProUGUI>();
			textComponent.autoSizeTextContainer = true;
			rectTransform = GetComponent<RectTransform>();
			
			// Get the size of the text for the given string.
			Vector2 textSize = textComponent.GetPreferredValues(text);
			// Adjust the button size / scale.
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, textSize.y);
			textComponent.text = text;
			textComponent.color = color;
			textComponent.fontSize = fontSize;
		}
	}
}