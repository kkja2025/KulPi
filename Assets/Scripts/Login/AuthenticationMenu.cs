using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AuthenticationMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private Button SignInButton = null;
    [SerializeField] private Button SignUpButton = null;
    [SerializeField] private Button ForgotPasswordButton = null;
    [SerializeField] private RectTransform panelRectTransform;

    private Vector2 originalPosition; 

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        ForgotPasswordButton.onClick.AddListener(ForgotPassword);
        SignInButton.onClick.AddListener(SignIn);
        SignUpButton.onClick.AddListener(SignUp);
        originalPosition = panelRectTransform.anchoredPosition;
        base.Initialize();
    }

    public override void Open()
    {
        panelRectTransform.anchoredPosition = originalPosition;
        emailInput.text = "";
        passwordInput.text = "";
        base.Open();
    }

    private void ForgotPassword()
    {
        PanelManager.GetSingleton("auth").Close();
        PanelManager.GetSingleton("forgot").Open();
    }

    private void SignIn()
    {
        string email = emailInput.text.Trim();
        string pass = passwordInput.text.Trim();
        if (string.IsNullOrEmpty(email) == false && string.IsNullOrEmpty(pass) == false)
        {
            LoginManager.Singleton.SignInAsync(email, pass);
        } else
        {
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Email and password are required", "OK");
        }
    }

    private void SignUp()
    {
        PanelManager.GetSingleton("auth").Close();
        PanelManager.GetSingleton("register").Open();

    }
}