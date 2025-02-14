using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EncyclopediaItemList
{
    public List<EncyclopediaItem> items = null;
}

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

    public static EncyclopediaItem Sample()
    {
        string id = "";
        string chapter = "";
        string title = "";
        string description = "";
        string category = "encyclopedia_sample";

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Figures
    public static EncyclopediaItem Figures_Chapter1_Babaylan()
    {
        string id = "Babaylan";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Babaylan";
        string description = "The Babaylan were powerful women (or sometimes men) in ancient Filipino communities. They were spiritual leaders who communicated with the spirits, healed the sick, and performed rituals to keep balance between the people and the spirit world. They also played a role in important ceremonies, like marriages and harvest celebrations. Today, some indigenous communities still have Babaylans. Source: National Commission for Culture and the Arts (NCCA)";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
    public static EncyclopediaItem Figures_Chapter1_Albularyo()
    {
        string id = "Albularyo";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Albularyo";
        string description = "The Albularyo is a traditional Filipino healer, known for using herbal medicine, massage (called hilot), and spiritual healing. They often treat illnesses believed to be caused by supernatural beings, like bad spirits. People still visit albularyos, especially in rural areas, when they feel sick or need spiritual guidance. Source: Philippine Institute of Traditional and Alternative Health Care (PITAHC)";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Figures_Chapter2_Datu()
    {
        string id = "Datu";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Datu";
        string description = "A Datu was a leader in pre-colonial Philippine societies, governing a barangay or village. Datus held both administrative and judicial roles and were integral to societal structures before Spanish colonization. Source: National Historical Commission of the Philippines";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Figures_Chapter2_LapuLapu()
    {
        string id = "LapuLapu";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Lapu Lapu";
        string description = "Lapu-Lapu was a chieftain of Mactan Island who resisted Spanish colonization. In 1521, his warriors defeated Ferdinand Magellan in the Battle of Mactan. Lapu-Lapu is celebrated as the first Filipino hero. Source: National Historical Commission of the Philippines";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_FIGURES_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Events or Items
    public static EncyclopediaItem Events_Chapter1_Lagundi()
    {
        string id = "Lagundi";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Lagundi";
        string description = "Lagundi (Vitex negundo) is a plant used in traditional Filipino medicine, especially for treating coughs, colds, and asthma. Many families in the Philippines keep lagundi leaves at home for making herbal tea when someone is sick. Source: Department of Health, (DOH) Philippines";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
    public static EncyclopediaItem Events_Chapter1_Sambong()
    {
        string id = "Sambong";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Sambong";
        string description = "Sambong (Blumea balsamifera) is another plant used in Filipino herbal medicine. It’s often used to treat kidney problems and urinary infections. People usually boil the leaves to make tea, which is believed to help flush out toxins from the body. Source: Philippine Institute of Traditional and Alternative Health Care (PITAHC)";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Events_Chapter1_NiyogNiyogan()
    {
        string id = "NiyogNiyogan";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Niyog-Niyogan";
        string description = "Niyog-niyogan (Quisqualis indica) is a plant used to get rid of intestinal worms. In the Philippines, kids are often given a few seeds of niyog-niyogan to chew when they have stomach issues caused by worms. Source: University of the Philippines, College of Pharmacy";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Events_Chapter2_Bangus()
    {
        string id = "Bangus";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Bangus (Milkfish)";
        string description = "Bangus, or milkfish (Chanos chanos), is the national fish of the Philippines and a significant species in aquaculture. It thrives in brackish water and is a key export product. Its tender flesh is versatile in Filipino cuisine, commonly prepared grilled, fried, or in sour soups. Source: Philippine Department of Agriculture - Bureau of Fisheries and Aquatic Resources (BFAR)";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Events_Chapter2_Bisugo()
    {
        string id = "Bisugo";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Bisugo (Threadfin Bream)";
        string description = "Bisugo is a common fish in Philippine waters, known for its pinkish hue and delicate flavor. It is popular in dishes such as 'pesang bisugo,' a soup made with ginger and vegetables. Source: Philippine Department of Agriculture - BFAR";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem Events_Chapter2_Apahap()
    {
        string id = "Apahap";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Apahap (Sea Bass)";
        string description = "Apahap, or Asian sea bass (Lates calcarifer), is a commercially valuable species farmed for its mild, firm meat. It is often prepared grilled or steamed in the Philippines and is a key species in aquaculture initiatives. Source: Philippine Department of Agriculture - BFAR";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_EVENTS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Practices and Traditions
    public static EncyclopediaItem PracticesAndTraditions_Chapter2_RhythmsOfUnity()
    {
        string id = "RhythmsOfUnity";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Ceremonial Drums";
        string description = "In the past, Filipinos used drums to bring people together during ceremonies or to call for courage in tough times. The beats of these drums connected them to nature and their ancestors. Instruments like the kulintang and tambol are still used today, keeping these traditions alive. Source: National Commission for Culture and the Arts (NCCA)";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    public static EncyclopediaItem PracticesAndTraditions_Chapter2_Boat()
    {
        string id = "Boat";
        string chapter = "Chapter 2: 'Betrayal at Mactan: The Fallen Hero'";
        string title = "Navigating with Bancas";
        string description = "Boats in the Philippines serve vital roles in transportation, fishing, and cultural practices. Traditional outrigger boats, or bancas, are integral to coastal communities. Modern advancements include motorized and eco-friendly boats for sustainability. Source: Maritime Industry Authority of the Philippines";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_PRACTICES_AND_TRADITIONS_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }

    // Mythology and Folklore
    public static EncyclopediaItem MythologyAndFolklore_Chapter1_Sigbin()
    {
        string id = "Sigbin";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Sigbin";
        string description = "The Sigbin is a creature from Filipino folklore. It’s said to come out at night, walking backwards with its head between its hind legs, and sucking the blood of its victims. Some stories say it can make itself invisible. In some regions, it’s believed that the Sigbin is controlled by powerful witches. Source: The Aswang Project";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
    public static EncyclopediaItem MythologyAndFolklore_Chapter1_Tikbalang()
    {
        string id = "Tikbalang";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Tikbalang";
        string description = "A Tikbalang is a mythical creature that has the head of a horse and the body of a human. It is said to roam the mountains and forests, playing tricks on travelers by confusing them and leading them in circles. However, if you manage to tame a tikbalang, it will become your protector. Source: Philippine Folk Literature by Damiana Eugenio";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
    public static EncyclopediaItem MythologyAndFolklore_Chapter1_Diwata()
    {
        string id = "Diwata";
        string chapter = "Chapter 1: 'The Tainted Paradise'";
        string title = "Diwata";
        string description = "A Diwata is a nature spirit or goddess in Filipino mythology. They are often beautiful, magical beings who protect forests, rivers, and mountains. Diwatas are kind to those who respect nature, but they can become angry and cause misfortune if their land is harmed. Source: The Aswang Project";
        string category = CLOUD_SAVE_ENCYCLOPEDIA_MYTHOLOGY_AND_FOLKLORE_KEY;

        return new EncyclopediaItem(id, chapter, title, description, category);
    }
}