using System.Linq;
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

        var chapters = EncyclopediaManager.Singleton.encyclopediaList.Select(item => item.itemChapter).Distinct().ToList();

        foreach (var chapter in chapters)
        {
            AddChapterButton(chapter);
        }
    }
    private void AddChapterButton(string chapter)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContent);
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = chapter;
        }

        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadEntryListTitles(chapter));
        }
    }

    private void LoadEntryListTitles(string chapter)
    {
        foreach (Transform child in buttonContent)
        {
            Destroy(child.gameObject);
        }

        AddBackButton(chapter);
        var titlesInChapter = EncyclopediaManager.Singleton.encyclopediaList.Select(item => item.itemTitle).Distinct().ToList()
            .Where(x => x.itemChapter == chapter) 
            .Select(x => x.itemTitle)             
            .ToList();

        foreach (var title in titlesInChapter)
        {
            AddTitleButton(title);
        }
    }

    private void AddBackButton(string chapter)
    {
        GameObject backButton = Instantiate(buttonPrefab, buttonContent);
        TMP_Text buttonText = backButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = chapter;
        }

        Button button = backButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadEntryList(keyType));
        }
    }

    private void AddTitleButton(string title)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonContent);
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = title;
        }

        Button button = newButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadEntryDataByTitle(title));
        }
    }

    private void LoadEntryDataByTitle(string title)
    {
        foreach (Transform child in menuContent)
        {
            Destroy(child.gameObject);
        }

        var selectedItem = EncyclopediaManager.Singleton.encyclopediaList.FirstOrDefault(x => x.itemTitle == title);

        if (selectedItem != null)
        {
            LoadEntryData(selectedItem);
        }
    }

    private void LoadEntryData(EncyclopediaItem item)
    {
        GameObject newItem = Instantiate(menuItemPrefab, menuContent);

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
