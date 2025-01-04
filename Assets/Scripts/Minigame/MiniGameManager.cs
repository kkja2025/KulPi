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
            AudioManager.Singleton.OnSceneLoaded();
        },
        () =>
        {
            AudioManager.Singleton.OnSceneLoaded();
            PanelManager.CloseAll();
            PanelManager.GetSingleton("tutorial").Open();
        });
    }

    protected void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(elapsedTime);
            timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        }
    }

    public void AddTime(float secondsToAdd)
    {
        elapsedTime += secondsToAdd;
        UpdateTimerDisplay();         
    }

    public void RestartAsync()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        PanelManager.LoadSceneAsync(currentScene.name);
    }

    public virtual async void ExitAsync()
    {
        var encounterData = GameManager.Singleton.GetActiveEncounter();
        if (GameManager.Singleton != null)
        {
            if (encounterData != null)
            {
                GameObject encounter = new GameObject(encounterData.GetEncounterID());
                encounter.transform.position = encounterData.GetPosition();
                await GameManager.Singleton.SavePlayerDataWithOffset(encounter, encounterData.GetPlayerPosition());
                GameManager.Singleton.SetActiveEncounter(null);
            }
        }
        
        PanelManager.LoadSceneAsync(encounterData.GetSceneName());
    }

    public async void RemoveEncounter()
    {
        EncounterData encounterData = GameManager.Singleton.GetActiveEncounter();
        GameObject encounter = new GameObject(encounterData.GetEncounterID());
        RemovedObjectsManager.Singleton.RemoveObject(encounter);
        await RemovedObjectsManager.Singleton.SaveRemovedObjectsAsync();
    }
}
