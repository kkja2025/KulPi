using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.VisualScripting;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.ShaderGraph;
using UnityEditor.Rendering.LookDev;
#endif

public class GameManager : MonoBehaviour
{
    private bool initialized = false;
    private static GameManager singleton = null;
    protected GameObject playerInstance;
    protected PlayerData playerData;
    protected List<string> removedObjects = new List<string>();
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] protected TMP_Text objectiveText;

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("GameManager not found in the scene!");
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
    }
    
    private void Awake()
    {
        if (singleton != null && singleton != this)
        {
            Destroy(gameObject);
            return;
        }
        singleton = this;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        Application.runInBackground = true;
        StartClientService();
    }

    private async void StartClientService()
    {
        PanelManager.LoadSceneAsync("");

        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            await UnityServices.InitializeAsync();
        }
        try
        {
            await LoadRemovedObjects();
            await LoadPlayerData();
            await Task.Delay(2000);
            PanelManager.CloseAll();
            PanelManager.GetSingleton("hud").Open();
        }
        catch (Exception e)
        {
            SceneManager.LoadScene("Login");
            Debug.LogError($"Error loading player data: {e.Message}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Login")
        {
            Destroy(gameObject);

        } else if (scene.name == "Chapter1-Jungle" || scene.name == "Chapter1-Beach" )
        {
            StartClientService();
        }
    }
    private void OnDestroy()
    {
        if (singleton == this)
        {
            singleton = null;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public virtual void SetObjective(string text)
    {
        if (objectiveText != null)
        {
            Debug.Log("Setting objective text to: " + text);
            objectiveText.text = text;
        }
        else
        {
            Debug.LogError("Objective Text is not assigned or initialized!");
        }
    }

    public async Task SavePlayerData()
    {
        playerData.SetPosition(playerInstance.transform.position);
        await CloudSaveManager.Singleton.SavePlayerData(playerData);
    }

    public async Task SavePlayerDataWithOffset(GameObject enemy, Vector3 playerPosition)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3 directionFromEnemy = (playerPosition - enemyPosition).normalized;
        float offsetDistance = 5f;
        playerPosition += new Vector3(directionFromEnemy.x * offsetDistance, 0, 0);

        playerData.SetPosition(playerPosition);
        await CloudSaveManager.Singleton.SavePlayerData(playerData);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public async Task LoadPlayerData()
    {
        PlayerData loadedData = await CloudSaveManager.Singleton.LoadPlayerData();

        if (playerInstance != null)
        {
            if (loadedData != null)
            {
                playerData = loadedData;
                Vector3 spawnPosition = loadedData.GetPosition();
                playerInstance.transform.position = spawnPosition;
                Debug.Log($"Updated Player Position to: {spawnPosition}");
            }
            else
            {
                Debug.LogWarning("No player data found to update position.");
            }

            return;
        }

        if (loadedData != null)
        {
            Vector3 spawnPosition = loadedData.GetPosition();
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            SetObjective(loadedData.GetActiveQuest());
        }
        else
        {
            Debug.LogWarning("No player data found.");
        }

        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
            Debug.Log("Assigned player instance to the virtual camera's follow target.");
        }
    }

    public async void RemoveObject(GameObject obj)
    {
        if (obj != null)
        {
            removedObjects.Add(obj.name);
            Destroy(obj);
            await CloudSaveManager.Singleton.SaveRemovedObjectsData(removedObjects);
        }
    }

    private async Task LoadRemovedObjects()
    {
        var result = await CloudSaveManager.Singleton.LoadRemovedObjectsData();
        if (result != null)
        {
            Debug.Log($"Loaded {result.Count} removed objects.");
            removedObjects = result;
            foreach (var objectName in removedObjects)
            {
                GameObject obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    Destroy(obj);
                    Debug.Log($"Removed {objectName} from the scene.");
                }
            }
        }
    }

    public async void ReturnToMainMenu()
    {
        PanelManager.LoadSceneAsync("MainMenu");
        await SavePlayerData();
    }

    public async void UnlockEncyclopediaItem(string id, string panel)
    {
        EncyclopediaItem item = null;
        switch (id)
        {
            case "Diwata":
                item = EncyclopediaItem.Figures_Chapter1_Diwata();
                break;
            case "SacredGrove":
                item = EncyclopediaItem.Events_Chapter1_Sacred_Grove();
                break;
            case "CursedLandOfSugbu":
                item = EncyclopediaItem.Events_Chapter1_Cursed_Land_of_Sugbu();
                break;
            case "TraditionalFilipinoMedicine":
                item = EncyclopediaItem.PracticesAndTraditions_Chapter1_Traditional_Filipino_Medicine();
                break;
            case "PowersAndFilipinoSpirituality":
                item = EncyclopediaItem.PracticesAndTraditions_Chapter1_Powers_and_Filipino_Spirituality();
                break;
            case "Tikbalang":
                item = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Tikbalang();
                break;
            case "Sigbin":
                item = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Sigbin();
                break;
            default:
                Debug.LogWarning("No encyclopedia entry provided.");
                return;
        }
        EncyclopediaUnlock encyclopediaUnlockEntry = PanelManager.GetSingleton(panel) as EncyclopediaUnlock;
        if (encyclopediaUnlockEntry != null)
        {
            encyclopediaUnlockEntry.SetEncyclopediaItem(item);
            encyclopediaUnlockEntry.Open();
        }
        await EncyclopediaManager.Singleton.AddItem(item);
    }
}