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
        }
        if(conversationComplete)
        {
            conversationComplete = false;
            PicturePuzzleItem puzzleItem = GetComponent<PicturePuzzleItem>();
            if (puzzleItem != null)
            {
                puzzleItem.ShowPuzzle();
                puzzleItem.OnPuzzleCompleted += () =>
                {
                    if (spriteID != null)
                    {
                        InventoryManager.Singleton.AddItem(spriteID, gameObject.name);
                        GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
                    }  
                    OnObjectRemoved(gameObject);
                };
            } else
            {
                GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
                OnObjectRemoved(gameObject);
            }
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
                await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync();
            }
        }
    }
}
