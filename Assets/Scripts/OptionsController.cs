using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Button BackButton;
    void Start()
    {
        BackButton.onClick.AddListener(GoBack);
    }

    void GoBack()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
