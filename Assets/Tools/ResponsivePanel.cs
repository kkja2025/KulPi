using UnityEngine;
using TMPro;

public class ResponsivePanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField[] inputFields; // Array to hold all your TMP_InputFields
    private RectTransform panelRectTransform;
    private float originalHeight;

    void Start()
    {
        // Store the original height of the panel
        panelRectTransform = GetComponent<RectTransform>();
        originalHeight = panelRectTransform.sizeDelta.y;
    }

    void Update()
    {
        // Check if any input field is selected (focused)
        bool isAnyInputFieldFocused = false;

        foreach (var inputField in inputFields)
        {
            if (inputField.isFocused)
            {
                isAnyInputFieldFocused = true;
                break;
            }
        }

        if (isAnyInputFieldFocused && TouchScreenKeyboard.visible)
        {
            // Adjust the position of the panel when the keyboard is visible
            panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, 100); // Adjust this value as needed
        }
        else
        {
            // Reset to the original position when the keyboard is not visible
            panelRectTransform.anchoredPosition = new Vector2(panelRectTransform.anchoredPosition.x, 0);
        }
    }
}
