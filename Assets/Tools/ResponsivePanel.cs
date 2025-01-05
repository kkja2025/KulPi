using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResponsivePanel : MonoBehaviour
{
    [SerializeField] private RectTransform panelRectTransform;
    [SerializeField] private TMP_InputField[] inputFields; 
    [SerializeField] private float[] moveAmounts;
    [SerializeField] private Button returnButton;
    [SerializeField] private Button[] inputFieldButtons;

    private Vector2 originalPosition; 
    private bool isKeyboardOpen = false;

    private void Start()
    {
        originalPosition = panelRectTransform.anchoredPosition;

        if (moveAmounts.Length != inputFields.Length)
        {
            return;
        }

        if (inputFieldButtons.Length != inputFields.Length)
        {
            return;
        }

        for (int i = 0; i < inputFieldButtons.Length; i++)
        {
            int index = i; 
            inputFieldButtons[i].onClick.AddListener(() => OnInputFieldButtonClick(index));
        }

        returnButton.onClick.AddListener(ReturnToOriginalPosition);
    }

    private void Update()
    {
        if (TouchScreenKeyboard.visible)
        {
            isKeyboardOpen = true;
        }
        else if (isKeyboardOpen)
        {
            isKeyboardOpen = false;
            ReturnToOriginalPosition();
        }
    }

    private void OnInputFieldButtonClick(int index)
    {
        inputFields[index].Select();
        MovePanelToInputField(index);
    }

    private void MovePanelToInputField(int index)
    {
        float moveAmount = moveAmounts[index];
        panelRectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y + moveAmount);
    }

    private void ReturnToOriginalPosition()
    {
        panelRectTransform.anchoredPosition = originalPosition;
    }
}
