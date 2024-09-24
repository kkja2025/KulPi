using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaUnlock : Panel
{
    [SerializeField] protected RectTransform itemContent = null;
    [SerializeField] protected GameObject itemPrefab = null;
    [SerializeField] private Button backButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        backButton.onClick.AddListener(Back);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        LoadEntryData();
    }

    public virtual void Back()
    {
        Debug.Log("CloseUnlock");
        PanelManager.GetSingleton("unlock").Close();
    }

    public virtual void LoadEntryData()
    {
        var item = EncyclopediaManager.Singleton.encyclopediaList.Last();
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