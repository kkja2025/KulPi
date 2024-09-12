using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

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

[System.Serializable]
public class User
{
    public string name;
    public int age;

    public User(string name, int age)
    {
        this.name = name;
        this.age = age;
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
    public async Task SavePlayerDataAsync(PlayerData playerData)
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        string json = JsonUtility.ToJson(playerData);

        try
        {
            await playerRef.SetRawJsonValueAsync(json);
            Debug.Log("Player data saved successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error saving player data: " + ex.Message);
        }
    }

    // Update player data
    public async Task UpdatePlayerDataAsync(Dictionary<string, object> updatedData)
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        try
        {
            await playerRef.UpdateChildrenAsync(updatedData);
            Debug.Log("Player data updated successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error updating player data: " + ex.Message);
        }
    }

    // Load player data
    public async Task<PlayerData> LoadPlayerDataAsync()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("Firebase Auth is not initialized or user is not signed in.");
            return null;
        }

        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        try
        {
            DataSnapshot snapshot = await playerRef.GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                return JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Debug.Log("No player data found.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading player data: " + ex.Message);
            return null;
        }
    }

    // Delete player data
    public async Task DeletePlayerDataAsync()
    {
        string userId = auth.CurrentUser.UserId;
        DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");

        try
        {
            await playerRef.RemoveValueAsync();
            Debug.Log("Player data deleted successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError("Error deleting player data: " + ex.Message);
        }
    }

    // Check for save data
    public async Task<bool> CheckForSaveDataAsync()
    {
        try
        {
            string userId = auth.CurrentUser.UserId;
            DatabaseReference playerRef = databaseReference.Child("users").Child(userId).Child("progress");
            DataSnapshot snapshot = await playerRef.GetValueAsync();
            return snapshot.Exists;
        }
        catch (Exception ex)
        {
            Debug.LogError("Error checking save data: " + ex.Message);
            return false;
        }
    }


    public async void SaveNewGame()
    {
        PlayerData newPlayerData = new PlayerData(1, 0, new List<string> { "Starter Sword", "Starter Shield" });
        await SavePlayerDataAsync(newPlayerData);
    }

    public async void NewGame()
    {
        await DeletePlayerDataAsync();
        SaveNewGame();
        Debug.Log("New game started, previous progress deleted.");
    }
<<<<<<< HEAD:Assets/Scripts/PlayerDataManager.cs

    public void WriteData()
    {
        string userId = "testUser"; // Hardcoded user ID

        // Create hardcoded user data
        User user = new User("John Doe", 25);

        // Write the data to the database
        string json = JsonUtility.ToJson(user);
        databaseReference.Child("users").Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Data written successfully.");
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Data written: Name = John Doe, Age = 25", "OK");
            }
            else
            {
                Debug.LogError("Failed to write data: " + task.Exception);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Failed to write data.", "OK");
            }
        });
    }

    // Method to read data from the database
    public void ReadData()
    {
        string userId = "testUser"; // Hardcoded user ID

        databaseReference.Child("users").Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    User user = JsonUtility.FromJson<User>(snapshot.GetRawJsonValue());
                    MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Name: " + user.name + "\nAge: " + user.age, "OK");
                }
                else
                {
                    MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "No data found for user: " + userId, "OK");
                }
            }
            else
            {
                Debug.LogError("Failed to read data: " + task.Exception);
                MainMenuManager.Singleton.ShowPopUp(PopUpMenu.Action.None, "Failed to read data.", "OK");
            }
        });
    }

}
=======
}
>>>>>>> 14d45a6dce77c264df08ba1c3c822d985265d767:Assets/Scripts/Firebase/PlayerDataManager.cs
