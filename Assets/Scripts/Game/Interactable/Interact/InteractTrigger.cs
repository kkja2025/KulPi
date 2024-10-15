using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : DialogueInteractable
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            PlayerMovement playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
            playerMovement.isGrounded = true;
            OnInteractButtonClicked();
        }
    }

}
