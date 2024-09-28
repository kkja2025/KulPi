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
        var game = GameManager.Singleton;
        if (game == null)
        {
            game = FindObjectOfType<GameManager>();
            if (game == null)
            {
                Debug.Log("Game Manager not found! Returning to Main Menu.");
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
                return;
            }
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
        Destroy(spawnsObject);
        PanelManager.GetSingleton("hud").Close();
        // LeaderboardManager.Singleton.SubmitTimeBossChapter1((long)(elapsedTime * 1000));
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as VictoryMenu;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText(timerText.text);
            victoryMenu.Open();
        }
    }
}
