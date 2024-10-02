using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject spawnButtonPrefab1 = null;     
    [SerializeField] private GameObject spawnButtonPrefab2 = null;
    [SerializeField] private RectTransform spawnParent = null;
    [SerializeField] private int spawnPerWave;
    [SerializeField] private float spawnInterval;
    private int clickCount = 0;    
    protected List<GameObject> currentSpawns = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, spawnInterval);
    }

    private void Spawn()
    {
        
        DespawnPrevious();

        float minSpacing = 100f;

        List<Vector2> spawnedPositions = new List<Vector2>();

        RectTransform parentRect = spawnParent.GetComponent<RectTransform>();
        Vector2 parentSize = parentRect.rect.size;

        float minX = -parentSize.x / 2;
        float maxX = parentSize.x / 2;
        float minY = -parentSize.y / 2; 
        float maxY = parentSize.y / 2;

        for (int i = 0; i < spawnPerWave; i++)
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

            GameObject spawnButtonPrefab = Random.value > 0.5f ? spawnButtonPrefab1 : spawnButtonPrefab2;
            
            GameObject spawnButton = Instantiate(spawnButtonPrefab, spawnParent);
            if (spawnButtonPrefab == spawnButtonPrefab1)
            {
                spawnButton.name = "SpawnType1";
            }
            else
            {
                spawnButton.name = "SpawnType2";
            }

            spawnButton.GetComponent<RectTransform>().anchoredPosition = randomPosition;

            Button buttonComponent = spawnButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => OnButtonClicked(spawnButton));

            spawnedPositions.Add(randomPosition);
            currentSpawns.Add(spawnButton);
        }
    }

    private void DespawnPrevious()
    {
        foreach (GameObject spawn in currentSpawns)
        {
            Destroy(spawn);
        }
        currentSpawns.Clear();
    }

    public virtual void OnButtonClicked(GameObject spawnButton)
    {
        AudioManager.Singleton.PlaySwordSoundEffect(clickCount);
        clickCount++;
        SlashAnimation slashAnimation = spawnButton.GetComponent<SlashAnimation>();
        if (slashAnimation != null)
        {
            StartCoroutine(WaitForAnimationToFinish(slashAnimation, spawnButton));
        }
        else
        {
            Destroy(spawnButton);
            currentSpawns.Remove(spawnButton);
        }
    }

    private IEnumerator WaitForAnimationToFinish(SlashAnimation slashAnimation, GameObject spawnButton)
    {
        slashAnimation.StartOverlayAnimation();
        yield return new WaitForSeconds(slashAnimation.animationFrames.Length * slashAnimation.animationSpeed);
        Destroy(spawnButton);
        currentSpawns.Remove(spawnButton);
    }
}
