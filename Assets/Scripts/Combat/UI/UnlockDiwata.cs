using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiwataUnlock : EncyclopediaUnlock
{
    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Back()
    {
        PanelManager.GetSingleton("unlock").Close();
        PanelManager.GetSingleton("win").Open();
    }

    public override void LoadEntryData()
    {
        var item = EncyclopediaItem.Figures_Chapter1_Diwata();
        foreach (Transform child in itemContent)
        {
            Destroy(child.gameObject);
        }

        GameObject newItem = Instantiate(itemPrefab, itemContent);

        TMP_Text[] textComponents = newItem.GetComponentsInChildren<TMP_Text>();
        Image itemIconImage = newItem.GetComponentInChildren<Image>();

        foreach (var text in textComponents)
        {
            if (text.name == "Title")
            {
                text.text = item.itemTitle;
            }
            else if (text.name == "Description")
            {
                text.text = item.itemDescription;
            }
        }

        Sprite icon = Resources.Load<Sprite>($"Icons/Encyclopedia/{item.itemID}");
        item.SetSprite(icon);
        if (itemIconImage != null)
        {   
            itemIconImage.sprite = item.itemIcon;
        }
    }
}