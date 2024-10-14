using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Firebase;
using Firebase.Auth;

public class LoginManager : MonoBehaviour
{
    private bool initialized = false;
    private static LoginManager singleton = null;
    private FirebaseAuth auth;

    public static LoginManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<LoginManager>();
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

    public async void StartClientService()
    {
        int maxRetries = 3;  
        int retryDelay = 2000;  
        int attempt = 0;
        bool success = false;

        PanelManager.LoadSceneAsync("");

        while (attempt < maxRetries && !success)
        {
            attempt++;
            try
            {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    Debug.Log($"Attempt {attempt}: Unity Services not initialized. Initializing...");
                    await UnityServices.InitializeAsync();
                    Debug.Log("Unity Services initialized.");
                }

                var firebaseService = FirebaseService.Singleton;
                if (firebaseService == null)
                {
                    Debug.Log("FirebaseService singleton is null.");
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(retryDelay); 
                        continue; 
                    }
                    ShowPopUp(PopUpMenu.Action.StartService, "Failed to initialize Firebase.", "Retry");
                    return;
                }

                auth = firebaseService.Auth;
                if (auth == null)
                {
                    Debug.Log("FirebaseAuth instance is null.");
                    if (attempt < maxRetries)
                    {
                        await Task.Delay(retryDelay); 
                        continue; 
                    }
                    ShowPopUp(PopUpMenu.Action.StartService, "Failed to initialize Firebase Auth.", "Retry");
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
                
                success = true;  
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);

                if (attempt < maxRetries)
                {
                    Debug.Log($"Retrying in {retryDelay / 1000} seconds...");
                    await Task.Delay(retryDelay);  
                }
                else
                {
                    ShowPopUp(PopUpMenu.Action.StartService, "Failed to initialize after multiple attempts.", "Retry");
                }
            }
        }

        if (!success)
        {
            Debug.Log("Failed to initialize after multiple attempts.");
            ShowPopUp(PopUpMenu.Action.StartService, "Failed to initialize after multiple attempts.", "Retry");
        }
    }

    private async void AutomaticSignIn()
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null)
        {
            Debug.Log($"User is already signed in: {user.Email}");
            await LinkFirebaseWithUnity(user);
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
        try
        {
            await auth.SignInWithEmailAndPasswordAsync(email, password);
            await LinkFirebaseWithUnity(auth.CurrentUser);
        }
        catch (FirebaseException firebaseException)
        {
            HandleFirebaseAuthErrors(firebaseException);
        }
        catch (Exception exception)
        {
            Debug.Log("Error during sign in: " + exception.Message);
            ShowPopUp(PopUpMenu.Action.None, "Error during sign in", "OK");
        }
    }

    public async Task LinkFirebaseWithUnity(FirebaseUser firebaseUser)
    {
        try
        {
            PanelManager.LoadSceneAsync("MainMenu");
            string trimmedString = firebaseUser.UserId.Length > 20 ? firebaseUser.UserId.Substring(0, 20) : firebaseUser.UserId;
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(trimmedString, "A1b@C2d#Ef3G");
            Debug.Log("Successfully linked Firebase with Unity Authentication.");
        }
        catch (Exception e)
        {
            Debug.Log("Linking Firebase with Unity Authentication failed: " + e.Message);
            ShowPopUp(PopUpMenu.Action.StartService, "Linking Firebase with Unity Authentication failed", "OK");
        }
    }

    public async void SignUpAsync(string email, string password)
    {
        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            await SignUpUnityWithFirebase(newUser);
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

    public async Task SignUpUnityWithFirebase(FirebaseUser firebaseUser)
    {
        try
        {
            PanelManager.LoadSceneAsync("MainMenu");
            string trimmedString = firebaseUser.UserId.Length > 20 ? firebaseUser.UserId.Substring(0, 20) : firebaseUser.UserId;
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(trimmedString, "A1b@C2d#Ef3G");
            Debug.Log("Successfully linked Firebase account to Unity.");
        }
        catch (Exception e)
        {
            Debug.Log("Linking Firebase with Unity failed: " + e.Message);
            ShowPopUp(PopUpMenu.Action.StartService, "Linking Firebase with Unity failed", "OK");
        }
    }

    public async void RequestResetPasswordAsync(string email)
    {
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
    
    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }

    private void HandleFirebaseAuthErrors(FirebaseException firebaseException)
    {
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
}
