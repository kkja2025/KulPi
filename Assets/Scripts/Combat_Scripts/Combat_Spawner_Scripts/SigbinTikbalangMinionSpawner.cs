using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SigbinTikbalangMinionSpawner : MinionSpawner
{
    [SerializeField] protected int destroyThreshold = 5;
    protected int minion1DestroyedCount = 0;
    protected int minion2DestroyedCount = 0;

    public override void OnMinionButtonClicked(GameObject minionButton)
    {
        AudioManager.Singleton.PlaySwordSoundEffect(clickCount);
        clickCount++;
        Destroy(minionButton);
        currentMinions.Remove(minionButton);

        if (minionButton.name == "MinionType1")
        {
            minion1DestroyedCount++;
            SigbinTikbalangBattleManager.Singleton.UpdateSigbinCount(minion1DestroyedCount);
        }
        else if (minionButton.name == "MinionType2")
        {
            minion2DestroyedCount++;
            SigbinTikbalangBattleManager.Singleton.UpdateTikbalangCount(minion2DestroyedCount);
        }

        base.OnMinionButtonClicked(minionButton);

        if (minion1DestroyedCount >= destroyThreshold && minion2DestroyedCount >= destroyThreshold)
        {
            SigbinTikbalangBattleManager.Singleton.Defeated();
        }
    }
}
