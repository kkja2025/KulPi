using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite highlightedSprite;
    [SerializeField] protected AudioClip onCollisionSound;
    protected Button interactButton;
    protected InteractButtonPositioner buttonPositioner;

    protected virtual void Awake()
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
                interactButton.onClick.AddListener(OnInteractButtonClicked); 
            }
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'InteractButton' found.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void OnInteractButtonClicked()
    {
        if (isPlayerInRange)
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        Debug.Log("Interacting with " + gameObject.name);
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

            AudioManager.Singleton.PlayBackgroundSound(onCollisionSound, false);
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
            HighlightObject(false); 
        }
    }

    protected virtual void HighlightObject(bool highlight)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = highlight && highlightedSprite != null ? highlightedSprite : normalSprite;
        }
    }
}
