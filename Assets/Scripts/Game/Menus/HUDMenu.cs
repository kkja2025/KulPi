using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HUDMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button encyclopediaButton = null;
    [SerializeField] private Button interactButton = null;

    public override void Initialize()
    {
        if (IsInitialized)
        {
            return;
        }
        pauseButton.onClick.AddListener(OpenPause);
        inventoryButton.onClick.AddListener(OpenInventory);
        encyclopediaButton.onClick.AddListener(OpenEncyclopedia);
        base.Initialize();
    }

    public override void Open()
    {
        base.Open();
        interactButton.gameObject.SetActive(false);
    }

    private void OpenInventory()
    {
        PanelManager.GetSingleton("inventory").Open();
    }

    private void OpenEncyclopedia()
    {
        PanelManager.GetSingleton("encyclopedia").Open();    
    }

    private void OpenPause()
    {
        Time.timeScale = 0;
        PanelManager.GetSingleton("pause").Open();
    }

}