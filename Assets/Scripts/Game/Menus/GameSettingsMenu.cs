using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameSettingsMenu : Panel
{
    [SerializeField] private Button BackButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        BackButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void Back()
    {
        PanelManager.GetSingleton("settings").Close();
        PanelManager.GetSingleton("pause").Open();
    }
}
