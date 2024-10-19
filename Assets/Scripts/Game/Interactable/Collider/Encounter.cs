using UnityEngine;
using UnityEngine.SceneManagement;
 
public class Encounter : Interactable
{
    [SerializeField] private string sceneName;
    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject player = GameObject.FindWithTag("Player");

        Scene currentScene = SceneManager.GetActiveScene();
        EncounterData encounterData = new EncounterData(gameObject.name, transform.position, player.transform.position, currentScene.name);
        if (collision.gameObject == player)
        {
            PanelManager.LoadSceneAsync(sceneName);
            GameManager.Singleton.SetActiveEncounter(encounterData);
            await GameManager.Singleton.SavePlayerData();
        }
    }
}