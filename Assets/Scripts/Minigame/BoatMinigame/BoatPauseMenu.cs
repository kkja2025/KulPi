using UnityEngine;
using UnityEngine.UI;

public class BoatPauseMenu : PauseMenu
{
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        base.Initialize();
        retryButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        BoatManager.Singleton.RestartAsync();   
    }

    protected override void ReturnMainMenu()
    {
        BoatManager.Singleton.ExitAsync();
    }
}
