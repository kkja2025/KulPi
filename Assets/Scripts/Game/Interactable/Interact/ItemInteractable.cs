using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInteractable : Interactable
{

    [SerializeField] private List<string> lakanDialogueLines;
    private Button dialogueInteractButton;

    private int lakanDialogueIndex = 0;
    protected bool conversationComplete = false;

    protected override void OnInteractButtonClicked()
    {
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if(playerMovement.isGrounded && !playerMovement.isMoving && !conversationComplete)
        {
            if (isPlayerInRange)
            {
                if (interactButton != null)
                {
                    interactButton.gameObject.SetActive(false);
                }
                PanelManager.GetSingleton("dialogue").Open();
                if(dialogueInteractButton == null)
                {
                    GameObject buttonObject = GameObject.FindWithTag("DialogueInteractButton");
                    dialogueInteractButton = buttonObject.GetComponent<Button>();
                    dialogueInteractButton.onClick.AddListener(OnInteractButtonClicked);
                }
                Interact();
                playerMovement.OnDisable();
                if (conversationComplete)
                {
                    if (interactButton != null)
                    {
                        ShowInteractButton();
                    }
                    playerMovement.OnEnable();
                    conversationComplete = false;
                }
            }
        }
    }

    protected override void Interact()
    {
        base.Interact();
        if (lakanDialogueIndex >= lakanDialogueLines.Count)
        {
            conversationComplete = true;
            lakanDialogueIndex = 0;
            PanelManager.GetSingleton("dialogue").Close();
            return;
        }        

        if (lakanDialogueIndex < lakanDialogueLines.Count)
        {
            DialogueUI dialogueUI = PanelManager.GetSingleton("dialogue") as DialogueUI;
            if (dialogueUI != null)
            {
                dialogueUI.ShowDialogue("Lakan", lakanDialogueLines[lakanDialogueIndex]);
                lakanDialogueIndex++;
            }
        }
    }
}