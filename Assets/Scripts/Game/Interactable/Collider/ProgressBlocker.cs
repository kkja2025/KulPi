using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBlocker : DialogueInteractable
{
    [SerializeField] private string completedObjective = null;
    [SerializeField] private string currentObjective = null;
    private Collider2D barrierCollider;

    protected override void Awake()
    {
        base.Awake();
        barrierCollider = GetComponent<Collider2D>();
    }

    protected override void OnInteractButtonClicked()
    {
        PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        if (isConversationComplete)
        {
            ConversationCompleted();
            isConversationComplete = false;
            // playerMovement.OnEnable();
        }
        else if (isPlayerInRange && !isConversationComplete)
        {
            PanelManager.GetSingleton("dialogue").Open();
            if(dialogueInteractButton == null)
            {
                GameObject buttonObject = GameObject.FindWithTag("DialogueInteractButton");
                dialogueInteractButton = buttonObject.GetComponent<Button>();
                dialogueInteractButton.onClick.AddListener(OnInteractButtonClicked);
            }
            Interact();
            // playerMovement.OnDisable();
        }         
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (currentObjective == GameManager.Singleton.GetObjective())
            {
                Destroy(gameObject);
            }
            else if (string.IsNullOrEmpty(completedObjective) || completedObjective == GameManager.Singleton.GetObjective())
            {
                Debug.Log("Opening dialogue. Objective is either null or matched.");

                if (barrierCollider != null)
                {
                    PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
                    playerMovement.isGrounded = true;
                    OnInteractButtonClicked();
                    barrierCollider.enabled = true; 
                }
            }
            else
            {
                Debug.Log("Objective completed. Destroying the barrier.");
                Destroy(gameObject);
            }
        }
    }

}
