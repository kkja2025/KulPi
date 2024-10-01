using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatPauseMenu : PauseMenu
{
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        retryButton.onClick.AddListener(Restart);
        base.Initialize();
    }

    private void Restart()
    {
        BattleManager.Singleton.Restart();   
    }

    protected override void ReturnMainMenu()
    {
        Time.timeScale = 1;
        BattleManager.Singleton.ExitBattle();
    }
}
