using System;
using UnityEngine;

public class QuestItemInteraction : ItemInteractable
{
    [SerializeField] protected string spriteID;
    [SerializeField] private string giveNewObjective;
    [SerializeField] private int totalItems;
    private bool hasInteracted = false;

    protected override void Interact()
    {
        base.Interact();
        if (!hasInteracted)
        {
            hasInteracted = true;
            GameManager.Singleton.IncrementCount();           
            if (spriteID != null)
            {
                InventoryManager.Singleton.AddItem(spriteID, gameObject.name);
                UnlockEntry();
            }
        }
        if(conversationComplete)
        {
            OnObjectRemoved(gameObject);
        }
    }

    private async void OnObjectRemoved(GameObject gameObject)
    {
        RemovedObjectsManager.Singleton.RemoveObject(gameObject);
        
        if (GameManager.Singleton.GetCount() >= totalItems)
        {
            if(giveNewObjective != "") 
            {
                GameManager.Singleton.SetObjective(giveNewObjective);
                GameManager.Singleton.SetCount(0);
                await RemovedObjectsManager.Singleton.SaveRemovedObjectsAsync();
                await InventoryManager.Singleton.SaveInventoryAsync();
                SaveUnlockedEntry();
            }
        }
    }

    private void UnlockEntry()
    {
        GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
        Debug.Log("Unlocked entry: " + spriteID);
    }

    private void SaveUnlockedEntry()
    {
        GameManager.Singleton.SaveEncyclopediaItem(spriteID);
    }
}
