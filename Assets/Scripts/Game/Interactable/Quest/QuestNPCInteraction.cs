using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCInteraction : DialogueInteractable
{
    [SerializeField] private string giveNewQuest;    
    [SerializeField] private int totalNPCs;
    [SerializeField] private List<string> questReturnDialogue;
    [SerializeField] private List<string> npcQuestReturnDialogue;
    [SerializeField] private bool returnDoesCharacterStartFirst;
    [SerializeField] private string completeQuest;
    [SerializeField] private string giveFollowUpQuest;   
    [SerializeField] private string encyclopediaEntry;
    private bool hasTalked = false;
    private bool hasCompleted = false;
    private bool hasUnlocked = false;

    protected override void Interact()
    {
        string currentQuest = GameManager.Singleton.GetObjective();
        if (currentQuest == completeQuest && !hasCompleted)
        {
            hasCompleted = true;
            CompleteQuest();
        }
        else if (!hasTalked && currentQuest != giveNewQuest)
        {
            hasTalked = true;
            GameManager.Singleton.IncrementCount();
            InteractedNPCManager.Singleton.AddInteractedNPC(gameObject);
            Debug.Log("Incremented count: " + GameManager.Singleton.GetCount());
        }   
        base.Interact();
    }

    protected override void ConversationCompleted()
    {
        base.ConversationCompleted();
        if(encyclopediaEntry != "" && !hasUnlocked)
        {
            GameManager.Singleton.UnlockEncyclopediaItem(encyclopediaEntry, "unlock");
            hasUnlocked = true;
        }
        StartQuest();
    }

    private void StartQuest()
    {
        if (GameManager.Singleton.GetCount() >= totalNPCs)
        {
            if(giveNewQuest != "") 
            {
                GameManager.Singleton.SetObjective(giveNewQuest);
                GameManager.Singleton.SetCount(0);
                InteractedNPCManager.Singleton.SaveInteractedNPC();
                Debug.Log("Quest started: " + giveNewQuest);
            }
        }
    }

    private void SetNewQuest()
    {
        if(giveFollowUpQuest != "") 
        {
            giveNewQuest = giveFollowUpQuest;
        }
    }

    private void CompleteQuest()
    {
        if(questReturnDialogue.Count > 0)
        {
            SetDialogue(questReturnDialogue, npcQuestReturnDialogue);
            SetNewQuest();
        }
    }

    private void SetDialogue(List<string> dialogueLines, List<string> npcDialogueLines)
    {
        lakanDialogueLines = dialogueLines;
        characterDialogueLines = npcDialogueLines;
        hasTalked = false;
        doesCharacterStartFirst = returnDoesCharacterStartFirst;
        isLakanTurn = !returnDoesCharacterStartFirst;
    }

    public void SetHasTalked(bool value)
    {
        hasTalked = value;
    }
}
