using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button encyclopediaButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        pauseButton.onClick.AddListener(OpenPause);
        inventoryButton.onClick.AddListener(OpenInventory);
        encyclopediaButton.onClick.AddListener(OpenEncyclopedia);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void OpenPause()
    {
        PanelManager.GetSingleton("pause").Open();
    }

    private void OpenInventory()
    {
        PanelManager.GetSingleton("inventory").Open();
    }

    private void OpenEncyclopedia()
    {
        PanelManager.GetSingleton("encyclopedia").Open();
    }
}
