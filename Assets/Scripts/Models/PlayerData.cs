using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public string playerID;
    public float x;
    public float y;
    public float z;

    public PlayerData(int level, string playerID, Vector3 position)
    {
        this.level = level;
        this.playerID = playerID;
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }
    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}