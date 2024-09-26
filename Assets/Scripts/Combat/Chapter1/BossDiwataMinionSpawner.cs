using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossDiwataMinionSpawner : SigbinTikbalangMinionSpawner
{
    public override void OnMinionButtonClicked(GameObject minionButton)
    {
        AudioManager.Singleton.PlaySwordSoundEffect(clickCount);
        clickCount++;
        Destroy(minionButton);
        currentMinions.Remove(minionButton);

        if (minionButton.name == "MinionType1")
        {
            minion1DestroyedCount++;
            DiwataBattleManager.Singleton.UpdateSigbinCount(minion1DestroyedCount);
        }
        else if (minionButton.name == "MinionType2")
        {
            minion2DestroyedCount++;
            DiwataBattleManager.Singleton.UpdateTikbalangCount(minion2DestroyedCount);
        }

        if (minion1DestroyedCount >= destroyThreshold && minion2DestroyedCount >= destroyThreshold)
        {
            DiwataBattleManager.Singleton.ShowUltimateButton();
        }
    }

    public void ResetCounters()
    {
        minion1DestroyedCount = 0;
        minion2DestroyedCount = 0;
        DiwataBattleManager.Singleton.UpdateSigbinCount(0);
        DiwataBattleManager.Singleton.UpdateTikbalangCount(0);
    }
}
