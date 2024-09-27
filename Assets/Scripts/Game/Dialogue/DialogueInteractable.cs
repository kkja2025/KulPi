using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteractable : MonoBehaviour
{
    public string characterName;
    public List<string> lakanDialogueLines;
    public List<string> characterDialogueLines;

    public bool doesCharacterStartFirst = false;

    private int lakanDialogueIndex = 0;
    private int characterDialogueIndex = 0;
    private bool isPlayerInRange = false;
    private bool isLakanTurn = true;
    private bool conversationComplete = false;
    private SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    private PlayerInput controls;
    private PlayerMovement playerMovement;

    public GameObject dialogueIcon; // Reference to the dialogue icon GameObject

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new PlayerInput();
        playerMovement = FindObjectOfType<PlayerMovement>();

        // Ensure the dialogue icon is visible by default
        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(true); // Set to true by default
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
            Interact();
        }
    }

    public void Interact()
    {
        // Disable player movement while interacting
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Hide the dialogue icon when the interaction starts
        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(false);
        }

        // Check if the conversation is complete
        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            conversationComplete = true;
            DialogueUI.Instance.HideDialogue();

            // Re-enable player movement after conversation
            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            // Show the dialogue icon again after conversation
            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
            }

            return;
        }

        // Show dialogue for Lakan or the character
        if (isLakanTurn)
        {
            if (lakanDialogueIndex < lakanDialogueLines.Count)
            {
                DialogueUI.Instance.ShowDialogue("Lakan", lakanDialogueLines[lakanDialogueIndex]);
                lakanDialogueIndex++;
            }
        }
        else
        {
            if (characterDialogueIndex < characterDialogueLines.Count)
            {
                DialogueUI.Instance.ShowDialogue(characterName, characterDialogueLines[characterDialogueIndex]);
                characterDialogueIndex++;
            }
        }

        // Toggle the turn
        isLakanTurn = !isLakanTurn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range");

            if (!conversationComplete)
            {
                HighlightObject(true);
                // Show the dialogue icon when the player is in range
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
            Debug.Log("Player out of range");
            HighlightObject(false);

            // Hide the dialogue icon when the player leaves the range, but not if conversation is ongoing
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
