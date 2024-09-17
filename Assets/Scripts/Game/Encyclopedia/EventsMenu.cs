using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventsMenu : Panel
{
    [SerializeField] private RectTransform eventsContent = null;
    [SerializeField] private GameObject eventsItemPrefab = null;
    [SerializeField] private RectTransform eventsButtonContent = null; 
    [SerializeField] private GameObject eventsButtonPrefab = null;

    private const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = "encyclopedia_events";

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
        foreach (Transform child in eventsButtonContent)
        {
            Destroy(child.gameObject);
        }
        
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY);
        var items = EncyclopediaManager.Singleton.encyclopediaList;

        foreach (var item in items)
        {
            AddEventsButton(item);
        }
    }

    private void AddEventsButton(EncyclopediaItem item)
    {
        GameObject newEventsButton = Instantiate(eventsButtonPrefab, eventsButtonContent);
        TMP_Text buttonText = newEventsButton.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
        {
            buttonText.text = item.itemChapter;
        }

        Button button = newEventsButton.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => LoadEventInformation(item));
        }
    }

    private void LoadEventInformation(EncyclopediaItem item)
    {
        foreach (Transform child in eventsContent)
        {
            Destroy(child.gameObject);
        }

        AddEventItem(item);
    }

    private void AddEventItem(EncyclopediaItem item)
    {
        GameObject newItem = Instantiate(eventsItemPrefab, eventsContent);

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
