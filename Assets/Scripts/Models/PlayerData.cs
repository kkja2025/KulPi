using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField] private string level;
    [SerializeField] private string playerID;
    [SerializeField] private string activeQuest;
    [SerializeField] public float x;
    [SerializeField] public float y;
    [SerializeField] public float z;

    public PlayerData(string level, Vector3 position)
    {
        this.level = level;
        this.playerID = "";
        this.activeQuest = "";
        this.x = position.x;
        this.y = position.y;
        this.z = position.z;
    }
    public string GetLevel()
    {
        return level;
    }

    public string GetPlayerID()
    {
        return playerID;
    }

    public string GetActiveQuest()
    {
        return activeQuest;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
    
    public void SetLevel(string level)
    {
        this.level = level;
    }

    public void SetPlayerID(string id)
    {
        playerID = id;
    }

    public void SetActiveQuest(string quest)
    {
        activeQuest = quest;
    }

    public void SetPosition(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
}
