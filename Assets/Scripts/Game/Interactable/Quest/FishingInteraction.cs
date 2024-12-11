using System;
using UnityEngine;

public class FishingInteraction : ItemInteractable
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
        }
        if(conversationComplete)
        {
            PanelManager.GetSingleton("hud").Close();
            FishingMenu menu = PanelManager.GetSingleton("fishing") as FishingMenu;
            if (menu != null)
            {
                menu.StartFishing(this);
            }
        }
        else
        {
            GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
            OnObjectRemoved(gameObject);
        }
    }

    public void OnQuestItemCompletion()
    {
        if (spriteID != null)
        {
            GameManager.Singleton.IncrementCount();   
            InventoryManager.Singleton.AddItem(spriteID, gameObject.name);
            GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
        }  
        OnObjectRemoved(gameObject);
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
                await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync();
            }
        }
    }
}
