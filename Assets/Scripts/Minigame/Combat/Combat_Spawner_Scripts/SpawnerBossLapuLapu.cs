using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossLapuLapu : Spawner
{
    [SerializeField] protected int destroyThreshold = 5;
    protected int spawnDestroyedCount = 0;
    private BattleManagerLapuLapu bossBattleManager;

    
    public override void OnButtonClicked(GameObject spawnButton)
    {
        base.OnButtonClicked(spawnButton);
        bossBattleManager = BattleManager.Singleton as BattleManagerLapuLapu;

        if (spawnButton.name == "SpawnType1")
        {
            bossBattleManager.AddTime(1);
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawnDestroyedCount++;
            bossBattleManager.UpdateEnemyCount(spawnDestroyedCount);
        }

        if (spawnDestroyedCount >= destroyThreshold)
        {
            bossBattleManager.ShowUltimateButton();
        }
    }

    public void ResetCounters()
    {
        bossBattleManager = BattleManager.Singleton as BattleManagerLapuLapu;
        spawnDestroyedCount = 0;
        bossBattleManager.UpdateEnemyCount(0);
    }
}
