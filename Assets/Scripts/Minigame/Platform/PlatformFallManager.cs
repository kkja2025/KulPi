using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlatformFallManager : MiniGameManager
{
    [SerializeField] private Transform player;           
    [SerializeField] private Camera mainCamera;
    [SerializeField] int maxHP;
    [SerializeField] string sceneExit;
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    private int count = 0;
    private bool isCasualMode = false;
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
        mainCamera = Camera.main;
        AdjustCameraSize();
        InitializeScene();
    }
    
    protected override void Update()
    {
        base.Update();
        CheckGameOver();
    }

    public void StartGame()
    {
        VerticalScrollingCamera verticalScrollingCamera = mainCamera.GetComponent<VerticalScrollingCamera>();
        isTimerRunning = true;
        verticalScrollingCamera.StartScrolling();
    }

    public void StartCasualGame()
    {
        isCasualMode = true;
        StartGame();
    }

    private void AdjustCameraSize()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        float targetAspect = 16f / 9f;

        if (aspectRatio >= targetAspect)
        {
            mainCamera.orthographicSize = 5f;
        }
        else
        {
            mainCamera.orthographicSize = 5f * (targetAspect / aspectRatio);
        }
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

    public void GameOver()
    {
        if(!isCasualMode)
        {
            count++;
            if(count >= maxHP)
            {
            Time.timeScale = 0;
            PanelManager.GetSingleton("gameover").Open();
            }
            else
            {
                RespawnPlayer();
            }  
        } 
        else
        {
            RespawnPlayer(); 
        }
    }

    private void RespawnPlayer()
    {
        Vector3 centerScreenPosition = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, mainCamera.nearClipPlane));

        Vector3 startRayPosition = new Vector3(centerScreenPosition.x, mainCamera.transform.position.y - mainCamera.orthographicSize, player.position.z);
        RaycastHit2D hit = Physics2D.Raycast(startRayPosition, Vector2.up, Mathf.Infinity, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            Vector3 spawnPosition = new Vector3(centerScreenPosition.x, hit.point.y + 2.0f, player.position.z);
            player.position = spawnPosition;
        }
        else
        {
            Debug.LogWarning("No ground platform found! Player will not respawn.");
        }
    }

    public async void Finish()
    {
        Vector3 newPosition = new Vector3(x, y, z);
        RemoveEncounter();
        if (GameManager.Singleton != null)
        {
            await GameManager.Singleton.SavePlayerDataPosition(newPosition);
            PlayerData player = GameManager.Singleton.GetPlayerData();
            Debug.Log("Player position: " + player.GetPosition());
        }
        
        PanelManager.LoadSceneAsync(sceneExit);
    }

    public async void ShowVictoryMenu()
    {
        isTimerRunning = false;
        if (!isCasualMode)
        {
            await LeaderboardManager.Singleton.SubmitTimeChapter1SacredGrove((long)(elapsedTime * 1000));
        }
        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as VictoryMenuSacredGrove;
        if (victoryMenu != null)
        {
            victoryMenu.SetTimerText($"Time: {timerText.text}");
            victoryMenu.Open();
        }
    }
}
