using System;
using UnityEngine;

public class QuestNPCInteraction : DialogueInteractable
{
    private bool hasTalked = false;
    private Chapter1GameManager gameManager;

    protected override void Interact()
    {
        gameManager = GameManager.Singleton as Chapter1GameManager;

        if (!hasTalked)
        {
            hasTalked = true;
            gameManager.TalkedToNPC();
        }
        base.Interact();
    }
}
