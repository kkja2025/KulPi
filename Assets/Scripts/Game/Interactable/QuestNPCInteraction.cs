using System;
using UnityEngine;

public class QuestNPCInteraction : DialogueInteractable
{
    [SerializeField] private string giveNewObjective;
    [SerializeField] private int totalNPCs;
    private bool hasTalked = false;
    private Chapter1GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Singleton as Chapter1GameManager;
    }

    protected override void Interact()
    {
        base.Interact();
        if (!hasTalked)
        {
            hasTalked = true;
            gameManager.IncrementCount();
            Debug.Log("Has talked Count is " + gameManager.GetCount());
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (gameManager.GetCount() >= totalNPCs)
        {
            Debug.Log("Count is " + gameManager.GetCount());
            if(giveNewObjective != "") 
            {
                gameManager.SetObjective(giveNewObjective);
            }
        }
    }
}
