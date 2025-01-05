using UnityEngine;

[System.Serializable]
public class EncounterData
{
    private string encounterID;
    private Vector3 position;
    private Vector3 playerPosition;
    private string sceneName;


    public EncounterData(string encounterID, Vector3 position, Vector3 playerPosition, string sceneName)
    {
        this.encounterID = encounterID;
        this.position = position;
        this.playerPosition = playerPosition;
        this.sceneName = sceneName;
    }

    public string GetEncounterID()
    {
        return encounterID;
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