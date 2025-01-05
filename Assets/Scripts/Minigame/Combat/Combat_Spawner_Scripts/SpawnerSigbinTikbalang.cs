using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerSigbinTikbalang : Spawner
{
    [SerializeField] protected int destroyThreshold;
    [SerializeField] protected float spawnInterval;
    protected int spawn1DestroyedCount = 0;
    protected int spawn2DestroyedCount = 0;
    private BattleManagerSigbinTikbalang battleManager;

    protected override void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    public int GetDestroyThreshold()
    {
        return destroyThreshold;
    }

    public override void OnButtonClicked(GameObject spawnButton)
    {
        base.OnButtonClicked(spawnButton);
        battleManager = BattleManager.Singleton as BattleManagerSigbinTikbalang;

        if (spawnButton.name == "SpawnType1")
        {
            spawn1DestroyedCount++;
            int currentCount = destroyThreshold - spawn1DestroyedCount;
            if (currentCount >= 0)
            {
                battleManager.UpdateSigbinCount(currentCount);
            }
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawn2DestroyedCount++;
            int currentCount = destroyThreshold - spawn2DestroyedCount;
            if (currentCount >= 0)
            {
                battleManager.UpdateTikbalangCount(currentCount);
            }
        }

        base.OnButtonClicked(spawnButton);

        if (spawn1DestroyedCount >= destroyThreshold && spawn2DestroyedCount >= destroyThreshold)
        {
            battleManager.Defeated();
        }
    }

    protected void BaseOnButtonClicked(GameObject spawnButton)
    {
        base.OnButtonClicked(spawnButton);
    }
}
