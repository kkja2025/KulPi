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
    private bool hasTalked = false;
    private bool hasCompleted = false;
    private Chapter1GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Singleton as Chapter1GameManager;
    }

    protected override void Interact()
    {
        string currentQuest = gameManager.GetObjective();
        if (currentQuest == completeQuest && !hasCompleted)
        {
            hasCompleted = true;
            CompleteQuest();
        }
        else if (!hasTalked && currentQuest != giveNewQuest)
        {
            hasTalked = true;
            gameManager.IncrementCount();
            InteractedNPCManager.Singleton.AddInteractedNPC(gameObject);
            Debug.Log("Incremented count: " + gameManager.GetCount());
        }   
        base.Interact();
    }

    protected override void ConversationCompleted()
    {
        base.ConversationCompleted();
        StartQuest();
    }

    private void StartQuest()
    {
        if (gameManager.GetCount() >= totalNPCs)
        {
            if(giveNewQuest != "") 
            {
                gameManager.SetObjective(giveNewQuest);
                gameManager.SetCount(0);
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
