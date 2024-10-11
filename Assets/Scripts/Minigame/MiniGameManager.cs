using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniGameManager : MonoBehaviour
{
    [SerializeField] protected TMP_Text timerText;
    private bool initialized = false; 
    protected bool isTimerRunning = false;
    protected float elapsedTime = 0f;

    protected void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    protected virtual void Update()
    {
        if (isTimerRunning) 
        {
            elapsedTime += Time.deltaTime; 
            UpdateTimerDisplay();
        }
    }

    protected void InitializeScene()
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

    public void RestartAsync()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        PanelManager.LoadSceneAsync(currentScene.name);
    }

    public virtual void ExitAsync()
    {
        
    }
}
