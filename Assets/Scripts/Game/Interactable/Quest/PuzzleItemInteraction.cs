using System;
using UnityEngine;

public class QuestItemInteraction : ItemInteractable
{
    [SerializeField] protected string spriteID;
    [SerializeField] private string updatedObjective;
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
            PicturePuzzleItem puzzleItem = GetComponent<PicturePuzzleItem>();
            if (puzzleItem != null)
            {
                puzzleItem.ShowPuzzle();
            } 
            else
            {
                GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
                OnObjectRemoved(gameObject);
            }
        }
    }

    public void OnQuestItemCompletion()
    {
        if (spriteID != null)
        {
            GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
        }  
        OnObjectRemoved(gameObject);
    }

    private async void OnObjectRemoved(GameObject gameObject)
    {
        RemovedObjectsManager.Singleton.RemoveObject(gameObject);
        if (updatedObjective != "")
        {
            GameManager.Singleton.SetTemporaryObjective(updatedObjective + " " + GameManager.Singleton.GetCount() + "/" + totalItems);
        }

        if (GameManager.Singleton.GetCount() >= totalItems)
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
