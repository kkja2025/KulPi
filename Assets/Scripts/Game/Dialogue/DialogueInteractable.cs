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

    public GameObject dialogueIcon;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new PlayerInput();
        playerMovement = FindObjectOfType<PlayerMovement>();

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
            Interact();
        }
    }

    public void Interact()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        if (dialogueIcon != null)
        {
            dialogueIcon.SetActive(false);
        }

        if (lakanDialogueIndex >= lakanDialogueLines.Count && characterDialogueIndex >= characterDialogueLines.Count)
        {
            conversationComplete = true;
            DialogueUI.Instance.HideDialogue();

            if (playerMovement != null)
            {
                playerMovement.enabled = true;
            }

            if (dialogueIcon != null)
            {
                dialogueIcon.SetActive(true);
            }

            return;
        }

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
