using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool initialized = false;
    private static GameManager singleton = null;

    public static GameManager Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton != null)
                {
                    singleton.Initialize();
                }
                else
                {
                    Debug.LogError("GameManager not found in the scene!");
                }
            }
            return singleton;
        }
    }

    private void Initialize()
    {
        if (initialized) return;
        initialized = true;
    }

    private void Awake()
    {
        Application.runInBackground = true;
        StartClientService();
    }

    private void StartClientService()
    {
        PanelManager.CloseAll();
        PanelManager.GetSingleton("hud").Open();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public async void UnlockEncyclopediaItem(string id)
    {
        EncyclopediaItem item = null;
        switch (id)
        {
            case "Diwata":
                item = EncyclopediaItem.Figures_Chapter1_Diwata();
                break;
            case "SacredGrove":
                item = EncyclopediaItem.Events_Chapter1_Sacred_Grove();
                break;
            case "CursedLandOfSugbu":
                item = EncyclopediaItem.Events_Chapter1_Cursed_Land_of_Sugbu();
                break;
            case "TraditionalFilipinoMedicine":
                item = EncyclopediaItem.PracticesAndTraditions_Chapter1_Traditional_Filipino_Medicine();
                break;
            case "PowersAndFilipinoSpirituality":
                item = EncyclopediaItem.PracticesAndTraditions_Chapter1_Powers_and_Filipino_Spirituality();
                break;
            case "Tikbalang":
                item = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Tikbalang();
                break;
            case "Sigbin":
                item = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Sigbin();
                break;
            default:
                Debug.LogWarning("Invalid encyclopedia id provided.");
                return;
        }
        await EncyclopediaManager.Singleton.AddItem(item);
        PanelManager.GetSingleton("unlock").Open();
    }
}
