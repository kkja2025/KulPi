using System;
using UnityEngine;

public class QuestItemInteraction : ItemInteractable
{
    [SerializeField] protected string spriteID;
    [SerializeField] private string giveNewObjective;
    [SerializeField] private int totalItems;
    private bool hasInteracted = false;
    private Chapter1GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Singleton as Chapter1GameManager;
    }

    protected override void Interact()
    {
        base.Interact();
        if (!hasInteracted)
        {
            hasInteracted = true;
            gameManager.IncrementCount();           
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
        
        if (gameManager.GetCount() >= totalItems)
        {
            if(giveNewObjective != "") 
            {
                gameManager.SetObjective(giveNewObjective);
                gameManager.SetCount(0);
                await RemovedObjectsManager.Singleton.SaveRemovedObjectsAsync();
                await InventoryManager.Singleton.SaveInventoryAsync();
            }
        }
    }

    private void UnlockEntry()
    {
        gameManager.UnlockEncyclopediaItem(spriteID, "unlock");
    }
}
