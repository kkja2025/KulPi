using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 8;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private GridPlayer player;
    [SerializeField] private Sprite energyTileSprite;
    [SerializeField] private Sprite damageTileSprite;
    private float spawnTimer;
    private int movementCount = 0;

    private void Start()
    {
        InitializeGrid();
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnRandomTiles();
            spawnTimer = spawnInterval;
            UpdateTileColorsBasedOnPosition();
            if (player != null)
            {
                player.CheckTileInteraction();
                SpeedUpSpawnRate();
            }
        }
    }

    private void InitializeGrid()
    {
        foreach (Transform child in gridContainer)
        {
            Destroy(child.gameObject);
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject tile = Instantiate(tilePrefab, gridContainer);

                Vector3 tilePosition = new Vector3(col * 50, row * -50, 0);
                tile.transform.localPosition = tilePosition;

                SetTileAppearance(tile, TileType.Empty);
            }
        }
    }

    private void SpeedUpSpawnRate()
    {
        movementCount++;
        if (player.GetMovementCount() == 0)
        {
            spawnInterval = 1f;
            movementCount = 0;
            return; 
        }
        if (movementCount >= 10)
        {
            spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f);
        }
    }

    private void SpawnRandomTiles()
    {
        for (int row = 0; row < rows; row++)
        {
            if (gridContainer.childCount > 0)
            {
                Transform lastTile = gridContainer.GetChild(row + (columns - 1) * rows);
                if (lastTile != null)
                {
                    Destroy(lastTile.gameObject);
                }
            }

            Vector3 tilePosition = new Vector3((columns - 1) * 50, row * -50, 0);
            GameObject tile = Instantiate(tilePrefab, gridContainer);
            tile.transform.localPosition = tilePosition;

            TileType randomType = GetRandomTileType();
            SetTileAppearance(tile, randomType);
        }
    }

    private void UpdateTileColorsBasedOnPosition()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns - 1; col++)
            {
                GameObject currentTile = gridContainer.GetChild(row + col * rows).gameObject;
                GameObject tileToRight = gridContainer.GetChild(row + (col + 1) * rows).gameObject;

                if (currentTile != null && tileToRight != null)
                {
                    Vector3 currentTilePos = currentTile.transform.localPosition;
                    Vector3 tileToRightPos = tileToRight.transform.localPosition;

                    if (currentTilePos.x < tileToRightPos.x)
                    {
                        TileType tileToRightType = GetTileType(tileToRight);
                        SetTileAppearance(currentTile, tileToRightType);
                    }
                }
            }
        }
    }

    private TileType GetRandomTileType()
    {
        float[] weights = { 0.7f, 0.1f, 0.2f };
        float totalWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        float[] cumulativeWeights = new float[weights.Length];
        float cumulativeSum = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            cumulativeSum += weights[i] / totalWeight;
            cumulativeWeights[i] = cumulativeSum;
        }

        float randomValue = UnityEngine.Random.value;

        for (int i = 0; i < cumulativeWeights.Length; i++)
        {
            if (randomValue <= cumulativeWeights[i])
            {
                return (TileType)i;
            }
        }
        return TileType.Empty;
    }

    private TileType GetTileType(GameObject tile)
    {
        Image image = tile.GetComponent<Image>();
        if (image != null && image.sprite != null)
        {
            if (image.sprite == energyTileSprite) return TileType.Energy;
            else if (image.sprite == damageTileSprite) return TileType.Damage;
            else if (image.color == new Color(1f, 1f, 1f, 0f)) return TileType.Empty;
        }
        return TileType.Empty;
    }

    private void SetTileAppearance(GameObject tile, TileType type)
    {
        Image image = tile.GetComponent<Image>();
        if (image != null)
        {
            switch (type)
            {
                case TileType.Energy:
                    image.sprite = energyTileSprite;
                    image.color = new Color(1f, 1f, 1f, 1f); 
                    break;

                case TileType.Damage:
                    image.sprite = damageTileSprite;
                    image.color = new Color(1f, 1f, 1f, 1f); 
                    break;

                case TileType.Empty:
                    image.sprite = null;
                    image.color = new Color(1f, 1f, 1f, 0f); 
                    break;
            }
        }
    }
}
