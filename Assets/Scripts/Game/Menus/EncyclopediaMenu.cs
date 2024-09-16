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
        PanelManager.GetSingleton("encyclopediapanels").Close();
        PanelManager.GetSingleton("figures").Open();
    }

    private void OpenEvents()
    {
        PanelManager.GetSingleton("encyclopediapanels").Close();
        PanelManager.GetSingleton("events").Open();
    }

    private void OpenPractices()
    {
        PanelManager.GetSingleton("encyclopediapanels").Close();
        PanelManager.GetSingleton("practices").Open();
    }

    private void OpenMythology()
    {
        PanelManager.GetSingleton("encyclopediapanels").Close();
        PanelManager.GetSingleton("mythology").Open();
    }

    private void ReturnToGame()
    {
        PanelManager.GetSingleton("encyclopedia").Close();
    }


}
