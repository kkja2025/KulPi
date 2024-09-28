using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.CloudSave;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    private bool initialized = false;
    private static InventoryManager singleton = null;
    public List<InventoryItem> inventory = null;
    public const string CLOUD_SAVE_INVENTORY_KEY = "inventory";
    
    public static InventoryManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<InventoryManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("InventoryManager not found in the scene!");
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
        LoadInventoryAsync();
    }

    public void AddItem(string itemID, string itemName)
    {
        InventoryItem item = new InventoryItem(itemID, itemName);
        inventory.Add(item);
        Debug.Log(itemName + " added to inventory.");
        SaveInventoryAsync();
    }

    public bool RemoveItem(string itemName)
    {
        int itemsRemoved = inventory.RemoveAll(item => item.itemName == itemName);
        
        if (itemsRemoved > 0)
        {
            Debug.Log(itemName + " removed from inventory.");
            SaveInventoryAsync(); 
            return true;  
        }
        else
        {
            Debug.LogWarning(itemName + " not found in inventory.");
            return false;
        }
    }

    private async void SaveInventoryAsync()
    {
        try
        {
            string jsonInventory = JsonUtility.ToJson(new InventoryItemList { items = inventory });
            var data = new Dictionary<string, object> { { CLOUD_SAVE_INVENTORY_KEY, jsonInventory } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
            Debug.Log("Inventory saved successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventory: " + e.Message);
        }
    }

    private async void LoadInventoryAsync()
    {
        try
        {
            var inventoryData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { CLOUD_SAVE_INVENTORY_KEY });

            if (inventoryData.TryGetValue(CLOUD_SAVE_INVENTORY_KEY, out var inventoryJson))
            {
                string json = inventoryJson.Value.GetAsString();
                var loadedInventory = JsonUtility.FromJson<InventoryItemList>(json)?.items;

                if (loadedInventory != null && loadedInventory.Count > 0)
                {
                    inventory = loadedInventory;
                    Debug.Log("Inventory loaded successfully.");
                }
                else
                {
                    Debug.Log("Inventory is empty.");
                    inventory = new List<InventoryItem>();
                }
            }
            else
            {
                Debug.LogWarning("No inventory found in Cloud Save, initializing a new inventory.");
                inventory = new List<InventoryItem>();
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to load inventory: " + e.Message);
            inventory = new List<InventoryItem>();
            GameManager.Singleton.ReturnToMainMenu();
        }
    }
}
