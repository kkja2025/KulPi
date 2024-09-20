using System;
using UnityEngine;

[System.Serializable]
public class EncyclopediaItem
{
    public string itemID;
    public string itemChapter;
    public string itemTitle;
    public Sprite itemIcon;
    public string itemDescription;
    public string itemCategory;

    public const string CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY = "encyclopedia_figures";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY = "encyclopedia_events";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY = "encyclopedia_practices_and_traditions";
    public const string CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY = "encyclopedia_mythology_and_folklore";

    public EncyclopediaItem(string id, string chapter, string title, string description, string category)
    {
        itemID = id;
        itemChapter = chapter;
        itemTitle = title;
        itemIcon = null;
        itemDescription = description;
        itemCategory = category;
    }

    public void SetSprite(Sprite sprite)
    {
        itemIcon = sprite;
    }   

    // Figures
    public static EncyclopediaItem Figures_Chapter1_Diwata()
    {
        string id = "Diwata";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Diwata";
        string description = "     The Diwata are revered nature spirits in Filipino folklore, protectors of forests, mountains, and bodies of water. In their pure form, they embody the beauty and serenity of the natural world. However, the Diwata has been corrupted by the greed of the Sakim, turning her into a vengeful, serpent-like creature. Once a benevolent guardian of Sugbu’s land, her pain and transformation into this monstrous form serve as a reminder of the consequences of environmental destruction.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Events
    public static EncyclopediaItem Events_Chapter1_Sacred_Grove()
    {
        string id = "SacredGrove";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Sacred Grove";
        string description = "     The devastation of Sugbu’s once-thriving environment, brought about by the actions of the Sakim, mirrors the real-world environmental degradation faced in many regions. The air is filled with toxins, the waters are polluted, and the Sacred Grove is blighted. This event highlights how the imbalance between humanity and nature can lead to catastrophic consequences, affecting not just the land but the very spirits tied to it.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Events_Chapter1_Cursed_Land_of_Sugbu()
    {
        string id = "CursedLand";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Cursed Land of Sugbu";
        string description = "     This marks the beginning of Lakan’s journey to restore balance to the island. The corruption of the land has caused suffering among the people, and the once-harmonious relationship between the villagers and nature has been severed. By purifying the Sacred Grove and confronting the Diwata, Lakan will restore Sugbu to its former state of peace and prosperity.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Practices and Traditions
    public static EncyclopediaItem PracticesAndTraditions_Chapter1_Traditional_Filipino_Medicine()
    {
        string id = "FilipinoMedicine";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Traditional Filipino Medicine";
        string description = "     Herbal medicine, an integral part of Filipino healing traditions, involves the use of plants and natural remedies. Healers like the Babaylan were often relied upon to treat both physical and spiritual ailments. The tradition where the power of nature and the deep knowledge of the land are essential to curing illnesses.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem PracticesAndTraditions_Chapter1_Powers_and_Filipino_Spirituality()
    {
        string id = "Powers";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Powers and Filipino Spirituality";
        string description = "     In Filipino spirituality, the elements—earth, water, fire, and wind—are sacred and essential to maintaining balance in the world. These elemental powers, granted to Lakan through the Bantayog ng Panahon, represent the ancient connection between humans and nature. These powers can be used to purify the land, cleanse water sources, and defeat corrupted creatures.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Mythology and Folklore
    public static EncyclopediaItem MythologyAndFolklore_Chapter1_Mythical_Creatures_Tikbalang()
    {
        string id = "Tikbalang";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Philippine Mythical Creatures: Tikbalang";
        string description = "     Tikbalang are horse-like creatures known to mislead travelers deep within the forests. Often depicted as tall, fearsome beings with the head of a horse and the body of a human, they are believed to create illusions to confuse and terrify humans. In the Sacred Grove, corrupted Tikbalang serve as guardians of the twisted path, attacking those who dare to venture too close to the heart of the Grove.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem MythologyAndFolklore_Chapter1_Mythical_Creatures_Sigbin()
    {
        string id = "Sigbin";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Philippine Mythical Creatures: Sigbin";
        string description = "     The Sigbin is a rare and terrifying creature in Filipino folklore. It walks backward, has large fangs, and lurks in the shadows, feeding on fear. These creatures are known to come out during the night and prey upon the weak and vulnerable. In the game, corrupted Sigbin roam the Blighted Grove, embodying the chaos and darkness that has overtaken the sacred land.";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
}