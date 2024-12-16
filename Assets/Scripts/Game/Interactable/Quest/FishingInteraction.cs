using System;
using UnityEngine;

public class FishingInteraction : ItemInteractable
{
    [SerializeField] protected string spriteID;
    [SerializeField] private string updatedObjective;
    [SerializeField] private string giveNewObjective;
    [SerializeField] private int totalItems;
    [SerializeField] private GameObject nextFishingSpot;
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
    }

    public void OnQuestItemCompletion()
    {
        if (spriteID != null)
        { 
            GameManager.Singleton.IncrementCount();
            GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
        }  
        OnObjectRemoved(gameObject);
    }

    private async void OnObjectRemoved(GameObject gameObject)
    {
        RemovedObjectsManager.Singleton.RemoveObject(gameObject);
        if (nextFishingSpot != null)
        {
            nextFishingSpot.SetActive(true);
        }
        if (updatedObjective != "")
        {
            GameManager.Singleton.SetTemporaryObjective(updatedObjective + " " + GameManager.Singleton.GetCount() / 2 + "/" + totalItems);
        }
        
        if (GameManager.Singleton.GetCount() / 2 >= totalItems)
        {
            if(giveNewObjective != "") 
            {
                GameManager.Singleton.SetObjective(giveNewObjective);
                GameManager.Singleton.SetCount(0);
                await RemovedObjectsManager.Singleton.SaveRemovedObjectsAsync();
                await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync();
            }
        }
    }
}
