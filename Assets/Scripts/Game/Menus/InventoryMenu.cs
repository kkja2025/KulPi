using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryMenu : Panel
{
    [SerializeField] private Button closeButton = null;
    [SerializeField] private RectTransform inventoryContent = null;
    [SerializeField] private GameObject inventoryItemPrefab = null;

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
        LoadInventoryItems();
    }

    private void CloseInventory()
    {
        PanelManager.GetSingleton("inventory").Close();
    }

    private void LoadInventoryItems()
    {
        foreach (Transform child in inventoryContent)
        {
            Destroy(child.gameObject);
        }

        var items = InventoryManager.Singleton.inventory;

        foreach (var item in items)
        {
            AddInventoryItem(item);
        }
    }

    private void AddInventoryItem(InventoryItem item)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryContent);

        TMP_Text itemText = newItem.GetComponentInChildren<TMP_Text>();
        Image itemIconImage = newItem.GetComponentInChildren<Image>();

        if (itemText != null)
        {
            itemText.text = item.itemName;
        }

        if (itemIconImage != null)
        {
            itemIconImage.sprite = item.itemIcon;
        }
    }
}
