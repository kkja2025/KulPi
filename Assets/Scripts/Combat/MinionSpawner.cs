using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject minionButtonPrefab = null;     
    [SerializeField] private RectTransform minionParent = null;
    [SerializeField] private int minionsToDestroy;
    [SerializeField] private int minionsPerWave;
    [SerializeField] private float spawnInterval;    
    private List<GameObject> currentMinions = new List<GameObject>();

    private int minionsDestroyed = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnMinions), 0f, spawnInterval);
    }

    void SpawnMinions()
    {
        
        DespawnPreviousMinions();

        float minSpacing = 100f;

        List<Vector2> spawnedPositions = new List<Vector2>();

        RectTransform parentRect = minionParent.GetComponent<RectTransform>();
        Vector2 parentSize = parentRect.rect.size;

        float minX = -parentSize.x / 2;
        float maxX = parentSize.x / 2;
        float minY = -parentSize.y / 2; 
        float maxY = parentSize.y / 2;

        for (int i = 0; i < minionsPerWave; i++)
        {
            Vector2 randomPosition;
            bool validPosition;

            do
            {
                randomPosition = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
                );
                validPosition = true;

                foreach (Vector2 pos in spawnedPositions)
                {
                    if (Vector2.Distance(randomPosition, pos) < minSpacing)
                    {
                        validPosition = false;
                        break;
                    }
                }

            } while (!validPosition); 

            GameObject minionButton = Instantiate(minionButtonPrefab, minionParent);
            minionButton.GetComponent<RectTransform>().anchoredPosition = randomPosition;

            Button buttonComponent = minionButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnMinionButtonClicked(minionButton));

            spawnedPositions.Add(randomPosition);
            currentMinions.Add(minionButton);
        }
    }

    void DespawnPreviousMinions()
    {
        foreach (GameObject minion in currentMinions)
        {
            Destroy(minion);
        }
        currentMinions.Clear();
    }

    void OnMinionButtonClicked(GameObject minionButton)
    {
        Destroy(minionButton);
        currentMinions.Remove(minionButton); 

        minionsDestroyed++;
        if (minionsDestroyed % minionsToDestroy == 0)
        {
            BattleManager.Singleton.ShowUltimateButton();
        }
    }
}
