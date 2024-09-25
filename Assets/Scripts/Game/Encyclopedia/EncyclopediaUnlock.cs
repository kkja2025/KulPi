using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EncyclopediaUnlock : Panel
{
    [SerializeField] private RectTransform itemContent = null;
    [SerializeField] private GameObject itemPrefab = null;
    [SerializeField] private Button actionButton = null;
    [SerializeField] private string actionButtonNavigation = "unlock";
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
        switch (actionButtonNavigation)
        {
            case "unlock":
                PanelManager.GetSingleton(actionButtonNavigation).Close();
                break;
            case "unlocktikbalang":
                PanelManager.GetSingleton(id).Close();
                GameManager.Singleton.UnlockEncyclopediaItem("Tikbalang", "unlocktikbalang");
                break;
            case "chapter1":
                SceneManager.LoadScene("Chapter1");
                break;
            case "chapter2":
                SceneManager.LoadScene("Chapter2");
                break;
            default:
                break;
        }
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