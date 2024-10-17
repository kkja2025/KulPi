using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor.Rendering.LookDev;
#endif

public class DialogueInteractable : Interactable
{
    [SerializeField] private string characterName;
    [SerializeField] protected List<string> lakanDialogueLines;
    [SerializeField] protected List<string> characterDialogueLines;
    [SerializeField] protected bool doesCharacterStartFirst;
    [SerializeField] private GameObject dialogueIcon;
    [SerializeField] protected Sprite dialogueIconSprite;
    [SerializeField] protected Sprite dialogueIconHighlightedSprite;
    protected Button dialogueInteractButton;

    private int lakanDialogueIndex = 0;
    private int characterDialogueIndex = 0;
    protected bool isLakanTurn = true;
    protected bool isConversationComplete = false;

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
        if(playerMovement.isGrounded && !playerMovement.isMoving)
        {
            if (isConversationComplete)
            {
                ConversationCompleted();
                isConversationComplete = false;
                playerMovement.OnEnable();
            }
            else if (isPlayerInRange && !isConversationComplete)
            {
                if (dialogueIcon != null)
                {
                    dialogueIcon.SetActive(false);
                }
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
            } 
        }
    }

    protected virtual void ConversationCompleted()
    {
        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
            }
            if (interactButton != null)
            {
                ShowInteractButton();
            }
            PanelManager.GetSingleton("dialogue").Close();
            lakanDialogueIndex = 0;
            characterDialogueIndex = 0;
            isLakanTurn = !doesCharacterStartFirst;
            return;
        }
    }

    protected override void Interact()
    {
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
                    characterDialogueIndex++;
                }
            }
        }
        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            isConversationComplete = true;
        }

        isLakanTurn = !isLakanTurn;
        if (isLakanTurn && lakanDialogueIndex >= lakanDialogueLines.Count)
        {
            isLakanTurn = !isLakanTurn;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (interactButton != null)
            {
                interactButton.gameObject.SetActive(true);
                ShowInteractButton();
                Debug.Log("Show interact button");
            }

            if (dialogueIcon != null)
            {
                HighlightObject(true);
                dialogueIcon.SetActive(true);
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
                interactButton.gameObject.SetActive(false);
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
