using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigbinTikbalangMinionSpawner : MinionSpawner
{
    [SerializeField] private int destroyThreshold = 5;
    private int minion1DestroyedCount = 0;
    private int minion2DestroyedCount = 0;

    public override void OnMinionButtonClicked(GameObject minionButton)
    {
        Debug.Log("Minion button clicked: " + minionButton.name);
        Destroy(minionButton);
        currentMinions.Remove(minionButton);

        if (minionButton.name == "MinionType1")
        {
            minion1DestroyedCount++;
            Debug.Log("Minion 1 destroyed count: " + minion1DestroyedCount);
        }
        else if (minionButton.name == "MinionType2")
        {
            minion2DestroyedCount++;
            Debug.Log("Minion 2 destroyed count: " + minion2DestroyedCount);
        }

        base.OnMinionButtonClicked(minionButton);

        if (minion1DestroyedCount >= destroyThreshold && minion2DestroyedCount >= destroyThreshold)
        {
            SigbinTikbalangBattleManager.Singleton.Defeated();
        }
    }
}
