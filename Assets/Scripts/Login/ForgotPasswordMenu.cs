using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ForgotPasswordMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private Button RequestButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        RequestButton.onClick.AddListener(RequestPasswordReset);
        base.Initialize();
    }

    public override void Open()
    {
        emailInput.text = "";
        base.Open();
    }

    private void RequestPasswordReset()
    {
        string email = emailInput.text;
        if (string.IsNullOrEmpty(email))
        {
            return;
        }
    }
}
