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
        backButton.onClick.AddListener(ReturnToGame);

        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private async void OpenFigures()
    {
        PanelManager.GetSingleton("events").Close();
        PanelManager.GetSingleton("practices").Close();
        PanelManager.GetSingleton("mythology").Close();
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY);
        PanelManager.GetSingleton("figures").Open();
    }


    private async void OpenEvents()
    {
        PanelManager.GetSingleton("figures").Close();
        PanelManager.GetSingleton("practices").Close();
        PanelManager.GetSingleton("mythology").Close();
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY);
        PanelManager.GetSingleton("events").Open();
    }

    private async void OpenPractices()
    {
        PanelManager.GetSingleton("figures").Close();
        PanelManager.GetSingleton("mythology").Close();
        PanelManager.GetSingleton("events").Close();
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY);
        PanelManager.GetSingleton("practices").Open();
    }

    private async void OpenMythology()
    {
        PanelManager.GetSingleton("figures").Close();
        PanelManager.GetSingleton("practices").Close();
        PanelManager.GetSingleton("events").Close();
        await EncyclopediaManager.Singleton.LoadEncyclopediaEntriesAsync(EncyclopediaItem.CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY);
        PanelManager.GetSingleton("mythology").Open();
    }

    private void ReturnToGame()
    {
        PanelManager.GetSingleton("figures").Close();
        PanelManager.GetSingleton("practices").Close();
        PanelManager.GetSingleton("events").Close();
        PanelManager.GetSingleton("mythology").Close();
        PanelManager.GetSingleton("encyclopedia").Close();
    }
}
