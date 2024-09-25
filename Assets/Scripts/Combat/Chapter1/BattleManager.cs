using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    private bool initialized = false;
    private bool isTimerRunning = false;
    private static BattleManager singleton = null;
    [SerializeField] public GameObject ultimateButton;
    private int ultimateUses = 0;
    private Boss boss;
    public GameObject bossObject;
    public GameObject minionsObject;
    private float elapsedTime = 0f;
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

    private void Initialize()
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

    private void StartClientService()
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

    public void ShowUltimateButton()
    {
        ultimateButton.SetActive(true);
    }

    public void StartBattle()
    {
        bossObject.SetActive(true);
        minionsObject.SetActive(true);
        boss = bossObject.GetComponent<Boss>();
        isTimerRunning = true;
    }

    public void UseUltimate()
    {
        ultimateButton.SetActive(false);
        ultimateUses++;

        boss.TakeUltimateDamage();
        Debug.Log("Ultimate used! Ultimate uses: " + ultimateUses);

        if (ultimateUses >= 5)
        {
            Debug.Log("Boss defeated!");
        }
    }

    public void Defeated()
    {
        Debug.Log("Boss destroyed!");
        Destroy(bossObject);
        Destroy(minionsObject);
        PanelManager.GetSingleton("hud").Close();
        LeaderboardManager.Singleton.SubmitTimeBossChapter1((long)(elapsedTime * 1000));
        WinMenu winMenu = PanelManager.GetSingleton("win") as WinMenu;
        if (winMenu != null)
        {
            winMenu.SetTimerText(timerText.text);
            winMenu.Open();
        }
    }
}
