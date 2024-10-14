using UnityEngine;
 
public class QuestCheckpointInteraction : Interactable
{
    [SerializeField] private string newQuest;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (collision.gameObject == player)
        {
            if (newQuest != "")
            {
                GameManager.Singleton.SetObjective(newQuest);
            }
        }
    }
}