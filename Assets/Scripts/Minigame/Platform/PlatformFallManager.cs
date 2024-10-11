using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlatformFallManager : MiniGameManager
{
    [SerializeField] private Transform player;           
    [SerializeField] private Camera mainCamera;
    private static PlatformFallManager singleton = null;

    public static PlatformFallManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<PlatformFallManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("PlatformFallManager not found in the scene!");
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
        InitializeScene();
    }
    
    protected override void Update()
    {
        base.Update();
        CheckGameOver();
    }

    public async void StartGame()
    {
        VerticalScrollingCamera verticalScrollingCamera = mainCamera.GetComponent<VerticalScrollingCamera>();
        isTimerRunning = true;
        verticalScrollingCamera.StartScrolling();
    }

    private void CheckGameOver()
    {
        if (player != null)
        {
            float lowerBound = mainCamera.transform.position.y - mainCamera.orthographicSize; 
            float upperBound = mainCamera.transform.position.y + mainCamera.orthographicSize;

            float leftLimit = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
            float rightLimit = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect;

            if (player.position.y < lowerBound ||
                player.position.y > upperBound ||
                player.position.x < leftLimit ||
                player.position.x > rightLimit)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        RespawnPlayerInCenter();
    }

    private void RespawnPlayerInCenter()
    {
        Vector3 centerScreenPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));

        float upwardOffset = 2.0f;
        
        player.position = new Vector3(centerScreenPosition.x, centerScreenPosition.y + upwardOffset, player.position.z);
    }

    public void Finish()
    {
        Debug.Log("Triggered finish");
    }

    public async void ShowVictoryMenu()
    {
        isTimerRunning = false;
        await LeaderboardManager.Singleton.SubmitTimeChapter1SacredGrove((long)(elapsedTime * 1000));
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as VictoryMenuSacredGrove;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText($"Time: {timerText.text}");
            victoryMenu.Open();
        }
    }

    public override void ExitAsync()
    {
        Debug.Log("Exit");
    }
}
