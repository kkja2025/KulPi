using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeTextColor : MonoBehaviour
{
    public Color newColor; // The color you want to change to

    void Start()
    {
        // Change color for all Unity Text components
        Text[] allTexts = FindObjectsOfType<Text>();
        foreach (Text text in allTexts)
        {
            text.color = newColor;
        }

        // Change color for all TextMeshPro components
        TextMeshProUGUI[] allTextMeshes = FindObjectsOfType<TextMeshProUGUI>();
        foreach (TextMeshProUGUI textMesh in allTextMeshes)
        {
            textMesh.color = newColor;
        }
    }
}
