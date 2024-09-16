using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool initialized = false;
    private static GameManager singleton = null;

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("GameManager not found in the scene!");
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
        Application.runInBackground = true;
        StartClientService();
    }

    private void StartClientService()
    {
        PanelManager.CloseAll();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPopUp(PopUpMenu.Action action = PopUpMenu.Action.None, string text = "", string button = "")
    {
        PanelManager.Close("loading");
        PopUpMenu panel = (PopUpMenu)PanelManager.GetSingleton("popup");
        panel.Open(action, text, button);
    }
}
