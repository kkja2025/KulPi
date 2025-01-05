using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossDiwata : SpawnerSigbinTikbalang
{
    private BattleManagerDiwata bossBattleManager;
    public override void OnButtonClicked(GameObject spawnButton)
    {
        BaseOnButtonClicked(spawnButton);
        bossBattleManager = BattleManager.Singleton as BattleManagerDiwata;

        if (spawnButton.name == "SpawnType1")
        {
            spawn1DestroyedCount++;
            int currentCount = destroyThreshold - spawn1DestroyedCount;
            if (currentCount >= 0)
            {
                bossBattleManager.UpdateSigbinCount(currentCount);
            }
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawn2DestroyedCount++;
            int currentCount = destroyThreshold - spawn2DestroyedCount;
            if (currentCount >= 0)
            {
                bossBattleManager.UpdateTikbalangCount(currentCount);
            }
        }

        if (spawn1DestroyedCount >= destroyThreshold && spawn2DestroyedCount >= destroyThreshold)
        {
            bossBattleManager.ShowUltimateButton();
        }
    }

    public void ResetCounters()
    {
        spawn1DestroyedCount = 0;
        spawn2DestroyedCount = 0;
        bossBattleManager.UpdateSigbinCount(destroyThreshold);
        bossBattleManager.UpdateTikbalangCount(destroyThreshold);
    }
}
