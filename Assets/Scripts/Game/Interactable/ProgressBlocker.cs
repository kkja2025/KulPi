using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProgressBlocker : DialogueInteractable
{
    [SerializeField] private string completedObjective;
    private Collider2D barrierCollider;
    private Chapter1GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        gameManager = GameManager.Singleton as Chapter1GameManager;
        barrierCollider = GetComponent<Collider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {   
            PanelManager.GetSingleton("dialogue").Open();
            Interact();
            if (completedObjective != gameManager.GetObjective())
            {
                if (barrierCollider != null)
                {
                    barrierCollider.enabled = true;
                }
            } 
            else
            {
                barrierCollider.enabled = false;
            }
            
        }
    }
}
