using UnityEngine;
using TMPro;

public class DialogueUI : Panel
{
    [SerializeField] private TMP_Text characterNameText;
    [SerializeField] private TMP_Text dialogueText;

    public void ShowDialogue(string characterName, string dialogue)
    {
        characterNameText.text = characterName;
        dialogueText.text = dialogue;
        
        Debug.Log("Showing Dialogue: " + characterName + ": " + dialogue);
    }
}
