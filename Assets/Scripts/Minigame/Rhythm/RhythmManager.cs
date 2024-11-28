using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MiniGameManager
{
    [SerializeField] private GameObject rhythmController;

    private static RhythmManager singleton = null;

    public static RhythmManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<RhythmManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("RhythmManager not found in the scene!");
                }
            }
            return singleton;
        }
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
        rhythmController.SetActive(false);
        InitializeScene();
    }

    public void StartGame()
    {
        isTimerRunning = true;
        PanelManager.GetSingleton("tutorial").Close();
        PanelManager.GetSingleton("hud").Open();
        rhythmController.SetActive(true);
    }

    public void ShowVictoryMenu()
    {
        isTimerRunning = false;
        rhythmController.SetActive(false);
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as RhythmVictoryMenu;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText($"Time: {timerText.text}");
            victoryMenu.Open();
        }
    }

    public void ShowGameOverMenu()
    {
        isTimerRunning = false;
        rhythmController.SetActive(false);
        PanelManager.GetSingleton("gameover").Open();
    }
}
