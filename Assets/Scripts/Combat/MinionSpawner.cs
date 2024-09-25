using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinionSpawner : MonoBehaviour
{
    [SerializeField] protected GameObject minionButtonPrefab1 = null;     
    [SerializeField] protected GameObject minionButtonPrefab2 = null;
    [SerializeField] private RectTransform minionParent = null;
    [SerializeField] private int minionsPerWave;
    [SerializeField] private float spawnInterval;    
    protected List<GameObject> currentMinions = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(SpawnMinions), 0f, spawnInterval);
    }

    private void SpawnMinions()
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

            GameObject minionButtonPrefab = Random.value > 0.5f ? minionButtonPrefab1 : minionButtonPrefab2;
            
            GameObject minionButton = Instantiate(minionButtonPrefab, minionParent);
            if (minionButtonPrefab == minionButtonPrefab1)
            {
                minionButton.name = "MinionType1";
            }
            else
            {
                minionButton.name = "MinionType2";
            }

            minionButton.GetComponent<RectTransform>().anchoredPosition = randomPosition;

            Button buttonComponent = minionButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnMinionButtonClicked(minionButton));

            spawnedPositions.Add(randomPosition);
            currentMinions.Add(minionButton);
        }
    }

    private void DespawnPreviousMinions()
    {
        foreach (GameObject minion in currentMinions)
        {
            Destroy(minion);
        }
        currentMinions.Clear();
    }

    public virtual void OnMinionButtonClicked(GameObject minionButton)
    {
        Destroy(minionButton);
        currentMinions.Remove(minionButton); 
    }
}
