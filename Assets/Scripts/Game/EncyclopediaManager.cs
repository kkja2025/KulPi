using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;

[System.Serializable]
public class EncyclopediaItem
{
    public string itemChapter;
    public string itemTitle;
    public Sprite itemIcon;
    public string itemDescription;
    public string itemCategory;

    public EncyclopediaItem(string chapter, string title, Sprite icon, string description, string category)
    {
        itemChapter = chapter;
        itemTitle = title;
        itemIcon = icon;
        itemDescription = description;
        itemCategory = category;
    }
}

[System.Serializable]
public class EncyclopediaItemList
{
    public List<EncyclopediaItem> items = null;
}

public class EncyclopediaManager : MonoBehaviour
{
    private bool initialized = false;
    private static EncyclopediaManager singleton = null;
    public List<EncyclopediaItem> encyclopediaList = null;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = "encyclopedia_figures";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = "encyclopedia_events";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY = "encyclopedia_practices_and_traditions";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY = "encyclopedia_mythology_and_folklore";
    
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
        StartClientService();
    }

    private void StartClientService()
    {
        
    }

    private async void SaveEncyclopediaEntryAsync(string key)
    {
        try
        {
            string jsonEncyclopedia = JsonUtility.ToJson(new EncyclopediaItemList { items = encyclopediaList });
            var data = new Dictionary<string, object> { { CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY, jsonEncyclopedia } };

            switch (key)
            {
                case CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY:
                    data[CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY] = jsonEncyclopedia;
                    break;
                case CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY:
                    data[CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY] = jsonEncyclopedia;
                    break;
                case CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY:
                    data[CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY] = jsonEncyclopedia;
                    break;
                case CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY:
                    data[CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY] = jsonEncyclopedia;
                    break;
                default:
                    Debug.LogWarning("Invalid encyclopedia key: " + key);
                    return;
            }

            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save encyclopedia entry: " + e.Message);
        }
    }

    private async void LoadEncyclopediaEntriesAsync(string key)
    {
        try
        {
            var keys = new HashSet<string> { key };
            
            var encyclopediaData = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (encyclopediaData.TryGetValue(key, out var encyclopediaEntryJson))
            {
                string json = encyclopediaEntryJson.Value.GetAsString();
                var loadedEncyclopediaList = JsonUtility.FromJson<EncyclopediaItemList>(json)?.items;

                switch (key)
                {
                    case CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY:
                        if (loadedEncyclopediaList != null && loadedEncyclopediaList.Count > 0)
                        {
                            encyclopediaList = loadedEncyclopediaList;
                            Debug.Log("Encyclopedia figures loaded successfully.");
                        }
                        else
                        {
                            Debug.Log("Encyclopedia figures list is empty.");
                            encyclopediaList = new List<EncyclopediaItem>();
                        }
                        break;
                    case CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY:
                        if (loadedEncyclopediaList != null && loadedEncyclopediaList.Count > 0)
                        {
                            encyclopediaList = loadedEncyclopediaList;
                            Debug.Log("Encyclopedia events loaded successfully.");
                        }
                        else
                        {
                            Debug.Log("Encyclopedia events list is empty.");
                            encyclopediaList = new List<EncyclopediaItem>();
                        }
                        break;
                    case CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY:
                        if (loadedEncyclopediaList != null && loadedEncyclopediaList.Count > 0)
                        {
                            encyclopediaList = loadedEncyclopediaList;
                            Debug.Log("Encyclopedia practices and traditions loaded successfully.");
                        }
                        else
                        {
                            Debug.Log("Encyclopedia practices and traditions list is empty.");
                            encyclopediaList = new List<EncyclopediaItem>();
                        }
                        break;
                    case CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY:
                        if (loadedEncyclopediaList != null && loadedEncyclopediaList.Count > 0)
                        {
                            encyclopediaList = loadedEncyclopediaList;
                            Debug.Log("Encyclopedia mythology and folklore loaded successfully.");
                        }
                        else
                        {
                            Debug.Log("Encyclopedia mythology and folklore list is empty.");
                            encyclopediaList = new List<EncyclopediaItem>();
                        }
                        break;
                    default:
                        Debug.LogWarning("Invalid encyclopedia key: " + key);
                        encyclopediaList = new List<EncyclopediaItem>();
                        break;
                }
            }
            else
            {
                Debug.LogWarning("No entries found in Cloud Save for key: " + key);
                encyclopediaList = new List<EncyclopediaItem>();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load encyclopedia entries: " + e.Message);
            encyclopediaList = new List<EncyclopediaItem>();
        }
    }
}