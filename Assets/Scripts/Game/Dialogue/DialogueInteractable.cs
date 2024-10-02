using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteractable : Interactable
{
    [SerializeField] private string characterName;
    [SerializeField] private List<string> lakanDialogueLines;
    [SerializeField] private List<string> characterDialogueLines;
    [SerializeField] private bool doesCharacterStartFirst = false;

    private int lakanDialogueIndex = 0;
    private int characterDialogueIndex = 0;
    private bool isLakanTurn = true;
    private bool conversationComplete = false;
    [SerializeField] private GameObject dialogueIcon;

    private void Start()
    {
        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(true);
        }
    }

    protected override void OnEnable()
    {
        controls.Enable();
        controls.Land.Interact.performed += OnInteract;
        isLakanTurn = !doesCharacterStartFirst;
    }

    protected override void OnDisable()
    {
        controls.Disable();
        controls.Land.Interact.performed -= OnInteract;
    }

    protected override void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange && !conversationComplete)
        {
            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(false);
                interactButton.gameObject.SetActive(false);
            }
            PanelManager.GetSingleton("dialogue").Open();
            Interact();
        }
    }

    protected override void Interact()
    {
        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            conversationComplete = true;

            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
                interactButton.gameObject.SetActive(true);
            }
            PanelManager.GetSingleton("dialogue").Close();
            lakanDialogueIndex = 0;
            characterDialogueIndex = 0;
            conversationComplete = false;
            return;
        }

        if (isLakanTurn)
        {
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
        else
        {
            if (characterDialogueIndex < characterDialogueLines.Count)
            {
                DialogueUI dialogueUI = PanelManager.GetSingleton("dialogue") as DialogueUI;
                if (dialogueUI != null)
                {
                    dialogueUI.ShowDialogue(characterName, characterDialogueLines[characterDialogueIndex]);
                    dialogueUI.Open();
                    characterDialogueIndex++;
                }
            }
        }

        isLakanTurn = !isLakanTurn;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            interactButton.gameObject.SetActive(true);

            if (!conversationComplete)
            {
                HighlightObject(true);
                if (dialogueIcon != null)
                {
                    dialogueIcon.SetActive(true);
                }
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactButton.gameObject.SetActive(false);
            HighlightObject(false);

            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
            }
        }
    }
}
