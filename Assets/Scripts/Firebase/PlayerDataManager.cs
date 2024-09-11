using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;

[Serializable]
public class PlayerData
{
    public int level;
    public int score;
    public List<string> inventory;

    public PlayerData(int level, int score, List<string> inventory)
    {
        this.level = level;
        this.score = score;
        this.inventory = inventory;
    }
}

public class PlayerDataManager : MonoBehaviour
{
    
    private bool initialized = false;
    private static PlayerDataManager singleton = null;
    private FirebaseAuth auth;
    private DatabaseReference databaseReference;

    public static PlayerDataManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<PlayerDataManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("PlayerDataManager not found in the scene!");
                }
            }
            return singleton;
        }
    }
    private void Initialize()
    {
        if (initialized) return;

        initialized = true;
        Debug.Log("PlayerDataManager Initialized");
    }

    private void Awake()
    {
        StartClientService();
    }

    private void StartClientService()
    {
        auth = FirebaseAuth.DefaultInstance;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        if (FirebaseAuth.DefaultInstance.CurrentUser == null)
        {
            Debug.LogError("User is not authenticated.");
            SceneManager.LoadScene("Login");
        }
    }

    // Save player data
    private void SavePlayerData(PlayerData playerData)
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        string json = JsonUtility.ToJson(playerData);

        playerRef.SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player data saved successfully.");
            }
            else
            {
                Debug.LogError("Error saving player data: " + task.Exception);
            }
        });
    }

    // Update player data
    private void UpdatePlayerData(Dictionary<string, object> updatedData)
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        playerRef.UpdateChildrenAsync(updatedData).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player data updated successfully.");
            }
            else
            {
                Debug.LogError("Error updating player data: " + task.Exception);
            }
        });
    }

    // Load player data
    private void LoadPlayerData(Action<PlayerData> onLoaded)
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        playerRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
                    Debug.Log("Player data loaded successfully.");
                    onLoaded?.Invoke(playerData);
                }
                else
                {
                    Debug.Log("No player data found.");
                }
            }
            else
            {
                Debug.LogError("Error loading player data: " + task.Exception);
            }
        });
    }

    // Delete player data
    private void DeletePlayerData()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        playerRef.RemoveValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Player data deleted successfully.");
            }
            else
            {
                Debug.LogError("Error deleting player data: " + task.Exception);
            }
        });
    }

    // Example method to save new game data
    public void SaveNewGame()
    {
        PlayerData newPlayerData = new PlayerData(1, 0, new List<string> { "Starter Sword", "Starter Shield" });
        SavePlayerData(newPlayerData);
    }

    // Example method to check for saved game data
    public void CheckForSaveData(Action<bool> callback)
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            callback(false);
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        playerRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                bool dataExists = snapshot.Exists; // Check if data exists
                callback?.Invoke(dataExists);
            }
            else
            {
                Debug.LogError("Error checking save data: " + task.Exception);
                callback?.Invoke(false);
            }
        });
    }

    // Method to load game data and start the game
    public void LoadGameData()
    {
        LoadPlayerData((playerData) =>
        {
            if (playerData != null)
            {
                Debug.Log($"Loaded Player Data - Level: {playerData.level}, Score: {playerData.score}");
                SceneManager.LoadScene("GameScene");
            }
        });
    }

    // Example method to update score
    public void UpdateScore(int newScore)
    {
        Dictionary<string, object> updates = new Dictionary<string, object>
        {
            {"score", newScore}
        };
        UpdatePlayerData(updates);
    }

    // Example method to start a new game
    public void NewGame()
    {
        DeletePlayerData();
        SaveNewGame();
        Debug.Log("New game started, previous progress deleted.");
    }
}
