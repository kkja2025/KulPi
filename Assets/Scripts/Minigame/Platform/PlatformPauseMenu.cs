using UnityEngine;
using UnityEngine.UI;

public class PlatformPauseMenu : PauseMenu
{
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        base.Initialize();
        retryButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        PlatformFallManager.Singleton.RestartAsync();   
    }

    protected override void ReturnMainMenu()
    {
        PlatformFallManager.Singleton.ExitAsync();
    }
}
