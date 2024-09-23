using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < cutscenePanels.Length; i++)
        {
            cutscenePanels[i].SetActive(false);
        }

        if (cutscenePanels != null && cutscenePanels.Length > 0)
        {
            cutscenePanels[currentCutsceneIndex].SetActive(true);

            buttonNext.GetComponentInChildren<Text>().text = currentCutsceneIndex < cutscenePanels.Length - 1 ? "Next" : "Start Game";
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
        MainMenuManager.Singleton.LoadGame();
    }
}
