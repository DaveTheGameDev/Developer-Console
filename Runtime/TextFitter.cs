using TMPro;
using UnityEngine;

namespace DeveloperConsole
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextFitter : MonoBehaviour
    {
        private TextMeshProUGUI textComponent;
        private RectTransform rectTransform;

        public void SetText(string text, float fontSize, Color color)
        {
            textComponent = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
            textComponent.autoSizeTextContainer = true;

            // Get the size of the text for the given string.
            Vector2 textSize = textComponent.GetPreferredValues(text);
            // Adjust the button size / scale.
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, textSize.y + 5);
            textComponent.text = text;
            textComponent.color = color;
            textComponent.fontSize = fontSize;
        }
    }
}