using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsMenu : Panel
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
        PanelManager.CloseAll();
        PanelManager.GetSingleton("main").Open();
    }
}
