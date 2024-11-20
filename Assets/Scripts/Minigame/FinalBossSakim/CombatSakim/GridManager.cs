using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int rows = 5;
    [SerializeField] private int columns = 8;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private GridPlayer player;
    private float spawnTimer;

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
        float[] weights = { 0.2f, 0.35f, 0.45f }; // Energy: , Damage: , Empty: 
        
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
        if (image != null)
        {
            if (image.color == Color.green) return TileType.Energy;
            else if (image.color == Color.red) return TileType.Damage;
            else if (image.color == Color.gray) return TileType.Empty;
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
                    image.color = Color.green;
                    break;
                case TileType.Damage:
                    image.color = Color.red;
                    break;
                case TileType.Empty:
                    image.color = Color.gray;
                    break;
            }
        }
    }
}
