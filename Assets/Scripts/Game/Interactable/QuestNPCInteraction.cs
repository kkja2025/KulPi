using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestNPCInteraction : DialogueInteractable
{
    [SerializeField] private string giveNewObjective;    
    [SerializeField] private string completeObjective;
    [SerializeField] private int totalNPCs;
    [SerializeField] private List<string> questReturnDialogue;
    [SerializeField] private List<string> npcQuestReturnDialogue;
    [SerializeField] private bool returnDoesCharacterStartFirst;
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
        string currentObjective = gameManager.GetObjective();
        if (currentObjective == completeObjective && !hasCompleted)
        {
            CompleteQuest();
        }
        else if (!hasTalked && currentObjective != giveNewObjective)
        {
            hasTalked = true;
            gameManager.IncrementCount();
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
            if(giveNewObjective != "") 
            {
                gameManager.SetObjective(giveNewObjective);
                gameManager.SetCount(0);
                Debug.Log("Quest started: " + giveNewObjective);
            }
        }
    }

    private void CompleteQuest()
    {
        if(questReturnDialogue.Count > 0)
        {
            SetDialogue(questReturnDialogue, npcQuestReturnDialogue);
            hasCompleted = true;
        }
    }

    private void SetDialogue(List<string> dialogueLines, List<string> npcDialogueLines)
    {
        Debug.Log("Setting dialogue. Character starts first: " + returnDoesCharacterStartFirst);
        lakanDialogueLines = dialogueLines;
        characterDialogueLines = npcDialogueLines;
        hasTalked = false;
        doesCharacterStartFirst = returnDoesCharacterStartFirst;
        isLakanTurn = !returnDoesCharacterStartFirst;
        Debug.Log("doesCharacterStartFirst assigned: " + isLakanTurn);
    }
}
