using UnityEngine;
using UnityEngine.SceneManagement;
 
public class EnemyEncounter : Interactable
{
    [SerializeField] private string combatSceneName;
    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");

        Scene currentScene = SceneManager.GetActiveScene();
        EnemyEncounterData enemyData = new EnemyEncounterData(gameObject.name, transform.position, player.transform.position, currentScene.name);
        if (collision.gameObject == player)
        {
            PanelManager.LoadSceneAsync(combatSceneName);
            GameManager.Singleton.SetActiveEnemy(enemyData);
            await GameManager.Singleton.SavePlayerData();
        }
    }
}