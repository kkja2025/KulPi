using UnityEngine;
using UnityEngine.SceneManagement;
 
public class EnemyEncounter : Interactable
{
    [SerializeField] private string combatSceneName;
    private Chapter1GameManager gameManager;
    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        gameManager = GameManager.Singleton as Chapter1GameManager;
        if (gameManager == null)
        {
            Debug.LogError("Chapter1GameManager instance not found.");
            return;
        }

        Scene currentScene = SceneManager.GetActiveScene();
        EnemyEncounterData enemyData = new EnemyEncounterData(gameObject.name, transform.position, player.transform.position, currentScene.name);
        if (collision.gameObject == player)
        {
            PanelManager.LoadSceneAsync(combatSceneName);
            gameManager.SetActiveEnemy(enemyData);
            await gameManager.SavePlayerData();
        }
    }
}