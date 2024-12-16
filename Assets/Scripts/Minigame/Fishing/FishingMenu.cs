using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishingMenu : Panel
{
    [SerializeField] private Button CloseButton = null;
    [SerializeField] private Button TutorialButton = null;
    [SerializeField] private Button FishingButton = null;
    [SerializeField] private Button CompleteButton = null;
    [SerializeField] private RectTransform catchZone;
    [SerializeField] private RectTransform fishIcon;
    [SerializeField] private Slider slider;
    [SerializeField] private Slider progressSlider;

    [SerializeField] private float catchZoneSpeed = 150f;
    [SerializeField] private float fishIncreaseRate = 0.3f;
    [SerializeField] private float fishDecreaseRate = 0.5f;
    [SerializeField] private float catchZoneSize = 0.2f;
    [SerializeField] private float progressIncreaseRate = 0.1f;
    [SerializeField] private float progressDecreaseRate = 0.2f;
    private FishingInteraction fishItem;

    private float catchZoneMinX, catchZoneMaxX;
    private bool isHoldingButton;
    private bool isCompleted = false;
    private bool isFishing = false;
    private bool isRead = false;

    public override void Initialize()
    {
        base.Initialize();
        CloseButton.onClick.AddListener(Close);
        TutorialButton.onClick.AddListener(OpenTutorial);
        CompleteButton.onClick.AddListener(OnCompleteButtonClicked);
        SetupCatchZone();
        SetupFishingButtonEvent();
    }

    private void Update()
    {
        if(isFishing)
        {
            if (!isCompleted)
            {
                MoveCatchZone();
                UpdateFishPosition();
            }
            CheckCatchSuccess();
        }
    }

    public override void Open()
    {
        PanelManager.GetSingleton("hud").Close();
        slider.value = 0f;
        progressSlider.value = 0f;
        isCompleted = false;
        base.Open();
        PanelManager.GetSingleton("fishcomplete").Close();
        if(!isRead)
        {
            OpenTutorial();
            isRead = true;
        }
    }

    public override void Close()
    {
        isFishing = false;
        base.Close();
        PanelManager.GetSingleton("hud").Open();
    }

    public void OpenTutorial()
    {
        var tutorial = PanelManager.GetSingleton("tutorial");
        if (tutorial != null)
        {
            Time.timeScale = 0;
            tutorial.Open();
        } else {
            return;
        }
    }

    public void StartFishing(FishingInteraction data)
    {
        Open();
        isFishing = true;
        fishItem = data;
    }

    public void OnCompleteButtonClicked()
    {
        Close();
        fishItem.OnQuestItemCompletion();
    }

    private void SetupCatchZone()
    {
        float sliderWidth = slider.GetComponent<RectTransform>().rect.width;
        catchZone.sizeDelta = new Vector2(sliderWidth * catchZoneSize, catchZone.sizeDelta.y);

        catchZoneMinX = -sliderWidth / 2 + catchZone.rect.width / 2;
        catchZoneMaxX = sliderWidth / 2 - catchZone.rect.width / 2;
    }

    private void MoveCatchZone()
    {
        float newX = catchZone.localPosition.x + catchZoneSpeed * Time.deltaTime;
        if (newX > catchZoneMaxX || newX < catchZoneMinX)
        {
            catchZoneSpeed *= -1;
        }
        catchZone.localPosition = new Vector3(newX, catchZone.localPosition.y, 0);
    }

    private void UpdateFishPosition()
    {
        if (isHoldingButton)
        {
            slider.value += fishIncreaseRate * Time.deltaTime;
        }
        else
        {
            slider.value -= fishDecreaseRate * Time.deltaTime;
        }
        slider.value = Mathf.Clamp01(slider.value);
    }

    private void CheckCatchSuccess()
    {
        float fishX = fishIcon.localPosition.x;
        float catchZoneLeft = catchZone.localPosition.x - catchZone.rect.width / 2;
        float catchZoneRight = catchZone.localPosition.x + catchZone.rect.width / 2;

        if (fishX > catchZoneLeft && fishX < catchZoneRight)
        {
            progressSlider.value += progressIncreaseRate * Time.deltaTime;
            if (progressSlider.value >= 1f)
            {
                progressSlider.value = 1f;
                isCompleted = true;
                PanelManager.GetSingleton("fishcomplete").Open();
            }
        }
        else
        {
            progressSlider.value -= progressDecreaseRate * Time.deltaTime;
        }

        progressSlider.value = Mathf.Clamp01(progressSlider.value);
    }

    // Setup Button Events for Pointer Down/Up
    private void SetupFishingButtonEvent()
    {
        // Get EventTrigger from FishingButton
        EventTrigger trigger = FishingButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = FishingButton.gameObject.AddComponent<EventTrigger>(); // Add EventTrigger component if not already added
        }

        // Add PointerDown Event
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pointerDownEntry.callback.AddListener((eventData) => OnPointerDown((PointerEventData)eventData));
        trigger.triggers.Add(pointerDownEntry);

        // Add PointerUp Event
        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        pointerUpEntry.callback.AddListener((eventData) => OnPointerUp((PointerEventData)eventData));
        trigger.triggers.Add(pointerUpEntry);
    }

    // These are the methods that will be called when the pointer is pressed or released
    public void OnPointerDown(PointerEventData eventData)
    {
        isHoldingButton = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHoldingButton = false;
    }
}
