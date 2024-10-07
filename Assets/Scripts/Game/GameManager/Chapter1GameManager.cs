using System;
using System.Threading.Tasks;
using UnityEngine;

public class Chapter1GameManager : GameManager
{
    private EnemyEncounterData activeEnemy = null;
    private int count = 0;

    public void SetActiveEnemy(EnemyEncounterData enemy)
    {
        activeEnemy = enemy;
    }

    public EnemyEncounterData GetActiveEnemy()
    {
        return activeEnemy;
    }

    public override async void SetObjective(string objective)
    {
        playerData.SetActiveQuest(objective);
        objectiveText.text = objective;
        await SavePlayerData();
    }

    public string GetObjective()
    {
        return playerData.GetActiveQuest();
    }

    public int GetCount()
    {
        return count;
    }

    public void SetCount(int newCount)
    {
        count = newCount;
    }

    public void IncrementCount()
    {
        count++;
    }

    protected override EncyclopediaItem GetEncyclopediaItemById(string id)
    {
        switch (id)
        {
            case "Babaylan":
                return EncyclopediaItem.Figures_Chapter1_Babaylan();
            case "Albularyo":
                return EncyclopediaItem.Figures_Chapter1_Albularyo();
            case "Lagundi":
                return EncyclopediaItem.PracticesAndTraditions_Chapter1_Lagundi();
            case "Sambong":
                return EncyclopediaItem.PracticesAndTraditions_Chapter1_Sambong();
            case "NiyogNiyogan":
                return EncyclopediaItem.PracticesAndTraditions_Chapter1_NiyogNiyogan();
            case "Tikbalang":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Tikbalang();
            case "Sigbin":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Sigbin();
            case "Diwata":
                return EncyclopediaItem.MythologyAndFolklore_Chapter1_Diwata();
            default:
                Debug.LogWarning("No encyclopedia entry found for the provided ID.");
                return null;
        }
    }
}