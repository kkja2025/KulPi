using System;
using System.Threading.Tasks;
using UnityEngine;

public class Chapter1GameManager : GameManager
{
    private EnemyEncounterData activeEnemy = null;
    private string currentObjective = "";
    private bool isObjectiveComplete = false;
    private int npcTalkCount = 0;
    private int totalNPCs = 4;

    public void SetActiveEnemy(EnemyEncounterData enemy)
    {
        activeEnemy = enemy;
    }

    public EnemyEncounterData GetActiveEnemy()
    {
        return activeEnemy;
    }

    public override void SetObjective(string objective)
    {
        currentObjective = objective;
        isObjectiveComplete = false;
        UpdateObjectiveText();
    }

     private void CompleteObjective()
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
        else
        {
            objectiveText.text = "Objective Complete!";
        }
    }

    public void TalkedToNPC()
    {
        npcTalkCount++;
        Debug.Log("Talked to NPC. Total NPCs talked to: " + npcTalkCount);
        if (npcTalkCount >= totalNPCs)
        {
            CompleteObjective();
        }
        UpdateObjectiveText();
    }



}