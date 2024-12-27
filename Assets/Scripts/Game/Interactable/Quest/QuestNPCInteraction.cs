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
    [SerializeField] private bool hasTalked = false;
    private bool hasCompleted = false;

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
        }   
        base.Interact();
    }

    protected override void ConversationCompleted()
    {
        base.ConversationCompleted();
        StartQuest();
    }

    private async void StartQuest()
    {
        string currentQuest = GameManager.Singleton.GetObjective();
        string trimmedGiveNewQuest = giveNewQuest.Split('.')[0].Trim();
        if (GameManager.Singleton.GetCount() >= totalNPCs)
        {
            if(giveNewQuest != "" && !currentQuest.Contains(trimmedGiveNewQuest)) 
            {
                GameManager.Singleton.SetObjective(giveNewQuest);
                GameManager.Singleton.SetCount(0);
                InteractedNPCManager.Singleton.SaveInteractedNPC();
                
                if(encyclopediaEntry != "")
                {
                    GameManager.Singleton.UnlockEncyclopediaItem(encyclopediaEntry, "unlock");
                    await EncyclopediaManager.Singleton.SaveEncyclopediaEntryAsync();
                }
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
