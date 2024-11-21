using UnityEngine;
using UnityEngine.UI;

public class BoatManager : MiniGameManager
{
    private static BoatManager singleton = null;

    [SerializeField] private GameObject boat; // Reference to the boat GameObject
    [SerializeField] private GameObject obstacleSpawner; // Reference to the obstacle spawner GameObject
    [SerializeField] private float x;
    [SerializeField] private float y;
    [SerializeField] private float z;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private float duration = 60f; // Game duration in seconds
    private BoatController boatController;    // Reference to the BoatController component
    private bool isCasualMode = false;

    public static BoatManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<BoatManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("BoatManager not found in the scene!");
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

        elapsedTime = duration;

        // Ensure the BoatController is fetched from the boat GameObject
        if (boat != null)
        {
            boatController = boat.GetComponent<BoatController>();
            if (boatController == null)
            {
                Debug.LogError("BoatController component not found on the assigned boat GameObject!");
            }
            UpdateHealthBar();
        }
        else
        {
            Debug.LogError("Boat GameObject is not assigned in the BoatManager!");
        }

        InitializeScene();
    }

    protected override void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime -= Time.deltaTime;
            UpdateTimerDisplay();
            if (!isCasualMode)
            {
                UpdateHealthBar();
            }
            if (elapsedTime <= 0f)
            {
                elapsedTime = 0f;
                ShowVictoryMenu();
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float healthPercentage = (float)(boatController.maxHP - boatController.damage) / boatController.maxHP;
            healthBarFill.fillAmount = healthPercentage;
            if (healthPercentage <= 0f)
            {
                ShowGameOverMenu();
            }
        }
    }

    public void StartGame()
    {
        isCasualMode = true;
        isTimerRunning = true;
        PanelManager.GetSingleton("tutorial").Close();
        PanelManager.GetSingleton("hud").Open();

        if (boatController != null)
        {
            boatController.StartGame(); // Start the boat's movement
            obstacleSpawner.SetActive(true); // Enable obstacle spawning
        }
    }

    public void ShowGameOverMenu()
    {
        Time.timeScale = 0f; // Pause the game
        PanelManager.GetSingleton("gameover").Open();
    }

    public async void ShowVictoryMenu()
    {
        isTimerRunning = false;
        obstacleSpawner.SetActive(false); // Stop obstacle spawning
        AudioManager.Singleton.PlayVictoryMusic();
        if (GameManager.Singleton != null)
        {
            Vector3 newPosition = new Vector3(x, y, z);
            await GameManager.Singleton.SavePlayerDataPosition(newPosition);
            PlayerData player = GameManager.Singleton.GetPlayerData();
        }

        // Example of submitting score to the leaderboard
        // await LeaderboardManager.Singleton.SubmitTimeChapter1SacredGrove((long)(elapsedTime * 1000));

        VictoryMenu victoryMenu = PanelManager.GetSingleton("victory") as BoatVictoryMenu;
        if (victoryMenu != null)
        {
            // victoryMenu.SetTimerText($"Time: {timerText.text}");
            victoryMenu.Open();
        }
    }
}
