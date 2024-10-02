using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyEncounter : Interactable
{
    [SerializeField] private string combatSceneName;

    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        EnemyEncounterData enemyData = new EnemyEncounterData(gameObject.name, transform.position, player.transform.position);
        if (collision.gameObject == player)
        {
            GameManager.Singleton.SetActiveEnemy(enemyData);
            PanelManager.GetSingleton("loading").Open();
            await GameManager.Singleton.SavePlayerData();
            SceneManager.LoadScene(combatSceneName);
        }
    }
}