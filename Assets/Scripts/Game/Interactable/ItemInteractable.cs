using System;
using UnityEngine;

public class ItemInteractable : Interactable
{
    [SerializeField] protected string spriteID;

    protected override void Interact()
    {
        base.Interact();
        if (spriteID != null)
        {
            InventoryManager.Singleton.AddItem(spriteID, gameObject.name);
            OnObjectRemoved(gameObject);
            UnlockEntry();
        }
    }

    private void OnObjectRemoved(GameObject gameObject)
    {
        GameManager.Singleton.RemoveObject(gameObject);
        Debug.Log("Destroying object: " + gameObject.name);
    }

    private void UnlockEntry()
    {
        GameManager.Singleton.UnlockEncyclopediaItem(spriteID, "unlock");
    }
}