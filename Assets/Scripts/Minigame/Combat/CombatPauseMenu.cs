using UnityEngine;
using UnityEngine.UI;

public class CombatPauseMenu : PauseMenu
{
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        base.Initialize();
        retryButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        BattleManager.Singleton.RestartAsync();   
    }

    protected override void ReturnMainMenu()
    {
        Time.timeScale = 1;
        BattleManager.Singleton.ExitAsync();
    }
}
