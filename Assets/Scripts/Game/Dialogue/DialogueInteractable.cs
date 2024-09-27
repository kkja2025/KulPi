using System.Collections.Generic;
using UnityEngine;

public class DialogueInteractable : Interactable
{
    public string characterName;
    public List<string> dialogueLines;
    public string playerResponse;

    private int currentDialogueIndex = 0;

    public override void Interact()
    {
        if (currentDialogueIndex < dialogueLines.Count)
        {
            DialogueUI.Instance.ShowDialogue(characterName, dialogueLines[currentDialogueIndex]);
            currentDialogueIndex++;
        }
        else
        {
            DialogueUI.Instance.ShowDialogue("Lakan", playerResponse);
            ResetDialogue();
        }
    }

    private void ResetDialogue()
    {
        currentDialogueIndex = 0;
    }
}
