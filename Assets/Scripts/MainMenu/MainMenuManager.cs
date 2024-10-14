using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Unity.Services.Core;
using Unity.Services.Authentication;
#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif

public class MainMenuManager : MonoBehaviour
{
    private bool initialized = false;
    private static MainMenuManager singleton = null;
    private FirebaseAuth auth;
    private FirebaseUser user;

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
        try
        {
            var firebaseService = FirebaseService.Singleton;
            auth = firebaseService.Auth;
            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                PanelManager.CloseAll();
                PanelManager.GetSingleton("main").Open();
            }
        }
        catch (Exception exception)
        {
            Debug.LogError($"Failed to start client service: {exception.Message}");
            PanelManager.LoadSceneAsync("Login");
        }
    }

    public void SignOut()
    {
        PanelManager.Singleton.StartLoading(2f, 
        () => {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            if (auth.CurrentUser != null)
            {
                auth.SignOut();
                Debug.Log("User signed out from Firebase.");
            }
            else
            {
                Debug.Log("No user is signed in to Firebase.");
            }

            if (AuthenticationService.Instance.IsSignedIn)
            {
                AuthenticationService.Instance.SignOut();
                Debug.Log("User signed out from Unity Services.");
            }
            else
            {
                Debug.Log("No user is signed in to Unity Services.");
            }
        }, 
        () => PanelManager.LoadSceneAsync("Login"));
    }

    public async void LoadGame()
    {
        try
        {
            PlayerData playerData = await CloudSaveManager.Singleton.LoadPlayerData();
            if (playerData == null)
            {
                throw new Exception("No player data found.");
            }
            PanelManager.LoadSceneAsync(playerData.GetLevel()); 
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading player data: " + e.Message);
            ShowPopUp(PopUpMenu.Action.None, "No save data found. Please start a new game.", "Ok");
        }
    }
    
    public async void NewGame()
    {
        string playerID = AuthenticationService.Instance.PlayerInfo.Id;
        Debug.Log("Starting new game for player: " + playerID);
        try
        {
            Vector3 startingPosition = new Vector3(0, 0, 0);
            PlayerData playerData = new PlayerData("Chapter1", startingPosition);
            playerData.SetPlayerID(playerID);
            playerData.SetActiveQuest("Make your way to the village. Find out what is happening.");

            await CloudSaveManager.Singleton.SaveNewPlayerData(playerData);
            PanelManager.CloseAll();
            PanelManager.GetSingleton("cutscene").Open();
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving player data: " + e.Message);
        }
    }

    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }
}
