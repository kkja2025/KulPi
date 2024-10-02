using System;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemID;
    public string itemName;
    public Sprite itemIcon;

    // Constructor
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