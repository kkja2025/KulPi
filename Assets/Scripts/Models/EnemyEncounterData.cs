using System;
using UnityEngine;

[System.Serializable]
public class EnemyEncounterData
{
    private string enemyID;
    private Vector3 position;
    private Vector3 playerPosition;
    private string sceneName;


    public EnemyEncounterData(string enemyID, Vector3 position, Vector3 playerPosition, string sceneName)
    {
        this.enemyID = enemyID;
        this.position = position;
        this.playerPosition = playerPosition;
        this.sceneName = sceneName;
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

    public string GetSceneName()
    {
        return sceneName;
    }
}