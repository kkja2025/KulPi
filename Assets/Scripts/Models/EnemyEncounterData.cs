using System;
using UnityEngine;

[System.Serializable]
public class EnemyEncounterData
{
    private string enemyID;
    private Vector3 position;
    private Vector3 playerPosition;


    public EnemyEncounterData(string enemyID, Vector3 position, Vector3 playerPosition)
    {
        this.enemyID = enemyID;
        this.position = position;
        this.playerPosition = playerPosition;
    }

    public string GetEnemyID()
    {
        return enemyID;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetPlayerPosition()
    {
        return playerPosition;
    }
}