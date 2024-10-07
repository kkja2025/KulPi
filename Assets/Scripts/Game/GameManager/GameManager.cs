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
            await LoadPlayerData();
            if (playerData != null)
            {
                SetObjective(playerData.GetActiveQuest());
            }
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

        }
        else if (scene.name == "Chapter1-Jungle" || scene.name == "Chapter1-Beach")
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
            objectiveText.text = text;
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

    public async void SaveEncyclopediaItem(string id)
    {
        EncyclopediaItem item = GetEncyclopediaItemById(id);
        if (item == null) return;

        await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync(item.itemCategory);
    }

    public void UnlockEncyclopediaItem(string id, string panel)
    {
        EncyclopediaItem item = GetEncyclopediaItemById(id);
        if (item == null) return;

        EncyclopediaUnlock encyclopediaUnlockEntry = PanelManager.GetSingleton(panel) as EncyclopediaUnlock;
        if (encyclopediaUnlockEntry != null)
        {
            encyclopediaUnlockEntry.SetEncyclopediaItem(item);
            encyclopediaUnlockEntry.Open();
        }

        EncyclopediaManager.Singleton.AddItemToEncyclopedia(item);
    }

    protected virtual EncyclopediaItem GetEncyclopediaItemById(string id)
    {
        return null;
    }
}