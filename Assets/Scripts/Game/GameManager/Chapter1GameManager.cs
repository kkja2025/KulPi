using System;
using UnityEngine;

public class Chapter1GameManager : GameManager
{
    private EnemyEncounterData activeEnemy = null;

    public void SetActiveEnemy(EnemyEncounterData enemy)
    {
        activeEnemy = enemy;
    }

    public EnemyEncounterData GetActiveEnemy()
    {
        return activeEnemy;
    }
}