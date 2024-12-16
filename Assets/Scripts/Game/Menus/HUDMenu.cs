using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    // [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button encyclopediaButton = null;
    [SerializeField] private Button questButton = null;
    [SerializeField] private GameObject questTextObject;
    private bool isQuestTextVisible = false;


    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        isQuestTextVisible = questTextObject.activeSelf;
        pauseButton.onClick.AddListener(OpenPause);
        // inventoryButton.onClick.AddListener(OpenInventory);
        encyclopediaButton.onClick.AddListener(OpenEncyclopedia);
        questButton.onClick.AddListener(ShowQuest);
        base.Initialize();
    }

    // private void OpenInventory()
    // {
    //     PanelManager.GetSingleton("inventory").Open();
    // }

    private void OpenEncyclopedia()
    {
        PanelManager.GetSingleton("encyclopedia").Open();    
    }

    private void ShowQuest()
    {
        isQuestTextVisible = !isQuestTextVisible;
        questTextObject.SetActive(isQuestTextVisible);
    }

    private void OpenPause()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("pause").Open();
    }
}