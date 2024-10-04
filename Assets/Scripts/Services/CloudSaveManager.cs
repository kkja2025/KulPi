using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

public class CloudSaveManager : MonoBehaviour
{
    private bool initialized = false;
    private static CloudSaveManager singleton = null;
    private const string CLOUD_SAVE_PLAYER_DATA_KEY = "player_data";
    private const string CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY = "removed_objects";

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

    public async Task SavePlayerData(PlayerData playerData)
    {
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

    public async Task SaveNewPlayerData(PlayerData playerData)
    {
        string jsonPlayerData = JsonUtility.ToJson(playerData);
        var data = new Dictionary<string, object> 
        { 
            { CLOUD_SAVE_PLAYER_DATA_KEY, jsonPlayerData },
            { CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY, "{}" },
            { InventoryManager.CLOUD_SAVE_INVENTORY_KEY, "{}" },
            { EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY, "{}" },
            { EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY, "{}" },
            { EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY, "{}" },
            { EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY, "{}" }   
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

    public async Task<PlayerData> LoadPlayerData()
    {
        var key = new HashSet<string>
        {
            CLOUD_SAVE_PLAYER_DATA_KEY
        };

        try
        {
            Dictionary<string, Item>  loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(key);

            if (loadedData.TryGetValue(CLOUD_SAVE_PLAYER_DATA_KEY, out var playerDataJson))
            {
                string json = playerDataJson.Value.GetAsString(); 
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                return playerData;
            }
            else
            {
                Debug.LogWarning("Player data not found.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in LoadPlayerData: " + e.Message);
            throw;
        }
    }

    public async Task SaveRemovedObjectsData(List<string> removedObjects)
    {

        string jsonRemovedObjects = JsonUtility.ToJson(new RemovedObjectDataList { List = removedObjects });
        var data = new Dictionary<string, object> { { CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY, jsonRemovedObjects } };
        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Data saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Error in SaveRemovedObjectsData: " + e.Message);
            HandleCloudSaveException(e);
        }
    }

    public async Task<List<string>> LoadRemovedObjectsData()
    {
        try
        {
            var removedObjectsData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY });

            if (removedObjectsData.TryGetValue(CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY, out var removedObjectsJson))
            {
                string json = removedObjectsJson.Value.GetAsString();
                var removedObjects = JsonUtility.FromJson<RemovedObjectDataList>(json).List;
                return removedObjects;
            }
            else
            {
                Debug.LogWarning("Removed objects data not found.");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error in LoadRemovedObjectsData: " + e.Message);
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