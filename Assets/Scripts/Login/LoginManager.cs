using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;

public class LoginManager : MonoBehaviour
{
    private bool initialized = false;
    private static LoginManager singleton = null;
    private FirebaseAuth auth;
    private const int MaxRetries = 3;
    private int retryCount = 0;

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

    public void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        AttemptInitializeFirebase();
    }

    private async void AttemptInitializeFirebase()
    {
        retryCount = 0; // Reset retry count
        while (retryCount < MaxRetries)
        {
            try
            {
                var firebaseService = FirebaseService.Singleton;

                if (firebaseService == null)
                {
                    Debug.LogError("FirebaseService singleton is null.");
                    retryCount++;
                    await RetryInitialization();
                    continue;
                }

                auth = firebaseService.Auth;

                if (auth == null)
                {
                    Debug.LogError("FirebaseAuth instance is null.");
                    retryCount++;
                    await RetryInitialization();
                    continue;
                }

                if (auth.CurrentUser != null)
                {
                    Debug.Log("User is signed in.");
                    AutomaticSignIn();
                    return;
                }
                else
                {
                    Debug.Log("User is not signed in.");
                    PanelManager.CloseAll();
                    PanelManager.GetSingleton("auth").Open();
                    return;
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                retryCount++;
                await RetryInitialization();
            }
        }

        // If max retries reached and still not successful
        ShowPopUp(PopUpMenu.Action.StartService, "Failed to start client after multiple attempts.", "Retry");
    }

    private async Task RetryInitialization()
    {
        // Close the loading panel and wait before retrying
        PanelManager.GetSingleton("loading").Close();
        await Task.Delay(2000); // Wait for 2 seconds before retrying
        PanelManager.GetSingleton("loading").Open();
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
            // Handle specific Firebase error codes
            switch ((AuthError)firebaseException.ErrorCode)
            {
                case AuthError.InvalidEmail:
                    ShowPopUp(PopUpMenu.Action.None, "Invalid email format", "OK");
                    break;
                case AuthError.WrongPassword:
                    ShowPopUp(PopUpMenu.Action.None, "Incorrect password", "OK");
                    break;
                case AuthError.UserNotFound:
                    ShowPopUp(PopUpMenu.Action.None, "User not found", "OK");
                    break;
                default:
                    ShowPopUp(PopUpMenu.Action.None, "Login failed. Please try again.", "OK");
                    break;
            }
            Debug.Log("Firebase error during sign in: " + firebaseException.Message);
        }
        catch (Exception exception)
        {
            Debug.Log("Error during sign in: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Error during sign in", "OK");
        }
        finally
        {
            PanelManager.Close("loading");
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
            switch ((AuthError)firebaseException.ErrorCode)
            {
                case AuthError.EmailAlreadyInUse:
                    ShowPopUp(PopUpMenu.Action.None, "Email already registered", "OK");
                    break;
                case AuthError.InvalidEmail:
                    ShowPopUp(PopUpMenu.Action.None, "Invalid email format", "OK");
                    break;
                default:
                    ShowPopUp(PopUpMenu.Action.None, "Error during user creation", "OK");
                    break;
            }
            Debug.Log("Firebase error during user creation: " + firebaseException.Message);
        }
        catch (Exception exception)
        {
            Debug.Log("Error during user creation: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Error during user creation", "OK");
        }
        finally
        {
            PanelManager.Close("loading");
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
        finally
        {
            PanelManager.Close("loading");
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
}