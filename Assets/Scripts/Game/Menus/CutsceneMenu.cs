using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneMenu : Panel
{
    [SerializeField] private Button prevButton1 = null;
    [SerializeField] private Button nextButton1 = null;
    [SerializeField] private Button prevButton2 = null;
    [SerializeField] private Button nextButton2 = null;
    [SerializeField] private Button prevButton3 = null;
    [SerializeField] private Button nextButton3 = null;
    [SerializeField] private Button prevButton4 = null;
    [SerializeField] private Button nextButton4 = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }

        prevButton1.onClick.AddListener(Main);
        nextButton1.onClick.AddListener(Scene2);
        prevButton2.onClick.AddListener(Scene1);
        nextButton2.onClick.AddListener(Scene3);
        prevButton3.onClick.AddListener(Scene2);
        nextButton3.onClick.AddListener(Scene4);
        prevButton4.onClick.AddListener(Scene3);
        nextButton4.onClick.AddListener(StartGame);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
    }

    private void Main()
    {
        PanelManager.GetSingleton("main").Open();
        PanelManager.GetSingleton("scene1").Close();
    }

    private void Scene1()
    {
        PanelManager.GetSingleton("main").Close();
        PanelManager.GetSingleton("scene2").Close();
        PanelManager.GetSingleton("scene1").Open();
    }

    private void Scene2()
    {
        PanelManager.GetSingleton("scene1").Close();
        PanelManager.GetSingleton("scene2").Open();
    }

    private void Scene3()
    {
        PanelManager.GetSingleton("scene2").Close();
        PanelManager.GetSingleton("scene3").Open();
    }

    private void Scene4()
    {
        PanelManager.GetSingleton("scene3").Close();
        PanelManager.GetSingleton("scene4").Open();
    }

    private void StartGame()
    {
        MainMenuManager.Singleton.LoadGame();
    }
}
