using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private bool initialized = false;
    private static InventoryManager singleton = null;
    private List<InventoryItem> inventory = new List<InventoryItem>();
    
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

    private async void StartClientService()
    {
        await LoadInventoryAsync();
    }

    public List<InventoryItem> GetInventory()
    {
        return inventory;
    }

    public void AddItem(string itemID, string itemName)
    {
        InventoryItem item = new InventoryItem(itemID, itemName);
        inventory.Add(item);
    }

    public bool RemoveItem(string itemName)
    {
        int itemsRemoved = inventory.RemoveAll(item => item.itemName == itemName);
        
        if (itemsRemoved > 0)
        {
            return true;  
        }
        else
        {
            Debug.LogWarning(itemName + " not found in inventory.");
            return false;
        }
    }

    public async Task SaveInventoryAsync()
    {
        try
        {
            await CloudSaveManager.Singleton.SaveInventoryData(inventory);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save inventory: " + e.Message);
        }
    }

    private async Task LoadInventoryAsync()
    {
        var result = await CloudSaveManager.Singleton.LoadInventoryData();
        if (result != null)
        {
            inventory = result;
        }     
    }
}
