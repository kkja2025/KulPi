using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RegisterMenu : Panel
{
    [SerializeField] private TMP_InputField emailInput = null;
    [SerializeField] private TMP_InputField passwordInput = null;
    [SerializeField] private TMP_InputField confirmPasswordInput = null;
    [SerializeField] private Button SignUpButton = null;
    [SerializeField] private Button BackButton = null;
    [SerializeField] private GameObject passwordWarning = null;
    [SerializeField] private RectTransform panelRectTransform;

    private Vector2 originalPosition; 

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        SignUpButton.onClick.AddListener(SignUp);
        BackButton.onClick.AddListener(Back);
        originalPosition = panelRectTransform.anchoredPosition;
        base.Initialize();
    }

    public override void Open()
    {
        panelRectTransform.anchoredPosition = originalPosition;
        emailInput.text = "";
        passwordInput.text = "";
        confirmPasswordInput.text = "";
        passwordWarning.gameObject.SetActive(false);
        base.Open();
    }

    private void SignUp()
    {
        string email = emailInput.text.Trim();
        string pass = passwordInput.text.Trim();
        string passConfirm = confirmPasswordInput.text.Trim();
        if (string.IsNullOrEmpty(pass) == false && string.IsNullOrEmpty(passConfirm) == false && string.IsNullOrEmpty(email) == false)
        {
            if (IsPasswordValid(pass) == false)
            {
                // LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password must be between 8 and 30 characters and contain at least one uppercase letter, one lowercase letter, one digit, and one symbol", "OK");
                passwordWarning.gameObject.SetActive(true);
            }
            else if (pass != passConfirm)
            {
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password do not match", "OK");
            }
            else
            {
                LoginManager.Singleton.SignUpAsync(email, pass);
            }
        }
        else
        {
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "All fields must be filled", "OK");
        }
    }

    private void Back()
    {
        PanelManager.GetSingleton("register").Close();
        PanelManager.GetSingleton("auth").Open();
    }

    private bool IsPasswordValid(string password)
    {
        if (password.Length < 8 || password.Length > 30)
        {
            return false;
        }
        
        bool hasUppercase = false;
        bool hasLowercase = false;
        bool hasDigit = false;
        bool hasSymbol = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasUppercase = true;
            }
            else if (char.IsLower(c))
            {
                hasLowercase = true;
            }
            else if (char.IsDigit(c))
            {
                hasDigit = true;
            }
            else if (!char.IsLetterOrDigit(c))
            {
                hasSymbol = true;
            }
        }
        return hasUppercase && hasLowercase && hasDigit && hasSymbol;
    }
}