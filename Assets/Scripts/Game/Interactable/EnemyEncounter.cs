using UnityEngine;
 
public class EnemyEncounter : Interactable
{
    [SerializeField] private string combatSceneName;

    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");
        EnemyEncounterData enemyData = new EnemyEncounterData(gameObject.name, transform.position, player.transform.position);
        if (collision.gameObject == player)
        {
            PanelManager.LoadSceneAsync(combatSceneName);
            GameManager.Singleton.SetActiveEnemy(enemyData);
            await GameManager.Singleton.SavePlayerData();
        }
    }
}