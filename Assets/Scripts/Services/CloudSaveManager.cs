using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;
using Firebase.Auth;

public class CloudSaveManager : MonoBehaviour
{
    private bool initialized = false;
    private static CloudSaveManager singleton = null;
    private FirebaseAuth auth;
    private const string CLOUD_SAVE_PLAYER_ID_KEY = "player_id";
    private const string CLOUD_SAVE_LEVEL_KEY = "level";

    public static CloudSaveManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<CloudSaveManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("CloudSaveManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
        DontDestroyOnLoad(gameObject);
        Debug.Log("CloudSaveManager Initialized");
    }

    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    private async void StartClientService()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            Debug.Log("Unity Services not initialized. Initializing...");
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Services initialized.");
        }
        var firebaseService = FirebaseService.Singleton;
        auth = firebaseService.Auth;
    }

    public async void SavePlayerData(int level, string playerID)
    {
        var data = new Dictionary<string, object>
        {
            { CLOUD_SAVE_LEVEL_KEY, level },
            { CLOUD_SAVE_PLAYER_ID_KEY, playerID }
        };
        try
        {
            if (auth.CurrentUser != null)
            {
                Debug.Log("Attempting to save data..." + auth.CurrentUser.UserId);
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
                Debug.Log($"Data saved successfully: {string.Join(", ", data)}");
            }
        }
        catch (Exception e)
        {
                HandleCloudSaveException(e);
        }
    }

    public async void LoadPlayerData()
    {
        var keysToLoad = new HashSet<string>
        {
            CLOUD_SAVE_LEVEL_KEY,
            CLOUD_SAVE_PLAYER_ID_KEY
        };
        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);
            if (!loadedData.ContainsKey(CLOUD_SAVE_PLAYER_ID_KEY) || string.IsNullOrEmpty(loadedData[CLOUD_SAVE_PLAYER_ID_KEY]?.ToString()))
            {
                throw new InvalidOperationException("Player ID is empty or not found in cloud save.");
            } 
            if (loadedData.TryGetValue(CLOUD_SAVE_PLAYER_ID_KEY, out var loadedPlayerID))
            {
                Debug.Log("Loaded saved player ID: " + loadedPlayerID);
            }
        }
        catch (Exception e)
        {
            HandleCloudSaveException(e);
        }
    }

    public async void UpdatePlayerData(int newLevel)
    {
        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_SAVE_LEVEL_KEY });

            if (loadedData.TryGetValue(CLOUD_SAVE_LEVEL_KEY, out var currentLevel))
            {
                Debug.Log("Updating level from " + currentLevel.Value.GetAsString() + " to " + newLevel);
                var data = new Dictionary<string, object>
                {
                    { CLOUD_SAVE_LEVEL_KEY, newLevel }
                };
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
                Debug.Log("Level updated successfully.");
            }
            else
            {
                Debug.LogWarning("Level not found. Saving new level.");
                SavePlayerData(newLevel, auth.CurrentUser.UserId);
            }
        }
        catch (Exception e)
        {
            HandleCloudSaveException(e);
        }
    }

    public async void DeletePlayerData()
    {
        try
        {
            await CloudSaveService.Instance.Data.Player.DeleteAsync(CLOUD_SAVE_LEVEL_KEY);
            await CloudSaveService.Instance.Data.Player.DeleteAsync(CLOUD_SAVE_PLAYER_ID_KEY);
            Debug.Log("Data deleted successfully.");
        }
        catch (Exception e)
        {
            HandleCloudSaveException(e);
        }
    }

    // Handle Cloud Save exceptions
    private void HandleCloudSaveException(Exception e)
    {
        switch (e)
        {
            case ServicesInitializationException initEx:
                Debug.LogError("Service not initialized: " + initEx.Message);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Error initializing services.", "Ok");
                break;
            case CloudSaveValidationException validationEx:
                Debug.LogError("Validation error: " + validationEx.Message);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Error saving data.", "Ok");
                break;
            case CloudSaveRateLimitedException rateLimitEx:
                Debug.LogError("Rate limited: " + rateLimitEx.Message);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Rate limited. Try again later.", "Ok");
                break;
            default:
                Debug.LogError("CloudSave error: " + e.Message);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Error saving/loading data.", "Ok");
                break;
        }
    }
}
