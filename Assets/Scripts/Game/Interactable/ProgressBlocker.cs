using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBlocker : DialogueInteractable
{
    [SerializeField] private string completedObjective;
    private Collider2D barrierCollider;
    private Chapter1GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        gameManager = GameManager.Singleton as Chapter1GameManager;
        barrierCollider = GetComponent<Collider2D>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (completedObjective == gameManager.GetObjective())
            {
                if (barrierCollider != null)
                {
                    OnInteractButtonClicked();
                    barrierCollider.enabled = true;
                }
            } 
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
