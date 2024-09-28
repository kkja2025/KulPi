using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private string combatSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (collision.gameObject == player)
        {
            // Trigger combat encounter
            SceneManager.LoadScene(combatSceneName);
        }
    }
}