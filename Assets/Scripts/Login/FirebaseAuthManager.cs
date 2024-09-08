using UnityEngine;
using Firebase;
using Firebase.Auth;
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

    public void RegisterUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("User creation canceled.");
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "User creation cancelled", "OK");
            }
            if (task.IsFaulted) {
                Debug.LogError("Error during user creation: " + task.Exception);
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Error during user creation", "OK");
            }

            AuthResult result = task.Result;
            FirebaseUser newUser = result.User;
            Debug.Log("User created successfully: " + newUser.Email);
        });
    }

    public void ResetPassword(string email)
    {
        auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("Password reset was canceled.");
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset was canceled.", "OK");
            }
            if (task.IsFaulted) {
                Debug.LogError("Password reset encountered an error: " + task.Exception);
                LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset encountered an error", "OK");
            }

            Debug.Log("Password reset email sent successfully.");
            LoginManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Password reset email sent successfully.", "OK");
        });
    }
}
