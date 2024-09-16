using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowHidePassword : MonoBehaviour
{
    public TMP_InputField passwordInputField;
    public Button showHideButton;
    public GameObject hideIcon;
    public GameObject showIcon;
    private bool isPasswordVisible = false;

    void Start()
    {
        hideIcon.SetActive(false);
        showIcon.SetActive(true);

        showHideButton.onClick.AddListener(TogglePasswordVisibility);

        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate();
    }
    public void TogglePasswordVisibility()
    {
        isPasswordVisible = !isPasswordVisible;

        if (isPasswordVisible)
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            hideIcon.SetActive(true);
            showIcon.SetActive(false);
        }
        else
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            hideIcon.SetActive(false);
            showIcon.SetActive(true);
        }
        passwordInputField.ForceLabelUpdate();
    }
}