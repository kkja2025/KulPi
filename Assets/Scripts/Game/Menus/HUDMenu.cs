using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDMenu : Panel
{
    [SerializeField] private Button pauseButton = null;
    [SerializeField] private Button inventoryButton = null;
    [SerializeField] private Button encyclopediaButton = null;

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
    }

    private async void OpenPause()
    {
        PanelManager.GetSingleton("pause").Open();

        EncyclopediaItem diwataItem = EncyclopediaItem.Figures_Chapter1_Diwata();
        await EncyclopediaManager.Singleton.AddItem(diwataItem);

        EncyclopediaItem sacredGrove = EncyclopediaItem.Events_Chapter1_Sacred_Grove();
        await EncyclopediaManager.Singleton.AddItem(sacredGrove);

        EncyclopediaItem cursedLandOfSugbu = EncyclopediaItem.Events_Chapter1_Cursed_Land_of_Sugbu();
        await EncyclopediaManager.Singleton.AddItem(cursedLandOfSugbu);

        EncyclopediaItem filipinoMedicine = EncyclopediaItem.PracticesAndTraditions_Chapter1_Traditional_Filipino_Medicine();
        await EncyclopediaManager.Singleton.AddItem(filipinoMedicine);

        EncyclopediaItem powersAndFilipinoSpirituality = EncyclopediaItem.PracticesAndTraditions_Chapter1_Powers_and_Filipino_Spirituality();
        await EncyclopediaManager.Singleton.AddItem(powersAndFilipinoSpirituality);

        EncyclopediaItem tikbalang = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Tikbalang();
        await EncyclopediaManager.Singleton.AddItem(tikbalang);

        EncyclopediaItem sigbin = EncyclopediaItem.MythologyAndFolklore_Chapter1_Mythical_Creatures_Sigbin();
        await EncyclopediaManager.Singleton.AddItem(sigbin);
    }

    private void OpenInventory()
    {
        PanelManager.GetSingleton("inventory").Open();        
        Sprite icon = Resources.Load<Sprite>("Icons/Encyclopedia/SacredGrove");
        InventoryManager.Singleton.AddItem("Sacred Grove", icon);
        // InventoryManager.Singleton.RemoveItem("Sacred Grove");
    }

    private void OpenEncyclopedia()
    {
        PanelManager.GetSingleton("encyclopedia").Open();    
    }
}