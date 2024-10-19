using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class InteractedNPCManager : MonoBehaviour
{
    private bool initialized = false;
    private static InteractedNPCManager singleton = null;
    protected List<string> interactedNPC = new List<string>();
    private const string CLOUD_SAVE_INTERACTED_NPC_DATA_KEY = "interacted_npc";
    
    public static InteractedNPCManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<InteractedNPCManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("InteractedNPCManager not found in the scene!");
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
        await LoadInteractedNPCAsync();
    }
    
    public async Task LoadInteractedNPCAsync()
    {
        var result = await CloudSaveManager.Singleton.LoadInteractedNPCData();
        if (result != null)
        {
            interactedNPC = result;
            foreach (var npcName in interactedNPC)
            {
                GameObject obj = GameObject.Find(npcName);
                if (obj != null)
                {
                    QuestNPCInteraction npcInteraction = obj.GetComponent<QuestNPCInteraction>();
                     if (npcInteraction != null)
                    {
                        npcInteraction.SetHasTalked(true);
                    }
                }
            }
        }    
    }

    public void AddInteractedNPC(GameObject npc)
    {
        if (npc != null)
        {
            interactedNPC.Add(npc.name);
        }
    }

    public async void SaveInteractedNPC()
    {
        await CloudSaveManager.Singleton.SaveInteractedNPCData(interactedNPC);
    }
}
