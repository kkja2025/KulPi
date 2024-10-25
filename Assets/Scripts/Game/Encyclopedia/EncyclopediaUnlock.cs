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

    public async void ActionButton()
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
            case "Chapter2":
                Vector3 startingPosition = new Vector3(0, 0, 0);
                PlayerData playerData = GameManager.Singleton.GetPlayerData();
                playerData.SetLevel("Chapter2");
                playerData.SetPosition(startingPosition);
                playerData.SetActiveQuest("Make your way to the village. Find out what is happening.");
                await CloudSaveManager.Singleton.SaveNewPlayerData(playerData);
                PanelManager.LoadSceneAsync(actionButtonNavigation);
                break;
            default:
                PanelManager.LoadSceneAsync(actionButtonNavigation);
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