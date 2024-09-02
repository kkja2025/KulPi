using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button settingsButton;
    
    void Start()
    {
        settingsPanel.SetActive(false);
        settingsButton.onClick.AddListener(OpenSettings);
    }

    void OpenSettings()
    {
        settingsPanel.SetActive(true);
        settingsButton.gameObject.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        settingsButton.gameObject.SetActive(true);
    }
}
