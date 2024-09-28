using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class Interactable : MonoBehaviour

{

    protected bool isPlayerInRange = false;
    // Reference to the SpriteRenderer component of the interactable
    protected SpriteRenderer spriteRenderer;
    // Sprites for normal and highlighted states (assign in Inspector for each interactable)
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

    }



    void OnDisable()

    {

        controls.Disable();

    }

    // Override this method in child classes for specific interactions

    private void OnInteract(InputAction.CallbackContext context)

    {

        if (isPlayerInRange)

        {

            Interact();

        }

    }



    public virtual void Interact()

    {

        Debug.Log("Interacted with " + gameObject.name);

    }



    // Detect when the player enters the range of the interactable item

 private void OnTriggerEnter2D(Collider2D collision)

    {

        if (collision.CompareTag("Player"))

        {

            isPlayerInRange = true;

            Debug.Log("Player in range");

            controls.Land.Interact.performed += OnInteract;

            // Highlight the interactable by switching to the highlighted sprite

            HighlightObject(true);

        }

    }



    // Detect when the player leaves the range of the interactable item

 private void OnTriggerExit2D(Collider2D collision)

    {

        if (collision.CompareTag("Player"))

        {

            isPlayerInRange = false;

            Debug.Log("Player out of range");

            controls.Land.Interact.performed -= OnInteract;



            // Remove highlight by reverting to the normal sprite

            HighlightObject(false);

        }

    }



    // Method to highlight or remove highlight from the object by changing its sprite

protected void HighlightObject(bool highlight)

    {

        if (spriteRenderer != null)

        {

            if (highlight && highlightedSprite != null)

            {

                spriteRenderer.sprite = highlightedSprite;  // Change to highlighted sprite

            }

            else if (normalSprite != null)

            {

                spriteRenderer.sprite = normalSprite;  // Change back to normal sprite

            }

        }

    }

}
