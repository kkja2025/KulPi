using System;
using System.Threading.Tasks;
using UnityEngine;

public class Chapter1GameManager : GameManager
{
    private EnemyEncounterData activeEnemy = null;
    private string currentObjective = "";
    private int count = 0;
    private bool isObjectiveComplete = false;

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
        currentObjective = objective;
        playerData.SetActiveQuest(currentObjective);
        isObjectiveComplete = false;
        UpdateObjectiveText();
        await SavePlayerData();
    }

    public bool IsObjectiveComplete()
    {
        return isObjectiveComplete;
    }

    public void CompleteObjective()
    {
        isObjectiveComplete = true;
        UpdateObjectiveText();
    }

    private void UpdateObjectiveText()
    {
        if (!isObjectiveComplete)
        {
            objectiveText.text = currentObjective;
        }
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