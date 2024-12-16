using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;
    [SerializeField] private int maxHP;
    [SerializeField] private GameObject dialogueContainer;
    [SerializeField] private List<string> dialogueLines; // List of dialogue lines
    [SerializeField] private float dialogueDisplayDuration = 5f; // Duration to display the text

    private int health;
    private int currentDialogueIndex = 0;
    private TextMeshProUGUI dialogueText; // Reference to the TMP Text component in the dialogueContainer

    private void Start()
    {
        health = maxHP;
        UpdateHealthBar();

        if (dialogueContainer != null)
        {
            dialogueText = dialogueContainer.GetComponentInChildren<TextMeshProUGUI>();
            dialogueContainer.SetActive(false); // Initially hide the dialogue container
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercentage = (float)health / maxHP;
            healthBarFill.fillAmount = healthPercentage;
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public virtual void TakeUltimateDamage()
    {
        int damageAmount = 1;
        health = Mathf.Max(health - damageAmount, 0);
        UpdateHealthBar();
        ShowNextDialogue();
    }

    private void ShowNextDialogue()
    {
        if (dialogueLines != null && dialogueLines.Count > 0 && currentDialogueIndex < dialogueLines.Count)
        {
            if (dialogueText != null)
            {
                dialogueText.text = dialogueLines[currentDialogueIndex];
                currentDialogueIndex++;
                dialogueContainer.SetActive(true); // Show the dialogue container
                CancelInvoke(nameof(HideDialogue)); // Reset the timer if the method is already scheduled
                Invoke(nameof(HideDialogue), dialogueDisplayDuration); // Schedule to hide the text
            }
        }
    }

    private void HideDialogue()
    {
        if (dialogueContainer != null)
        {
            dialogueContainer.SetActive(false); // Hide the dialogue container
        }
    }
}
