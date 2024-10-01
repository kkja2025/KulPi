using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private bool initialized = false;
    protected bool isTimerRunning = false;
    private static BattleManager singleton = null;
    public GameObject spawnsObject;
    protected float elapsedTime = 0f;
    [SerializeField] public TMP_Text timerText;

    public static BattleManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<BattleManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("BattleManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    protected void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StartClientService();
    }

    void Update()
    {
        if (isTimerRunning) 
        {
            elapsedTime += Time.deltaTime; 
            UpdateTimerDisplay();
        }
    }

    private async void StartClientService()
    {
        if (GameManager.Singleton == null)
        {
            Debug.Log("Game Manager not found! Returning to Main Menu.");
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
            return;
        } else
        {
            PanelManager.CloseAll();
            PanelManager.GetSingleton("loading").Open();
            await Task.Delay(2000);
            PanelManager.CloseAll();
            PanelManager.GetSingleton("hud").Open();
            PanelManager.GetSingleton("tutorial").Open();
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    public virtual void StartBattle()
    {
        spawnsObject.SetActive(true);
        isTimerRunning = true;
    }

    public virtual void Defeated()
    {
        DestroyEnemy();
        PanelManager.GetSingleton("hud").Close();
        PanelManager.GetSingleton("victory").Open();
    }

    public void DestroyEnemy()
    {
        EnemyEncounterData enemyData = GameManager.Singleton.GetActiveEnemy();
        GameObject enemy = new GameObject(enemyData.GetEnemyID());
        GameManager.Singleton.RemoveObject(enemy);

    }

    public async void ExitBattle()
    {
        EnemyEncounterData enemyData = GameManager.Singleton.GetActiveEnemy();
        GameObject enemy = new GameObject(enemyData.GetEnemyID());
        enemy.transform.position = enemyData.GetPosition();
        await GameManager.Singleton.SavePlayerDataWithOffset(enemy, enemyData.GetPlayerPosition());
        GameManager.Singleton.SetActiveEnemy(null);
        SceneManager.LoadScene("Chapter1");
    }

    public void Restart()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
