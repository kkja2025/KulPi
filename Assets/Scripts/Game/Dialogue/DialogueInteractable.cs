using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueInteractable : MonoBehaviour
{
    public string characterName;
    public List<string> dialogueLines;
    public string playerResponse;

    private int currentDialogueIndex = 0;
    private bool isPlayerInRange = false;
    private SpriteRenderer spriteRenderer;
    public Sprite normalSprite;
    public Sprite highlightedSprite;
    private PlayerInput controls;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new PlayerInput();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Land.Interact.performed += OnInteract;
    }

    void OnDisable()
    {
        controls.Disable();
        controls.Land.Interact.performed -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            Interact();
        }
    }

    public void Interact()
    {
        Debug.Log("Interacting with " + characterName);
        if (currentDialogueIndex < dialogueLines.Count)
        {
            DialogueUI.Instance.ShowDialogue(characterName, dialogueLines[currentDialogueIndex]);
            currentDialogueIndex++;
        }
        else
        {
            DialogueUI.Instance.ShowDialogue("Lakan", playerResponse);
            ResetDialogue();
        }
    }

    private void ResetDialogue()
    {
        currentDialogueIndex = 0; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            Debug.Log("Player in range");
            HighlightObject(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            Debug.Log("Player out of range");
            HighlightObject(false);
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
