using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    public GameObject player; // Reference to the player object
    public string combatSceneName; // Name of the combat scene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            // Trigger combat encounter
            SceneManager.LoadScene(combatSceneName);
        }
    }
}