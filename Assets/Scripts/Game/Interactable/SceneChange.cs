using UnityEngine;
 
public class SceneChange : Interactable
{
    [SerializeField] private string sceneName;

    protected override async void OnTriggerEnter2D(Collider2D collision)
    {
        Vector3 defaultPosition = new Vector3(0, 0, 0);
        GameObject player = GameObject.FindWithTag("Player");
        if (collision.gameObject == player)
        {
            PanelManager.LoadSceneAsync(sceneName);

            PlayerData playerData = GameManager.Singleton.GetPlayerData();
            playerData.SetLevel(sceneName);
            playerData.SetPosition(defaultPosition);
            
            await GameManager.Singleton.SavePlayerData();
        }
    }
}