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
    private EnemyEncounterData activeEnemy = null;
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
            PanelManager.Singleton.StartLoading(3f, 
            async () => {
                if (UnityServices.State != ServicesInitializationState.Initialized)
                {
                    await UnityServices.InitializeAsync();
                }
                await LoadPlayerData();
            },
            () => {
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
        else if (scene.name == "Chapter1")
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

    public async void SetObjective(string objective)
    {
        playerData.SetActiveQuest(objective);
        objectiveText.text = objective;
        await SavePlayerData();   
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

    public async void SetPlayerPosition(Vector3 position)
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

        EncyclopediaUnlock encyclopediaUnlockEntry = PanelManager.GetSingleton(panel) as EncyclopediaUnlock;
        if (encyclopediaUnlockEntry != null)
        {
            encyclopediaUnlockEntry.SetEncyclopediaItem(item);
            encyclopediaUnlockEntry.Open();
        }

        EncyclopediaManager.Singleton.AddItemToEncyclopedia(item);
    }
}