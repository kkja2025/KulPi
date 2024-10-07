using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class RemovedObjectsManager : MonoBehaviour
{
    private bool initialized = false;
    private static RemovedObjectsManager singleton = null;
    private List<string> removedObjects = new List<string>();
    private const string CLOUD_SAVE_REMOVED_OBJECTS_DATA_KEY = "removed_objects";
    
    public static RemovedObjectsManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<RemovedObjectsManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("RemovedObjectsManager not found in the scene!");
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
        await LoadRemovedObjectsAsync();
    }
    
    private async Task LoadRemovedObjectsAsync()
    {
        var result = await CloudSaveManager.Singleton.LoadRemovedObjectsData();
        if (result != null)
        {
            removedObjects = result;
            foreach (var objectName in removedObjects)
            {
                GameObject obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }           
    }

    public void RemoveObject(GameObject obj)
    {
        if (obj != null)
        {
            removedObjects.Add(obj.name);
            Destroy(obj);
        }
    }

    public async Task SaveRemovedObjectsAsync()
    {
        await CloudSaveManager.Singleton.SaveRemovedObjectsData(removedObjects);
    }
}
