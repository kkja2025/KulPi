using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class LoginManager : MonoBehaviour
{
    private bool initialized = false;
    private static LoginManager singleton = null;
    private FirebaseApp app;
    private FirebaseAuth auth;

    public static LoginManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<LoginManager>();
                singleton.Initialize();
            }
            return singleton; 
        }
    }

    private void Initialize()
    {
        if (initialized) { return; }
        initialized = true;
        DontDestroyOnLoad(gameObject);
    }
    
    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
    }
    
    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    public async void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        try
        {       
           var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase initialized successfully.");

                AutomaticSignIn();
            }
            else
            {
                Debug.Log($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                return;
            }

             if (auth.CurrentUser != null)
            {
                Debug.Log("User is signed in.");
                AutomaticSignIn();
            }
            else
            {
                Debug.Log("User is not signed in.");
                PanelManager.CloseAll();
                PanelManager.GetSingleton("auth").Open();
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception);
            ShowPopUp(PopUpMenu.Action.StartService, "Failed to start client.", "Retry");
        }
    }

    private void AutomaticSignIn()
    {
        PanelManager.GetSingleton("loading").Open();
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log($"User is already signed in: {user.Email}");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            Debug.Log("No user is signed in.");
            PanelManager.CloseAll();
            PanelManager.GetSingleton("auth").Open();
        }
    }

    public async void SignInAsync(string email, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            Debug.Log("Signed in successfully.");
            SignInConfirmAsync();
        }
        catch (FirebaseException firebaseException)
        {
            Debug.Log("Firebase error during sign in: " + firebaseException.Message);
            ShowPopUp(PopUpMenu.Action.None, "Email or password incorrect", "OK");
        }
        catch (Exception exception)
        {
            Debug.Log("Error during sign in: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Error during sign in", "OK");
        }
    }

    public async void SignUpAsync(string email, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("User created successfully: " + newUser.Email);
            ShowPopUp(PopUpMenu.Action.None, "User created successfully", "OK");
        }
        catch (FirebaseException firebaseException)
        {
            Debug.Log("Firebase error during user creation: " + firebaseException.Message);
            ShowPopUp(PopUpMenu.Action.None, "Email already registered", "OK");
        }
        catch (Exception exception)
        {
            Debug.Log("Error during user creation: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Error during user creation", "OK");
        }
    }

    public async void RequestResetPasswordAsync(string email)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            await auth.SendPasswordResetEmailAsync(email);
            Debug.Log("Password reset email sent successfully.");
            ShowPopUp(PopUpMenu.Action.None, "Password reset email sent successfully.", "OK");
        }
        catch (FirebaseException firebaseException)
        {
            Debug.Log("Firebase error during password reset: " + firebaseException.Message);
            ShowPopUp(PopUpMenu.Action.None, "Password reset encountered an error", "OK");
        }
        catch (Exception exception)
        {
            Debug.Log("Error during password reset: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Password reset encountered an error", "OK");
        }
    }

    public void SignOutUser()
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            if (auth.CurrentUser != null)
            {
                auth.SignOut();
                Debug.Log("Firebase user signed out.");
            }

            PanelManager.CloseAll();
            SceneManager.LoadScene("Login");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error during logout: {ex}");
        }
    }

    private void SignInConfirmAsync()
    {
            PanelManager.CloseAll();
            SceneManager.LoadScene("MainMenu");
    }
    
    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }

    public bool IsEmailValid(string email) 
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}