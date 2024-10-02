using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] protected TMP_Text timerText;
    [SerializeField] protected GameObject spawnsObject;
    private bool initialized = false;
    protected bool isTimerRunning = false;
    protected float elapsedTime = 0f;
    private static BattleManager singleton = null;

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

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }
    private void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
        }
        InitializeScene();
    }

    private void Update()
    {
        if (isTimerRunning) 
        {
            elapsedTime += Time.deltaTime; 
            UpdateTimerDisplay();
        }
    }

    private void InitializeScene()
    {
        if (GameManager.Singleton == null)
        {
            Debug.Log("Game Manager not found! Returning to Main Menu.");
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
            return;
        } else
        {
            PanelManager.LoadSceneAsync("");
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
        spawnsObject.SetActive(false);
    }

    public void DestroyEnemy()
    {
        EnemyEncounterData enemyData = GameManager.Singleton.GetActiveEnemy();
        GameObject enemy = new GameObject(enemyData.GetEnemyID());
        GameManager.Singleton.RemoveObject(enemy);

    }

    public async void ExitBattleAsync()
    {
        if (GameManager.Singleton != null)
        {
            var enemyData = GameManager.Singleton.GetActiveEnemy();
            if (enemyData != null)
            {
                GameObject enemy = new GameObject(enemyData.GetEnemyID());
                enemy.transform.position = enemyData.GetPosition();
                await GameManager.Singleton.SavePlayerDataWithOffset(enemy, enemyData.GetPlayerPosition());
                GameManager.Singleton.SetActiveEnemy(null);
            }
        }
        
        var asyncOperation = SceneManager.LoadSceneAsync("Chapter1");
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
    }

    public void RestartAsync()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        PanelManager.LoadSceneAsync(currentScene.name);
    }
}
