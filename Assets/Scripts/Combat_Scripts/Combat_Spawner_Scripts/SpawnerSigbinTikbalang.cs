using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerSigbinTikbalang : Spawner
{
    [SerializeField] protected int destroyThreshold = 5;
    protected int spawn1DestroyedCount = 0;
    protected int spawn2DestroyedCount = 0;

    public override void OnButtonClicked(GameObject spawnButton)
    {
        AudioManager.Singleton.PlaySwordSoundEffect(clickCount);
        clickCount++;
        Destroy(spawnButton);
        currentSpawns.Remove(spawnButton);

        if (spawnButton.name == "SpawnType1")
        {
            spawn1DestroyedCount++;
            BattleManagerSigbinTikbalang.Singleton.UpdateSigbinCount(spawn1DestroyedCount);
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawn2DestroyedCount++;
            BattleManagerSigbinTikbalang.Singleton.UpdateTikbalangCount(spawn2DestroyedCount);
        }

        base.OnButtonClicked(spawnButton);

        if (spawn1DestroyedCount >= destroyThreshold && spawn2DestroyedCount >= destroyThreshold)
        {
            BattleManagerSigbinTikbalang.Singleton.Defeated();
        }
    }
}
