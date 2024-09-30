using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Authentication;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.Rendering.LookDev;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private bool initialized = false;
    [SerializeField] private GameObject playerPrefab;
    private GameObject playerInstance;
    private PlayerData playerData;
    private EnemyEncounterData activeEnemy = null;
    private static GameManager singleton = null;
    private List<string> removedObjects = new List<string>();

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
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
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
            SceneManager.LoadScene("MainMenu");
            Debug.LogError($"Error loading player data: {e.Message}");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name == "Login")
        {
            Destroy(gameObject);

        } else if (scene.name == "Chapter1")
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

    public void SetActiveEnemy(EnemyEncounterData enemy)
    {
        activeEnemy = enemy;
    }

    public EnemyEncounterData GetActiveEnemy()
    {
        return activeEnemy;
    }

    public async Task SavePlayerData()
    {
        await CloudSaveManager.Singleton.SavePlayerData(1, playerData.playerID, playerInstance.transform.position);
    }

    public async Task SavePlayerDataWithOffset(GameObject enemy, Vector3 playerPosition)
    {
        Vector3 enemyPosition = enemy.transform.position;
        Vector3 directionFromEnemy = (playerPosition - enemyPosition).normalized;
        float offsetDistance = 5f;
        playerPosition += new Vector3(directionFromEnemy.x * offsetDistance, 0, 0);
        await CloudSaveManager.Singleton.SavePlayerData(1, playerData.playerID, playerPosition);
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
            Debug.Log($"Loaded new Player Level: {loadedData.level}");
            Debug.Log($"Loaded new Player Name: {loadedData.playerID}");
            Debug.Log($"Loaded new Player Position: {spawnPosition}");
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
        await SavePlayerData();
        SceneManager.LoadScene("MainMenu");
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
                Debug.LogWarning("Invalid encyclopedia id provided.");
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