using UnityEngine;
using UnityEngine.UI;

public class RhythmPauseMenu : PauseMenu
{
    [SerializeField] private Button retryButton = null;

    public override void Initialize()
    {
        base.Initialize();
        retryButton.onClick.AddListener(Restart);
    }

    private void Restart()
    {
        RhythmManager.Singleton.RestartAsync();   
    }

    protected override void ReturnMainMenu()
    {
        RhythmManager.Singleton.ExitAsync();
    }
}
