using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class MainMenuManager : MonoBehaviour
{
    private bool initialized = false;
    private DatabaseReference reference;
    private static MainMenuManager singleton = null;

    public static MainMenuManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<MainMenuManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("MainMenuManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    private void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        try
        {
            reference = FirebaseService.Singleton.Reference;
            if (reference != null)
            {
                Debug.Log("Connected to the database.");
                PanelManager.GetSingleton("loading").Close();
                PanelManager.GetSingleton("main").Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("Error connecting to Firebase: " + exception);
            PanelManager.GetSingleton("loading").Close();
        }
    }

    public void SignOut()
    {
        PanelManager.GetSingleton("loading").Open();
        var auth = FirebaseAuth.DefaultInstance;
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("User signed out.");
            SceneManager.LoadScene("Login");
        }
    }

    public void LoadGame()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        SceneManager.LoadScene("GameScene");
    }

    public void NewGame()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        // TODO: Add code to reset the game state.
        SceneManager.LoadScene("GameScene");
    }
}
