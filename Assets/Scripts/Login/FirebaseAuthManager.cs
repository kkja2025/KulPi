using UnityEngine;
using Firebase;
using Firebase.Auth;
using System;
using System.Threading.Tasks;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private bool initialized = false;
    private static FirebaseAuthManager singleton = null;

    public static FirebaseAuthManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindFirstObjectByType<FirebaseAuthManager>();
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

    void Awake()
    {
        Application.runInBackground = true;
        auth = FirebaseAuth.DefaultInstance;
    }

    public async void RegisterUser(string email, string password)
    {
        PanelManager.GetSingleton("loading").Open();
        try
        {
            var authResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);

            FirebaseUser newUser = authResult.User;
            Debug.Log("User created successfully: " + newUser.Email);
            PanelManager.GetSingleton("link").Close();
            PanelManager.GetSingleton("register").Open();
        }
        catch (FirebaseException firebaseException)
        {
            Debug.Log("Firebase error during user creation: " + firebaseException.Message);
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Email already registered", "OK");
        }
        catch (Exception exception)
        {
            Debug.Log("Error during user creation: " + exception.Message);
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Error during user creation", "OK");
        }
    }

    public void ResetPassword(string email)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.Log("Password reset was canceled.");
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset was canceled.", "OK");
            }
            if (task.IsFaulted) {
                Debug.Log("Password reset encountered an error: " + task.Exception);
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset encountered an error", "OK");
            }

            Debug.Log("Password reset email sent successfully.");
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset email sent successfully.", "OK");
        });
    }
}
