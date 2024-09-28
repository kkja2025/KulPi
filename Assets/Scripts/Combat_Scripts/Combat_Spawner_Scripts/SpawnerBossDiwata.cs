using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossDiwata : SpawnerSigbinTikbalang
{
    public override void OnButtonClicked(GameObject spawnButton)
    {
        AudioManager.Singleton.PlaySwordSoundEffect(clickCount);
        clickCount++;
        Destroy(spawnButton);
        currentSpawns.Remove(spawnButton);

        if (spawnButton.name == "SpawnType1")
        {
            spawn1DestroyedCount++;
            BattleManagerDiwata.Singleton.UpdateSigbinCount(spawn1DestroyedCount);
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawn2DestroyedCount++;
            BattleManagerDiwata.Singleton.UpdateTikbalangCount(spawn2DestroyedCount);
        }

        if (spawn1DestroyedCount >= destroyThreshold && spawn2DestroyedCount >= destroyThreshold)
        {
            BattleManagerDiwata.Singleton.ShowUltimateButton();
        }
    }

    public void ResetCounters()
    {
        spawn1DestroyedCount = 0;
        spawn2DestroyedCount = 0;
        BattleManagerDiwata.Singleton.UpdateSigbinCount(0);
        BattleManagerDiwata.Singleton.UpdateTikbalangCount(0);
    }
}
