using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    public GameObject dialoguePanel;
    public Text characterNameText;
    public Text dialogueText;

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
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
