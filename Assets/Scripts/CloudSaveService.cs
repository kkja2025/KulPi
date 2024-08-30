using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;
using Unity.Services.Core;
using UnityEngine;

public class CloudSave : MonoBehaviour
{
    private const string CLOUD_SAVE_PLAYER_NAME_KEY = "player_name";
    private const string CLOUD_SAVE_LEVEL_KEY = "level";

    private void Start()
    {
        Awake();
    }

    private async void Awake()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            SaveData();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadData();
        }
    }

    async void SaveData()
    // async void SaveData(int level, string playerName)
    {
        var data = new Dictionary<string, object>{
            {CLOUD_SAVE_LEVEL_KEY, 3},
            {CLOUD_SAVE_PLAYER_NAME_KEY, "PLAYER4"}
        };
        try
        {
            Debug.Log("Attempting to save data...");
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log($"Saved data success! {string.Join(',', data)}");
        }
        catch (ServicesInitializationException e)
        {
            // service not initialized
            Debug.LogError(e);
        }
        catch (CloudSaveValidationException e)
        {
            // validation error
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            // rate limited
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }

    }

    async void LoadData()
    {
        var keysToLoad = new HashSet<string> {
            CLOUD_SAVE_LEVEL_KEY,
            CLOUD_SAVE_PLAYER_NAME_KEY
        };
        try
        {
            var loadedData = await CloudSaveService.Instance.Data.Player.LoadAsync(keysToLoad);

            if (loadedData.TryGetValue(CLOUD_SAVE_LEVEL_KEY, out var loadedLevel))
            {
                Debug.Log("Loaded saved level: " + loadedLevel.Value.GetAs<int>());
            }
            else
            {
                Debug.Log("Level not found in cloud save");
            }

            if (loadedData.TryGetValue(CLOUD_SAVE_PLAYER_NAME_KEY, out var loadedPlayerName))
            {
                Debug.Log("Loaded saved player name: " + loadedPlayerName.Value.GetAs<string>());
            }
            else
            {
                Debug.Log("Player not found in cloud save");
            }
        }
        catch (ServicesInitializationException e)
        {
            // service not initialized
            Debug.LogError(e);
        }
        catch (CloudSaveValidationException e)
        {
            // validation error
            Debug.LogError(e);
        }
        catch (CloudSaveRateLimitedException e)
        {
            // rate limited
            Debug.LogError(e);
        }
        catch (CloudSaveException e)
        {
            Debug.LogError(e);
        }
    }
}