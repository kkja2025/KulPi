using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerSigbinTikbalang : Spawner
{
    [SerializeField] protected int destroyThreshold = 5;
    [SerializeField] protected float spawnInterval;
    protected int spawn1DestroyedCount = 0;
    protected int spawn2DestroyedCount = 0;
    private BattleManagerSigbinTikbalang battleManager;

    protected override void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    public override void OnButtonClicked(GameObject spawnButton)
    {
        base.OnButtonClicked(spawnButton);
        battleManager = BattleManager.Singleton as BattleManagerSigbinTikbalang;

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
