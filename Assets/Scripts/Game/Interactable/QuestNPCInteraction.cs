using System;
using UnityEngine;

public class QuestNPCInteraction : DialogueInteractable
{
    [SerializeField] private string giveNewObjective;
    [SerializeField] private int totalNPCs = 1;
    private bool hasTalked = false;
    private Chapter1GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.Singleton as Chapter1GameManager;
    }

    protected override void Interact()
    {
        base.Interact();
        if (!hasTalked)
        {
            hasTalked = true;
            gameManager.IncrementCount();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        if (gameManager.GetCount() >= totalNPCs)
        {
            gameManager.CompleteObjective();
            if(giveNewObjective != "") 
            {
                gameManager.SetObjective(giveNewObjective);
            }
        }
    }
}
