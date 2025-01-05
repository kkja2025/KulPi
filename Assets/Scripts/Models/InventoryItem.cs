using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItemList
{
    public List<InventoryItem> items = null;
}

[System.Serializable]
public class InventoryItem
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;

    public InventoryItem(string id, string name)
    {
        itemID = id;
        itemName = name;
        itemIcon = null;
    }
    public void SetSprite(Sprite sprite)
    {
        itemIcon = sprite;
    }
}