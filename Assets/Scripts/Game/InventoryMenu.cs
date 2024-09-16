using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : Panel
{
    [SerializeField] private Button closeButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        closeButton.onClick.AddListener(CloseInventory);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void CloseInventory()
    {
        PanelManager.GetSingleton("inventory").Close();
    }

}
