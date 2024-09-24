using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public string playerID;

    public PlayerData(int level, string playerID)
    {
        this.level = level;
        this.playerID = playerID;
    }
}

public class CloudSaveManager : MonoBehaviour
{
    private bool initialized = false;
    private static CloudSaveManager singleton = null;
    private const string CLOUD_SAVE_PLAYER_DATA_KEY = "player_data";

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
    }

    public async Task SavePlayerData(int level, string playerID)
    {
        PlayerData playerData = new PlayerData(level, playerID);
        string jsonPlayerData = JsonUtility.ToJson(playerData);
        var data = new Dictionary<string, object> { { CLOUD_SAVE_PLAYER_DATA_KEY, jsonPlayerData } };
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error in SavePlayerData: " + e.Message);
            HandleCloudSaveException(e);
        }
    }

    public async Task SaveNewPlayerData(int level, string playerID)
    {
        PlayerData playerData = new PlayerData(level, playerID);
        string jsonPlayerData = JsonUtility.ToJson(playerData);
        var data = new Dictionary<string, object> 
        { 
            { CLOUD_SAVE_PLAYER_DATA_KEY, jsonPlayerData },
            { "inventory", "{}" },
            { "encyclopedia_figures", "{}" },
            { "encyclopedia_events", "{}" },
            { "encyclopedia_practices_and_traditions", "{}" },
            { "encyclopedia_mythology_and_folklore", "{}" }   
        };
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error in SavePlayerData: " + e.Message);
            HandleCloudSaveException(e);
        }
    }

    public async Task LoadPlayerData()
    {
        var key = new HashSet<string>
        {
            CLOUD_SAVE_PLAYER_DATA_KEY
        };

        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(key);

            if (loadedData.ContainsKey(CLOUD_SAVE_PLAYER_DATA_KEY))
            {
                Debug.Log("Player data loaded: ");
            }
            else
            {
                Debug.LogWarning("Player data not found.");
                throw new Exception("Player data does not exist.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in LoadPlayerData: " + e.Message);
            throw;
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