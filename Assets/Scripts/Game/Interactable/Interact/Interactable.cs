using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interactable : MonoBehaviour
{
    protected bool isPlayerInRange = false;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite normalSprite;
    [SerializeField] protected Sprite highlightedSprite;
    [SerializeField] protected AudioClip onCollisionSound;
    [SerializeField] protected string buttonText;
    [SerializeField] protected Button interactButton;
    protected TMP_Text interactButtonText;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonClicked);
            interactButtonText = interactButton.GetComponentInChildren<TMP_Text>();
        }
    }

    protected void ShowInteractButton()
    {
        if (!string.IsNullOrEmpty(buttonText))
        {
            interactButton.gameObject.SetActive(true);
            interactButtonText.text = buttonText;
        } 
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
            ShowInteractButton();
            AudioManager.Singleton.PlayBackgroundSound(onCollisionSound, false);
            HighlightObject(true); 
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactButton.gameObject.SetActive(false);
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
