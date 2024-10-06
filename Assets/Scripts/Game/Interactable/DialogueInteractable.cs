using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif

public class DialogueInteractable : Interactable
{
    [SerializeField] private string characterName;
    [SerializeField] private List<string> lakanDialogueLines;
    [SerializeField] private List<string> characterDialogueLines;
    [SerializeField] private bool doesCharacterStartFirst = false;
    [SerializeField] private GameObject dialogueIcon;
    [SerializeField] protected Sprite dialogueIconSprite;
    [SerializeField] protected Sprite dialogueIconHighlightedSprite;
    private Button dialogueInteractButton;

    private int lakanDialogueIndex = 0;
    private int characterDialogueIndex = 0;
    private bool isLakanTurn = true;
    protected bool conversationComplete = false;

    protected override void Awake()
    {
        isLakanTurn = !doesCharacterStartFirst;
        base.Awake();
        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(true);
        }
    }

    protected override void OnInteractButtonClicked()
    {
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if(playerMovement.isGrounded)
        {
            if (isPlayerInRange && !conversationComplete)
            {
                if (dialogueIcon != null)
                {
                    dialogueIcon.SetActive(false);
                }
                PanelManager.GetSingleton("dialogue").Open();
                if(dialogueInteractButton == null)
                {
                    GameObject buttonObject = GameObject.FindWithTag("DialogueInteractButton");
                    dialogueInteractButton = buttonObject.GetComponent<Button>();
                    dialogueInteractButton.onClick.AddListener(OnInteractButtonClicked);
                }
                Interact();
            }
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
            }
            PanelManager.GetSingleton("dialogue").Close();

            isLakanTurn = !doesCharacterStartFirst;
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

            if (interactButton != null)
            {

                if (buttonPositioner != null)
                {
                    buttonPositioner.SetTargetObject(transform); 
                }
            }

            if (!conversationComplete)
            {
                if (dialogueIcon != null)
                {
                    HighlightObject(true);
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

            if (interactButton != null)
            {
                if (buttonPositioner != null)
                {
                    buttonPositioner.SetTargetObject(null); 
                }
            }

            if (dialogueIcon != null)
            {
                HighlightObject(false);
                dialogueIcon.SetActive(true);
            }
        }
    }

    protected override void HighlightObject(bool highlight)
    {  
        base.HighlightObject(highlight);
        SpriteRenderer dialogueIconSpriteRenderer = dialogueIcon.GetComponent<SpriteRenderer>();
        if (dialogueIconSpriteRenderer != null)
        {
            dialogueIconSpriteRenderer.sprite = highlight && dialogueIconHighlightedSprite != null ? dialogueIconHighlightedSprite : dialogueIconSprite;
        }
    }
}
