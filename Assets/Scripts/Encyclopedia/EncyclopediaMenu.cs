using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EncyclopediaMenu : MonoBehaviour
{
    public Button encyclopediaButton;
    public GameObject encyclopediaPanel;
    public Button figuresButton;
    public Button eventsButton;
    public Button practicesButton;
    public Button mythologyButton;
    public Button backButton;

    void Start()
    {
        encyclopediaPanel.SetActive(false);

        encyclopediaButton.onClick.AddListener(ShowEncyclopediaPanel);
        figuresButton.onClick.AddListener(OnFiguresButtonClicked);
        eventsButton.onClick.AddListener(OnEventsButtonClicked);
        practicesButton.onClick.AddListener(OnPracticesButtonClicked);
        mythologyButton.onClick.AddListener(OnMythologyButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void ShowEncyclopediaPanel()
    {
        encyclopediaPanel.SetActive(true);
        Debug.Log("Encyclopedia panel is now visible");
    }

    void OnFiguresButtonClicked()
    {
        Debug.Log("Figures button clicked");
    }

    void OnEventsButtonClicked()
    {
        Debug.Log("Events button clicked");
    }

    void OnPracticesButtonClicked()
    {
        Debug.Log("Practices & Traditions button clicked");
    }

    void OnMythologyButtonClicked()
    {
        Debug.Log("Mythology & Folklore button clicked");
    }

    void OnBackButtonClicked()
    {
        Debug.Log("Back button clicked, returning to the game");
        encyclopediaPanel.SetActive(false);
    }
}
