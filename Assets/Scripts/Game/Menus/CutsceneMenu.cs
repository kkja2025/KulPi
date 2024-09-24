using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneMenu : Panel
{
    [SerializeField] private Button buttonNext = null;
    [SerializeField] private GameObject[] cutscenePanels = null;

    private int currentCutsceneIndex = 0;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        if (buttonNext == null)
        {
            Debug.LogError("buttonNext is not assigned in the Inspector.");
            return;
        }

        if (cutscenePanels == null || cutscenePanels.Length == 0)
        {
            Debug.LogError("cutscenePanels array is either null or empty.");
            return;
        }

        UpdateCutscene();
        buttonNext.onClick.AddListener(NextCutscene);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void UpdateCutscene()
    {
        if (cutscenePanels == null)
        {
            Debug.LogError("cutscenePanels is null.");
            return;
        }

        for (int i = 0; i < cutscenePanels.Length; i++)
        {
            cutscenePanels[i].SetActive(false);
        }

        if (cutscenePanels.Length > 0)
        {
            cutscenePanels[currentCutsceneIndex].SetActive(true);
        }
    }

    private void NextCutscene()
    {
        if (currentCutsceneIndex < cutscenePanels.Length - 1)
        {
            currentCutsceneIndex++;
            UpdateCutscene();
        }
        else
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("loading").Open();
        SceneManager.LoadScene("Chapter1");
    }
}
