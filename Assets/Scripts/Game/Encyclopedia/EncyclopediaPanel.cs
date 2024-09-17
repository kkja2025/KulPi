using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EncyclopediaPanel : Panel
{
    [SerializeField] private RectTransform menuContent = null;
    [SerializeField] private GameObject menuItemPrefab = null;
    [SerializeField] private RectTransform buttonContent = null; 
    [SerializeField] private GameObject buttonPrefab = null;
    [SerializeField] private string keyType;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY = EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY = EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

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
        LoadEntryList(keyType);
    }

    private async void LoadEntryList(string key)
    {
        foreach (Transform child in buttonContent)
        {
            Destroy(child.gameObject);
        }

        switch (key)
        {
            case "figures":
                await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY);
                break;
            case "events":
                await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY);
                break;
            case "practices":
                await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY);
                break;
            case "mythology":
                await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY);
                break;
            default:
                Debug.LogWarning("Invalid encyclopedia key provided.");
                return;
        }

        var items = EncyclopediaManager.Singleton.encyclopediaList;

        foreach (var item in items)
        {
            AddEntryButton(item);
        }
    }

    private void AddEntryButton(EncyclopediaItem item)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContent);
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = item.itemChapter;
        }

        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadEntryData(item));
        }
    }

    private void LoadEntryData(EncyclopediaItem item)
    {
        foreach (Transform child in menuContent)
        {
            Destroy(child.gameObject);
        }

        AddMenuItem(item);
    }

    private void AddMenuItem(EncyclopediaItem item)
    {
        GameObject newItem = Instantiate(menuItemPrefab, menuContent);

        TMP_Text itemTitleText = newItem.GetComponentInChildren<TMP_Text>();
        Image itemIconImage = newItem.GetComponentInChildren<Image>();
        TMP_Text itemDescriptionText = newItem.GetComponentInChildren<TMP_Text>();

        if (itemTitleText != null)
        {
            itemTitleText.text = item.itemTitle;
        }

        if (itemIconImage != null)
        {
            itemIconImage.sprite = item.itemIcon;
        }

        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = item.itemDescription;
        }
    }
}
