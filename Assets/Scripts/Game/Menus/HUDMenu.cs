using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button encyclopediaButton = null;

    //Sprites for the inventory items
    [SerializeField] private Sprite PauseButton = null;
    [SerializeField] private Sprite Encyclopedia = null;

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
        InventoryManager.Singleton.RemoveItem("Encyclopedia Item");
        InventoryManager.Singleton.RemoveItem("Pause Item");
    }

    private void OpenInventory()
    {
        InventoryManager.Singleton.GetInventory();
        PanelManager.GetSingleton("inventory").Open();
    }

    private void OpenEncyclopedia()
    {
        PanelManager.GetSingleton("encyclopedia").Open();
        InventoryManager.Singleton.AddItem("Pause Item", PauseButton);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
        InventoryManager.Singleton.AddItem("Encyclopedia Item", Encyclopedia);
    }
}