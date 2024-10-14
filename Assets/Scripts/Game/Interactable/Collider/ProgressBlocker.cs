using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBlocker : DialogueInteractable
{
    [SerializeField] private string completedObjective = null;
    private Collider2D barrierCollider;

    protected override void Awake()
    {
        base.Awake();
        barrierCollider = GetComponent<Collider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (string.IsNullOrEmpty(completedObjective) || completedObjective == GameManager.Singleton.GetObjective())
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
