using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDiwataMinionSpawner : MinionSpawner
{
    [SerializeField] private int minionsToDestroy;  
    private int minionsDestroyed = 0;

    public override void OnMinionButtonClicked(GameObject minionButton)
    {
        Destroy(minionButton);
        currentMinions.Remove(minionButton); 

        minionsDestroyed++;
        if (minionsDestroyed % minionsToDestroy == 0)
        {
            DiwataBattleManager.Singleton.ShowUltimateButton();
        }
    }
}
