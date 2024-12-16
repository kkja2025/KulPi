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
    private EncounterData activeEncounter = null;
    private int count = 0;
    private GameObject playerInstance;
    private PlayerData playerData;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private TMP_Text objectiveText;

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

    private void StartClientService()
    {
        try
        {
            PanelManager.Singleton.StartLoading(10f, 
            async () => {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await UnityServices.InitializeAsync();
                }
                if (Time.timeScale != 1f)
                {
                    Time.timeScale = 1f;
                }
                await RemovedObjectsManager.Singleton.LoadRemovedObjectsAsync();
                await InteractedNPCManager.Singleton.LoadInteractedNPCAsync();
                await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync();
                PanelManager.GetSingleton("figures").Open();
                await LoadPlayerData();
            },
            () => {
                GameObject playerObject = GameObject.FindWithTag("Player");
                if (playerObject == null)
                {
                    SceneManager.LoadScene("MainMenu");
                }
                PanelManager.CloseAll();
                PanelManager.GetSingleton("hud").Open();
                if (objectiveText == null)
                {
                    objectiveText = GameObject.Find("QuestText").GetComponent<TextMeshProUGUI>();
                }
                if (playerData != null)
                {
                    SetObjective(playerData.GetActiveQuest());
                }
            });
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

        }
        else if (scene.name == "Chapter1" || scene.name == "Chapter2" || scene.name == "Chapter3")
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
    
    public void SetActiveEncounter(EncounterData encounter)
    {
        activeEncounter = encounter;
    }

    public EncounterData GetActiveEncounter()
    {
        return activeEncounter;
    }

    public async void SetObjective(string objective)
    {
        playerData.SetActiveQuest(objective);
        objectiveText.text = objective;
        await SavePlayerData();   
    }

    public void SetTemporaryObjective(string objective)
    {
        playerData.SetActiveQuest(objective);
        objectiveText.text = objective;
    }

    public string GetObjective()
    {
        return playerData.GetActiveQuest();
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int newCount)
    {
        count = newCount;
    }

    public void IncrementCount()
    {
        count++;
    }

    public async Task SavePlayerData()
    {
        playerData.SetPosition(playerInstance.transform.position);
        await CloudSaveManager.Singleton.SavePlayerData(playerData);
    }

    public async Task SavePlayerDataPosition(Vector3 newPosition)
    {
        playerData.SetPosition(newPosition);
        await CloudSaveManager.Singleton.SavePlayerData(playerData);
    }

    public async Task SavePlayerDataWithOffset(GameObject encounter, Vector3 playerPosition)
    {
        Vector3 encounterPosition = encounter.transform.position;
        Vector3 directionFromEncounter = (playerPosition - encounterPosition).normalized;
        float offsetDistance = 5f;
        playerPosition += new Vector3(directionFromEncounter.x * offsetDistance, 0, 0);
        playerData.SetPosition(playerPosition);
        await CloudSaveManager.Singleton.SavePlayerData(playerData);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    public async Task SetPlayerDataPosition(Vector3 position)
    {
        playerData.SetPosition(position);
        await SavePlayerData();
    }

    public async Task SetPlayerPosition(Vector3 position)
    {
        if (playerInstance != null)
        {
            playerInstance.transform.position = position;
            await SavePlayerData();
        }
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
            }
            return;
        }

        if (loadedData != null)
        {
            Vector3 spawnPosition = loadedData.GetPosition();
            playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        }

        CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
        }
    }

    public void ReturnToMainMenu()
    {
        PanelManager.LoadSceneAsync("MainMenu");
    }

    public void UnlockEncyclopediaItem(string id, string panel)
    {
        EncyclopediaItem item = EncyclopediaManager.Singleton.GetEncyclopediaItemById(id);
        if (item == null) return;
        EncyclopediaManager.Singleton.AddItemToEncyclopedia(item);

        EncyclopediaUnlock encyclopediaUnlockEntry = PanelManager.GetSingleton(panel) as EncyclopediaUnlock;
        if (encyclopediaUnlockEntry != null)
        {
            encyclopediaUnlockEntry.SetEncyclopediaItem(item);
            encyclopediaUnlockEntry.Open();
        }
    }
}