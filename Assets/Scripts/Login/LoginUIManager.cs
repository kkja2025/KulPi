using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginUIManager : MonoBehaviour
{
    public InputField emailInputField;
    public InputField passwordInputField;
    public Button loginButton;
    public Button registerButton;
    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClick);
        registerButton.onClick.AddListener(OnRegisterButtonClick);
    }

    void OnLoginButtonClick()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;

        if (IsValidEmail(email) && IsValidPassword(password))
        {
            SceneManager.LoadScene("GameScene");
        }
        else
        {
            Debug.Log("Invalid email or password");
        }
    }

    void OnRegisterButtonClick()
    {
        SceneManager.LoadScene("RegisterScene");
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
