using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FiguresMenu : Panel
{
    [SerializeField] private RectTransform figuresContent = null;
    [SerializeField] private GameObject figuresItemPrefab = null;
    [SerializeField] private RectTransform figuresButtonContent = null; 
    [SerializeField] private GameObject figuresButtonPrefab = null;

    private const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = "encyclopedia_figures";


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
        LoadChapters();
    }

    private async void LoadChapters()
    {
        foreach (Transform child in figuresButtonContent)
        {
            Destroy(child.gameObject);
        }
        
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY);
        var items = EncyclopediaManager.Singleton.encyclopediaList;

        foreach (var item in items)
        {
            AddFiguresButton(item);
        }
    }

    private void AddFiguresButton(EncyclopediaItem item)
    {
        GameObject newFiguresButton = Instantiate(figuresButtonPrefab, figuresButtonContent);
        TMP_Text buttonText = newFiguresButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = item.itemChapter;
        }

        Button button = newFiguresButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadFigureInformation(item));
        }
    }

    private void LoadFigureInformation(EncyclopediaItem item)
    {
        foreach (Transform child in figuresContent)
        {
            Destroy(child.gameObject);
        }

        AddFigureItem(item);
    }

    private void AddFigureItem(EncyclopediaItem item)
    {
        GameObject newItem = Instantiate(figuresItemPrefab, figuresContent);

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
