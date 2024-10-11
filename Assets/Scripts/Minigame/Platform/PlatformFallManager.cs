using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlatformFallManager : MonoBehaviour
{
    [SerializeField] private Transform player;           
    [SerializeField] private Camera mainCamera;       
    [SerializeField] protected TMP_Text timerText;
    private bool initialized = false; 
    protected bool isTimerRunning = false;
    protected float elapsedTime = 0f;
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
        CheckGameOver();
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
    
    public virtual void Start()
    {
        isTimerRunning = true;
    }

    private void CheckGameOver()
    {
        // Check if the player transform is assigned
        if (player != null)
        {
            // Calculate vertical limits based on camera size and aspect ratio
            float lowerBound = mainCamera.transform.position.y - mainCamera.orthographicSize; 
            float upperBound = mainCamera.transform.position.y + mainCamera.orthographicSize;

            // Calculate horizontal limits based on camera size and aspect ratio
            float leftLimit = mainCamera.transform.position.x - mainCamera.orthographicSize * mainCamera.aspect;
            float rightLimit = mainCamera.transform.position.x + mainCamera.orthographicSize * mainCamera.aspect;

            // Check if the player is below the calculated lower bound or outside the horizontal limits
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
        // Here you can implement your game over UI or restart logic
        Debug.Log("Game Over!"); // Placeholder for game over logic
        // Optionally reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartAsync()
    {
        Time.timeScale = 1;
        Scene currentScene = SceneManager.GetActiveScene();
        PanelManager.LoadSceneAsync(currentScene.name);
    }

    public async void ExitAsync()
    {
        
    }
}
