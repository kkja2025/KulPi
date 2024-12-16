using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneMenu : Panel
{
    [SerializeField] private GameObject[] cutscenePanels;

    private int currentCutsceneIndex = 0;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        UpdateCutscene();
        base.Initialize();
    }

    private void UpdateCutscene()
    {
        foreach (GameObject panel in cutscenePanels)
        {
            panel.SetActive(false);
        }

        if (currentCutsceneIndex >= 0 && currentCutsceneIndex < cutscenePanels.Length)
        {
            cutscenePanels[currentCutsceneIndex].SetActive(true);
        }
    }

    public void NextCutscene()
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
        PanelManager.LoadSceneAsync("Chapter1");
    }
}
