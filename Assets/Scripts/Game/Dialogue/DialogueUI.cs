using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    public GameObject dialoguePanel;
    public TMP_Text characterNameText;
    public TMP_Text dialogueText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDialogue(string characterName, string dialogue)
    {
        dialoguePanel.SetActive(true);
        characterNameText.text = characterName;
        dialogueText.text = dialogue;

        Debug.Log("Showing Dialogue: " + characterName + ": " + dialogue);
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
