using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncyclopediaMenu : Panel
{
    [SerializeField] private Button figuresButton = null;
    [SerializeField] private Button eventsButton = null;
    [SerializeField] private Button practicesButton = null;
    [SerializeField] private Button mythologyButton = null;
    [SerializeField] private Button resumeButton = null;
    [SerializeField] private Button backButton = null;
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
        figuresButton.onClick.AddListener(OpenFigures);
        eventsButton.onClick.AddListener(OpenEvents);
        practicesButton.onClick.AddListener(OpenPractices);
        mythologyButton.onClick.AddListener(OpenMythology);
        resumeButton.onClick.AddListener(ReturnToGame);
        backButton.onClick.AddListener(ShowAllButtons);

        backButton.gameObject.SetActive(false);

        base.Initialize();
    }

    public async override void Open()
    {
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync();
        base.Open();
    }

    private void OpenFigures()
    {
        backButton.gameObject.SetActive(true);
        HideAllButtons();
        PanelManager.GetSingleton("figures").Open();
    }


    private void OpenEvents()
    {
        backButton.gameObject.SetActive(true);
        HideAllButtons();
        PanelManager.GetSingleton("events").Open();
    }

    private void OpenPractices()
    {
        backButton.gameObject.SetActive(true);
        HideAllButtons();
        PanelManager.GetSingleton("practices").Open();
    }

    private void OpenMythology()
    {
        backButton.gameObject.SetActive(true);
        HideAllButtons();
        PanelManager.GetSingleton("mythology").Open();
    }

    private void ReturnToGame()
    {
        HidePanels();
        PanelManager.GetSingleton("encyclopedia").Close();
        ShowAllButtons();
    }

    private void ShowAllButtons()
    {
        figuresButton.gameObject.SetActive(true);
        eventsButton.gameObject.SetActive(true);
        practicesButton.gameObject.SetActive(true);
        mythologyButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        HidePanels();
    }

    private void HideAllButtons()
    {
        figuresButton.gameObject.SetActive(false);
        eventsButton.gameObject.SetActive(false);
        practicesButton.gameObject.SetActive(false);
        mythologyButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
    }

    private void HidePanels()
    {
        PanelManager.GetSingleton("figures").Close();
        PanelManager.GetSingleton("practices").Close();
        PanelManager.GetSingleton("events").Close();
        PanelManager.GetSingleton("mythology").Close();
        backButton.gameObject.SetActive(false);
    }
}