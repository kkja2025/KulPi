using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor.Tilemaps;
#endif

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite highlightedSprite;
    [SerializeField] protected string spriteID;
    protected Button interactButton;
    protected InteractButtonPositioner buttonPositioner;
    protected PlayerInput controls;

    private void Awake()
    {
        GameObject buttonObject = GameObject.FindWithTag("InteractButton");
        if (buttonObject != null)
        {
            interactButton = buttonObject.GetComponent<Button>();
            if (interactButton == null)
            {
                Debug.LogError("InteractButton found but does not have a Button component.");
            }
            else
            {
                buttonPositioner = interactButton.GetComponent<InteractButtonPositioner>();
            }
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'InteractButton' found.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        controls = new PlayerInput();
    }

    protected virtual void OnEnable()
    {
        controls.Enable();
    }

    protected virtual void OnDisable()
    {
        controls.Disable();
    }

    protected virtual void OnInteract(InputAction.CallbackContext context)
    {
        if (isPlayerInRange)
        {
            Interact();
        }
    }

    protected virtual void OnObjectRemoved(GameObject gameObject)
    {
        GameManager.Singleton.RemoveObject(gameObject);
        Debug.Log("Destroying object: " + gameObject.name);
    }

    protected virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
        InventoryManager.Singleton.AddItem(spriteID, gameObject.name);
        OnObjectRemoved(gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true; 
            if (buttonPositioner != null)
            {
                buttonPositioner.SetTargetObject(transform); 
            }

            controls.Land.Interact.performed += OnInteract;
            HighlightObject(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (buttonPositioner != null)
            {
                buttonPositioner.SetTargetObject(null); 
            }

            controls.Land.Interact.performed -= OnInteract;
            HighlightObject(false);
        }
    }

    protected void HighlightObject(bool highlight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = highlight && highlightedSprite != null ? highlightedSprite : normalSprite;
        }
    }
}
