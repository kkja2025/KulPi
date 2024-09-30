using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteractable : MonoBehaviour
{
    [SerializeField] private string characterName;
    [SerializeField] private List<string> lakanDialogueLines;
    [SerializeField] private List<string> characterDialogueLines;
    [SerializeField] private bool doesCharacterStartFirst = false;

    private int lakanDialogueIndex = 0;
    private int characterDialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isLakanTurn = true;
    private bool conversationComplete = false;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightedSprite;
    private PlayerInput controls;

    [SerializeField] private GameObject dialogueIcon;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new PlayerInput();

        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(true);
        }
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Land.Interact.performed += OnInteract;
        isLakanTurn = !doesCharacterStartFirst;
    }

    void OnDisable()
    {
        controls.Disable();
        controls.Land.Interact.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange && !conversationComplete)
        {
            PanelManager.GetSingleton("dialogue").Open();
            Interact();
        }
    }

    public void Interact()
    {
        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(false);
        }

        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            conversationComplete = true;

            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            HighlightObject(false);

            if (dialogueIcon != null && !conversationComplete)
            {
                dialogueIcon.SetActive(false);
            }
        }
    }

    private void HighlightObject(bool highlight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = highlight && highlightedSprite != null ? highlightedSprite : normalSprite;
        }
    }
}
