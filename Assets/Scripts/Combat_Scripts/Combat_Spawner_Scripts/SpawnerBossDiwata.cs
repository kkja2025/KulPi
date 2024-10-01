using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossDiwata : SpawnerSigbinTikbalang
{
    private BattleManagerDiwata battleManager;
    public override void OnButtonClicked(GameObject spawnButton)
    {
        BaseOnButtonClicked(spawnButton);
        battleManager = FindObjectOfType<BattleManagerDiwata>();

        if (spawnButton.name == "SpawnType1")
        {
            spawn1DestroyedCount++;
            battleManager.UpdateSigbinCount(spawn1DestroyedCount);
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawn2DestroyedCount++;
            battleManager.UpdateTikbalangCount(spawn2DestroyedCount);
        }

        if (spawn1DestroyedCount >= destroyThreshold && spawn2DestroyedCount >= destroyThreshold)
        {
            battleManager.ShowUltimateButton();
        }
    }

    public void ResetCounters()
    {
        spawn1DestroyedCount = 0;
        spawn2DestroyedCount = 0;
        battleManager.UpdateSigbinCount(0);
        battleManager.UpdateTikbalangCount(0);
    }
}
