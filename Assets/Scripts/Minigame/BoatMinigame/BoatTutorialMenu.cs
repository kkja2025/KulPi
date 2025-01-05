using UnityEngine;
using UnityEngine.UI;


public class BoatTutorialMenu : Panel
{
    [SerializeField] private Button startButton = null;
    [SerializeField] private Button startCasualButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        if (startButton != null)
        {
        startButton.onClick.AddListener(StartGame);
        }
        if (startCasualButton != null)
        {
        startCasualButton.onClick.AddListener(StartCasualGame);
        }
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        var tutorial1 = PanelManager.GetSingleton("tutorial1");
        if (tutorial1 != null)
        {
            tutorial1.Open();
        } else {
            return;
        }
        var tutorialpause1 = PanelManager.GetSingleton("tutorialpause1");
        if (tutorialpause1 != null)
        {
            tutorialpause1.Open();
        } else {
            return;
        }
    }
    
    protected virtual void StartGame()
    {
        Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            BoatManager.Singleton.StartGame();
        }
    }

    private void StartCasualGame()
    {
        Close();
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        } else {
            BoatManager.Singleton.StartCasualGame();
        }
    }
}
