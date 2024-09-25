using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaUnlock : Panel
{
    [SerializeField] private RectTransform itemContent = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private Button actionButton = null;
    private EncyclopediaItem encyclopediaItem = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        actionButton.onClick.AddListener(ActionButton);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        LoadEntryData(encyclopediaItem);
    }

    public void SetEncyclopediaItem(EncyclopediaItem item)
    {
        if (item != null)
        {
            encyclopediaItem = item;
        }
    }

    public void ActionButton()
    {
        PanelManager.GetSingleton("unlock").Close();
    }

    public void LoadEntryData(EncyclopediaItem item)
    {
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