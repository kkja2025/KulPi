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
    // Reference to the SpriteRenderer component of the interactable
    protected SpriteRenderer spriteRenderer;
    // Sprites for normal and highlighted states (assign in Inspector for each interactable)
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite highlightedSprite;
    protected Button interactButton;
    protected PlayerInput controls;



    private void Awake()

    {
        if (interactButton == null)
        {
            GameObject buttonObject = GameObject.Find("InteractButton");
            if (buttonObject != null)
            {
                interactButton = buttonObject.GetComponent<Button>();
                Debug.Log("Interact Button found and assigned automatically.");
            }
            else
            {
                Debug.LogWarning("Interact Button not found. Please assign the Interact Button in the Inspector.");
            }
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

    // Override this method in child classes for specific interactions

    protected virtual void OnInteract(InputAction.CallbackContext context)

    {

        if (isPlayerInRange)

        {

            Interact();

        }

    }



    protected virtual void Interact()

    {

        Debug.Log("Interacted with " + gameObject.name);

    }



    // Detect when the player enters the range of the interactable item

 protected virtual void OnTriggerEnter2D(Collider2D collision)

    {

        if (collision.CompareTag("Player"))

        {

            isPlayerInRange = true;

            interactButton.gameObject.SetActive(true);

            controls.Land.Interact.performed += OnInteract;

            // Highlight the interactable by switching to the highlighted sprite

            HighlightObject(true);

        }

    }



    // Detect when the player leaves the range of the interactable item

  protected virtual void OnTriggerExit2D(Collider2D collision)

    {

        if (collision.CompareTag("Player"))

        {

            isPlayerInRange = false;

            interactButton.gameObject.SetActive(false);

            controls.Land.Interact.performed -= OnInteract;



            // Remove highlight by reverting to the normal sprite

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

    protected virtual void OnObjectRemoved(GameObject gameObject) {
        GameManager.Singleton.RemoveObject(gameObject);
        Debug.Log("Destroying object: " + gameObject.name);
    }

}
