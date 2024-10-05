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

    public void IncrementCount()
    {
        count++;
    }
}