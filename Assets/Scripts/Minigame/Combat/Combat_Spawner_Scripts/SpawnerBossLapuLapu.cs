using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnerBossLapuLapu : Spawner
{
    [SerializeField] protected int destroyThreshold = 5;
    protected int spawnDestroyedCount = 0;
    [SerializeField] private RectTransform panelPrefab;
    [SerializeField] private float panelMoveSpeed = 200f;
    private int comboCount = 0; 
    private int maxComboCount = 5; 
    private float speedMultiplier = 1.0f; 
    private float maxSpeedMultiplier = 2.5f;

    private BattleManagerLapuLapu bossBattleManager;

    private float[] yPositions = { 200f, 0f, -200f };
    private bool[] yPositionsOccupied = new bool[3];

    protected override void Spawn()
    {
        int availableYIndex = GetAvailableYPosition();

        if (availableYIndex == -1)
        {
            return;
        }

        yPositionsOccupied[availableYIndex] = true;

        RectTransform panelInstance = Instantiate(panelPrefab, spawnParent);

        bool spawnOnLeft = Random.value > 0.5f;
        float parentWidth = spawnParent.rect.width;
        float selectedYPosition = yPositions[availableYIndex];

        panelInstance.anchoredPosition = spawnOnLeft 
            ? new Vector2(-parentWidth / 2, selectedYPosition)  
            : new Vector2(parentWidth / 2, selectedYPosition); 

        GridLayoutGroup gridLayout = panelInstance.GetComponent<GridLayoutGroup>();
        gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        gridLayout.constraintCount = 1;

        for (int i = 0; i < spawnPerWave; i++)
        {
            GameObject spawnButtonPrefab = Random.value > 0.5f ? spawnButtonPrefab1 : spawnButtonPrefab2;
            GameObject spawnButton = Instantiate(spawnButtonPrefab, panelInstance);
            spawnButton.name = spawnButtonPrefab == spawnButtonPrefab1 ? "SpawnType1" : "SpawnType2";

            Button buttonComponent = spawnButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClicked(spawnButton));
        }

        float targetXPosition = spawnOnLeft 
            ? parentWidth / 2  
            : -parentWidth - parentWidth / 2; 

        StartCoroutine(MovePanel(panelInstance, targetXPosition, availableYIndex));
    }

    private IEnumerator MovePanel(RectTransform panelInstance, float targetXPosition, int yIndex)
    {
        Vector2 targetPosition = new Vector2(targetXPosition, panelInstance.anchoredPosition.y);

        while (Mathf.Abs(panelInstance.anchoredPosition.x - targetXPosition) > 0.1f)
        {
            panelInstance.anchoredPosition = Vector2.MoveTowards(panelInstance.anchoredPosition, targetPosition, panelMoveSpeed * speedMultiplier * Time.deltaTime);
            yield return null;
        }

        Destroy(panelInstance.gameObject);

        yPositionsOccupied[yIndex] = false;

        Spawn();
    }

    private int GetAvailableYPosition()
    {
        List<int> availablePositions = new List<int>();

        for (int i = 0; i < yPositionsOccupied.Length; i++)
        {
            if (!yPositionsOccupied[i])
            {
                availablePositions.Add(i);
            }
        }

        if (availablePositions.Count > 0)
        {
            return availablePositions[Random.Range(0, availablePositions.Count)];
        }
        else
        {
            return -1;
        }
    }

    public override void OnButtonClicked(GameObject spawnButton)
    {
        base.OnButtonClicked(spawnButton);
        bossBattleManager = BattleManager.Singleton as BattleManagerLapuLapu;

        if (spawnButton.name == "SpawnType1")
        {
            bossBattleManager.AddTime(1);
            ResetCombo();
        }
        else if (spawnButton.name == "SpawnType2")
        {
            spawnDestroyedCount++;
            bossBattleManager.UpdateEnemyCount(spawnDestroyedCount);

            comboCount++;
            speedMultiplier = 1.0f + Mathf.Clamp(comboCount / (float)maxComboCount, 0.0f, maxSpeedMultiplier - 1.0f);
            if (comboCount >= maxComboCount)
            {
                speedMultiplier = maxSpeedMultiplier;
                Spawn();
            }
        }

        if (spawnDestroyedCount >= destroyThreshold)
        {
            bossBattleManager.ShowUltimateButton();
        }
    }

    private void ResetCombo()
    {
        comboCount = 0;
        speedMultiplier = 1.0f;
    }

    public void ResetCounters()
    {
        bossBattleManager = BattleManager.Singleton as BattleManagerLapuLapu;
        spawnDestroyedCount = 0;
        bossBattleManager.UpdateEnemyCount(0);
    }
}