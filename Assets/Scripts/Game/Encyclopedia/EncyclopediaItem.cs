using System;
using UnityEngine;

[System.Serializable]
public class EncyclopediaItem
{
    public string itemChapter;
    public string itemTitle;
    public Sprite itemIcon;
    public string itemDescription;
    public string itemCategory;

    public const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = "encyclopedia_figures";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = "encyclopedia_events";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY = "encyclopedia_practices_and_traditions";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY = "encyclopedia_mythology_and_folklore";

    public EncyclopediaItem(string chapter, string title, Sprite icon, string description, string category)
    {
        itemChapter = chapter;
        itemTitle = title;
        itemIcon = icon;
        itemDescription = description;
        itemCategory = category;
    }

    public static EncyclopediaItem Figures_Chapter1_Diwata()
    {   
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Diwata";
        Sprite icon = Resources.Load<Sprite>("Icons/Encyclopedia/Diwata");
        string description = "     The Diwata are revered nature spirits in Filipino folklore, protectors of forests, mountains, and bodies of water. In their pure form, they embody the beauty and serenity of the natural world. However, the Diwata has been corrupted by the greed of the Sakim, turning her into a vengeful, serpent-like creature. Once a benevolent guardian of Sugbu’s land, her pain and transformation into this monstrous form serve as a reminder of the consequences of environmental destruction."; 
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(chapter, title, icon, description, category);
    }
}