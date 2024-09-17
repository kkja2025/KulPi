using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncyclopediaMenu : Panel
{
    [SerializeField] private Button figuresButton = null;
    [SerializeField] private Button eventsButton = null;
    [SerializeField] private Button practicesButton = null;
    [SerializeField] private Button mythologyButton = null;
    [SerializeField] private Button backButton = null;
    private const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = "encyclopedia_figures";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = "encyclopedia_events";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY = "encyclopedia_practices_and_traditions";
    private const string CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY = "encyclopedia_mythology_and_folklore";

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        figuresButton.onClick.AddListener(OpenFigures);
        eventsButton.onClick.AddListener(OpenEvents);
        practicesButton.onClick.AddListener(OpenPractices);
        mythologyButton.onClick.AddListener(OpenMythology);
        backButton.onClick.AddListener(ReturnToGame);

        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void OpenFigures()
    {
        EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY);
        PanelManager.GetSingleton("encyclopediapanels").Close();
        // PanelManager.GetSingleton("events").Close();
        // PanelManager.GetSingleton("practices").Close();
        // PanelManager.GetSingleton("mythology").Close();
        PanelManager.GetSingleton("figures").Open();
    }

    private void OpenEvents()
    {
        EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY);
        PanelManager.GetSingleton("encyclopediapanels").Close();
        // PanelManager.GetSingleton("figures").Close();
        // PanelManager.GetSingleton("practices").Close();
        // PanelManager.GetSingleton("mythology").Close();
        PanelManager.GetSingleton("events").Open();
    }

    private void OpenPractices()
    {
        EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY);
        PanelManager.GetSingleton("encyclopediapanels").Close();
        // PanelManager.GetSingleton("figures").Close();
        // PanelManager.GetSingleton("mythology").Close();
        // PanelManager.GetSingleton("events").Close();
        PanelManager.GetSingleton("practices").Open();
    }

    private void OpenMythology()
    {
        EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY);
        PanelManager.GetSingleton("encyclopediapanels").Close();
        // PanelManager.GetSingleton("figures").Close();
        // PanelManager.GetSingleton("practices").Close();
        // PanelManager.GetSingleton("events").Close();
        PanelManager.GetSingleton("mythology").Open();
    }

    private void ReturnToGame()
    {
        PanelManager.GetSingleton("encyclopedia").Close();
    }
}
