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
    }

    private void CloseInventory()
    {
        PanelManager.GetSingleton("inventory").Close();
    }

    private void LoadInventoryItems()
    {
        GameObject[] items = inventoryContent.GetComponentsInChildren<GameObject>();
        if (items != null)
        {
            for (int i = 0; i < items.Length; i++)
            {
                Destroy(items[i].gameObject);
            }
        }

    }

    private void AddInventoryItem(string itemName)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryContent);
        
        TMP_Text itemText = newItem.GetComponentInChildren<TMP_Text>();
        if (itemText != null)
        {
            itemText.text = itemName;
        }
    }
}
