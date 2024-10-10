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
        PanelManager.Singleton.StartLoading(3f, 
        () => { 
            if (Time.timeScale != 1f)
            {
                Time.timeScale = 1f;
            }
        },
        () =>
        {
            PanelManager.GetSingleton("hud").Open();
            PanelManager.GetSingleton("tutorial").Open();
        });
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
        VictoryAnimation();
    }

    public void VictoryAnimation()
    {
        // Play victory animation
    }

    public void RestartAsync()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        PanelManager.LoadSceneAsync(currentScene.name);
    }

    public async void DestroyEnemy()
    {
        EnemyEncounterData enemyData = GameManager.Singleton.GetActiveEnemy();
        GameObject enemy = new GameObject(enemyData.GetEnemyID());
        RemovedObjectsManager.Singleton.RemoveObject(enemy);
        await RemovedObjectsManager.Singleton.SaveRemovedObjectsAsync();
    }

    public async void ExitBattleAsync()
    {
        var enemyData = GameManager.Singleton.GetActiveEnemy();
        if (GameManager.Singleton != null)
        {
            if (enemyData != null)
            {
                GameObject enemy = new GameObject(enemyData.GetEnemyID());
                enemy.transform.position = enemyData.GetPosition();
                await GameManager.Singleton.SavePlayerDataWithOffset(enemy, enemyData.GetPlayerPosition());
                GameManager.Singleton.SetActiveEnemy(null);
            }
        }
        
        PanelManager.LoadSceneAsync(enemyData.GetSceneName());
    }
}
