using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.SceneManagement;

public class EncyclopediaManager : MonoBehaviour
{
    private bool initialized = false;
    private static EncyclopediaManager singleton = null;
    private List<EncyclopediaItem> encyclopediaList = new List<EncyclopediaItem>();
    public static EncyclopediaManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<EncyclopediaManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("EncyclopediaManager not found in the scene!");
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
    }

    public List<EncyclopediaItem> GetEncyclopediaList()
    {
        return encyclopediaList;
    }

    public void SetEncyclopediaList(List<EncyclopediaItem> list)
    {
        encyclopediaList = list;
    }

    public async Task SaveEncyclopediaEntryAsync(string key)
    {
        string jsonEncyclopedia = JsonUtility.ToJson(new EncyclopediaItemList { items = encyclopediaList });
        var data = new Dictionary<string, object> { { key, jsonEncyclopedia } };

        switch (key)
        {
            case EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY:
                data[EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY] = jsonEncyclopedia;
                break;
            case EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY:
                data[EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY] = jsonEncyclopedia;
                break;
            case EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY:
                data[EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY] = jsonEncyclopedia;
                break;
            case EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY:
                data[EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY] = jsonEncyclopedia;
                break;
            default:
                Debug.LogWarning("Invalid encyclopedia key: " + key);
                return;
        }

        await CloudSaveManager.Singleton.SaveEncyclopediaEntryData(data);
    }

    public async Task<List<EncyclopediaItem>> LoadEncyclopediaEntriesAsync(string key)
    {
        var result = await CloudSaveManager.Singleton.LoadEncyclopediaEntriesData(key);
        if (result != null)
        {
            return result;
        }
        else
        {
            Debug.LogWarning("Encyclopedia data not found.");
            return new List<EncyclopediaItem>();
        }
    }

    public void AddItemToEncyclopedia(EncyclopediaItem item)
    {
        encyclopediaList.Add(item);
    }
}