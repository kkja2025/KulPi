using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterUIManager : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;
    public Button registerButton;
    public Button backButton;
    void Start()
    {
        registerButton.onClick.AddListener(OnRegisterButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    void OnRegisterButtonClick()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        if (IsValidEmail(email) && IsValidPassword(password) && password == confirmPassword)
        {
            SceneManager.LoadScene("Login");
        }
        else if (password != confirmPassword)
        {
            Debug.Log("Passwords do not match");
        }
        else
        {
            Debug.Log("Invalid email or password");
        }
    }

    void OnBackButtonClick()
    {
        SceneManager.LoadScene("Login");
    }

    bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    bool IsValidPassword(string password)
    {
        return password.Length >= 8;
    }
}
