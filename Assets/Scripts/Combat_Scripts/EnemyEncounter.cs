using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : MonoBehaviour
{
    [SerializeField] private string combatSceneName;

    private async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        Vector3 enemyPosition = transform.position;

        if (collision.gameObject == player)
        {
            PanelManager.GetSingleton("loading").Open();
            await GameManager.Singleton.SavePlayerDataWithOffset(enemyPosition);
            SceneManager.LoadScene(combatSceneName);
        }
    }
}