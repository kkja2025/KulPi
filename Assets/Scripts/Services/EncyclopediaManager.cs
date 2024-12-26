using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

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

    public async Task SaveEncyclopediaEntryAsync()
    {
        await CloudSaveManager.Singleton.SaveEncyclopediaEntryData(encyclopediaList);
        await LoadEncyclopediaEntriesAsync();
    }

    public async Task<List<EncyclopediaItem>> LoadEncyclopediaEntriesAsync()
    {
        var result = await CloudSaveManager.Singleton.LoadEncyclopediaEntriesData();
        if (result != null)
        {
            return result;
        }
        else
        {
            return new List<EncyclopediaItem>();
        }
    }

    public void AddItemToEncyclopedia(EncyclopediaItem item)
    {
        encyclopediaList.Add(item);
    }

    public EncyclopediaItem GetEncyclopediaItemById(string id)
    {
        switch (id)
        {
            case "Sample":
                return EncyclopediaItem.Sample();
            case "Babaylan":
                return EncyclopediaItem.Figures_Chapter1_Babaylan();
            case "Albularyo":
                return EncyclopediaItem.Figures_Chapter1_Albularyo();
            case "Lagundi":
                return EncyclopediaItem.Events_Chapter1_Lagundi();
            case "Sambong":
                return EncyclopediaItem.Events_Chapter1_Sambong();
            case "NiyogNiyogan":
                return EncyclopediaItem.Events_Chapter1_NiyogNiyogan();
            case "Tikbalang":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Tikbalang();
            case "Sigbin":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Sigbin();
            case "Diwata":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Diwata();
            case "Datu":
                return EncyclopediaItem.Figures_Chapter2_Datu();
            case "RhythmsOfUnity":
                return EncyclopediaItem.PracticesAndTraditions_Chapter2_RhythmsOfUnity();
            case "Boat":
                return EncyclopediaItem.PracticesAndTraditions_Chapter2_Boat();
            case "Bangus":
                return EncyclopediaItem.Events_Chapter2_Bangus();
            case "Bisugo":
                return EncyclopediaItem.Events_Chapter2_Bisugo();
            case "Apahap":
                return EncyclopediaItem.Events_Chapter2_Apahap();
            case "LapuLapu":
                return EncyclopediaItem.Figures_Chapter2_LapuLapu();
            default:
                Debug.LogWarning("No encyclopedia entry found for the provided ID.");
                return null;
        }
    }
}