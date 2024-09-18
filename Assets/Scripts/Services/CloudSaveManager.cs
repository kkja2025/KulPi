using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            singleton = this;
            DontDestroyOnLoad(gameObject);
        }
        StartClientService();
    }

    private async void StartClientService()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
        var firebaseService = FirebaseService.Singleton;
        auth = firebaseService.Auth;
    }

    public async Task SavePlayerData(int level, string playerID)
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
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            }
        }
        catch (Exception e)
        {
            HandleCloudSaveException(e);
        }
    }

    public async Task LoadPlayerData()
    {
        var keysToLoad = new HashSet<string>
        {
            CLOUD_SAVE_LEVEL_KEY,
            CLOUD_SAVE_PLAYER_ID_KEY
        };

        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);
            Debug.Log("Data loaded successfully: " + string.Join(", ", loadedData.Keys));

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
            Debug.LogError("Error in LoadPlayerData: " + e.Message);
            throw;
        }
    }

    public async Task UpdatePlayerData(int newLevel)
    {
        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_SAVE_LEVEL_KEY });

            if (loadedData.TryGetValue(CLOUD_SAVE_LEVEL_KEY, out var currentLevel))
            {
                var data = new Dictionary<string, object>
                {
                    { CLOUD_SAVE_LEVEL_KEY, newLevel }
                };
                await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            }
            else
            {
                SavePlayerData(newLevel, auth.CurrentUser.UserId);
            }
        }
        catch (Exception e)
        {
            HandleCloudSaveException(e);
        }
    }

    public async Task DeletePlayerData()
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

    private void HandleCloudSaveException(Exception e)
    {
        switch (e)
        {
            case ServicesInitializationException initEx:
                Debug.LogError("Service not initialized: " + initEx.Message);
                break;
            case CloudSaveValidationException validationEx:
                Debug.LogError("Validation error: " + validationEx.Message);
                break;
            case CloudSaveRateLimitedException rateLimitEx:
                Debug.LogError("Rate limited: " + rateLimitEx.Message);
                break;
            default:
                Debug.LogError("CloudSave error: " + e.Message);
                break;
        }
    }
}