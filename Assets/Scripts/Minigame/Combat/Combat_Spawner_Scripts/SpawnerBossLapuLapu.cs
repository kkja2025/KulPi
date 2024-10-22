using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossLapuLapu : Spawner
{
    [SerializeField] protected int destroyThreshold = 5;
    protected int spawnDestroyedCount = 0;
    [SerializeField] private RectTransform panelPrefab;
    [SerializeField] private float panelMoveSpeed = 200f;
    private BattleManagerLapuLapu bossBattleManager;

    protected override void Start()
    {
        Spawn();
    }

    protected override void Spawn()
    {
        RectTransform panelInstance = Instantiate(panelPrefab, spawnParent);

        bool spawnOnLeft = Random.value > 0.5f;

        float parentWidth = spawnParent.rect.width;

        panelInstance.anchoredPosition = spawnOnLeft 
            ? new Vector2(-parentWidth / 2, 0)  
            : new Vector2(parentWidth / 2, 0); 

        GridLayoutGroup gridLayout = panelInstance.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(125f, 125f);
        gridLayout.spacing = new Vector2(10f, 10f);
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        for (int i = 0; i < spawnPerWave; i++)
        {
            GameObject spawnButtonPrefab = Random.value > 0.5f ? spawnButtonPrefab1 : spawnButtonPrefab2;
            GameObject spawnButton = Instantiate(spawnButtonPrefab, panelInstance);
            spawnButton.name = spawnButtonPrefab == spawnButtonPrefab1 ? "SpawnType1" : "SpawnType2";

            Button buttonComponent = spawnButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClicked(spawnButton));

            currentSpawns.Add(spawnButton);
        }

        float targetXPosition = spawnOnLeft 
            ? parentWidth / 2  
            : -parentWidth - parentWidth / 2; 

        StartCoroutine(MovePanel(panelInstance, targetXPosition));
    }

    private IEnumerator MovePanel(RectTransform panelInstance, float targetXPosition)
    {
        Vector2 targetPosition = new Vector2(targetXPosition, panelInstance.anchoredPosition.y);

        while (Mathf.Abs(panelInstance.anchoredPosition.x - targetXPosition) > 0.1f)
        {
            panelInstance.anchoredPosition = Vector2.MoveTowards(panelInstance.anchoredPosition, targetPosition, panelMoveSpeed * Time.deltaTime);

            yield return null;
        }

        Destroy(panelInstance.gameObject);
        Spawn();
    }

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
