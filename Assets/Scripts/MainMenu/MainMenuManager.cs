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

    public async void LoadGame()
    {
        try
        {
            bool dataExists = await PlayerDataManager.Singleton.CheckForSaveDataAsync();
            if (dataExists)
            {
                Debug.Log("Save data found. Loading game...");
                PlayerData playerData = await PlayerDataManager.Singleton.LoadPlayerDataAsync();
                if (playerData != null)
                {
                    Debug.Log($"Loaded Player Data - Level: {playerData.level}, Score: {playerData.score}");
                    PanelManager.GetSingleton("loading").Open();
                    SceneManager.LoadScene("GameScene");
                }
                else
                {
                    Debug.LogError("Failed to load player data.");
                    ShowPopUp(PopUpMenu.Action.None, "Failed to load player data. Please try again.", "OK");
                }
            }
            else
            {
                Debug.Log("No save data found. Please start a new game.");
                ShowPopUp(PopUpMenu.Action.None, "No save data found. Please start a new game.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error during LoadGame: " + ex.Message);
            ShowPopUp(PopUpMenu.Action.None, "An error occurred while loading the game. Please try again.", "OK");
        }
    }
    public void NewGame()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        PlayerDataManager.Singleton.NewGame();
        SceneManager.LoadScene("GameScene");
    }

    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }
}
