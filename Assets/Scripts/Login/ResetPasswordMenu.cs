using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResetPasswordMenu : Panel
{
    [SerializeField] private TMP_InputField newPasswordInput = null;
    [SerializeField] private TMP_InputField confirmPasswordInput = null;
    [SerializeField] private Button ResetButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ResetButton.onClick.AddListener(ResetPassword);
        base.Initialize();
    }

    public override void Open()
    {
        newPasswordInput.text = "";
        confirmPasswordInput.text = "";
        base.Open();
    }

    private void ResetPassword()
    {
        string newPassword = newPasswordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        if (string.IsNullOrEmpty(newPassword) == false && string.IsNullOrEmpty(confirmPassword) == false)
        {
            if (newPassword != confirmPassword)
            {
                LoginManager.Singleton.ShowError(ErrorMenu.Action.None, "Passwords do not match", "OK");
                Debug.Log("Passwords do not match");
                return;
            }
            // Reset password logic
            // LoginManager.Singleton.ResetPasswordAsync(newPassword, confirmPassword);
        }
    }
}
