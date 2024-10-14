using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PicturePuzzleMenu : Panel
{
    [SerializeField] private Button showCompletePuzzleButton = null;
    [SerializeField] private Button closeButton = null;
    [SerializeField] private Button completePuzzleButton = null;
    [SerializeField] private RectTransform puzzleContent = null;
    [SerializeField] private GameObject puzzlePiecePrefab = null;
    [SerializeField] private GridLayoutGroup gridLayoutGroup = null;
    private GameObject firstSelectedPiece = null;
    private Sprite[] correctPuzzle = null;
    private PicturePuzzleItem puzzleData;
    public event Action OnPuzzleSolved;

    public override void Initialize()
    {
        base.Initialize();
        showCompletePuzzleButton.onClick.AddListener(ShowCompletePuzzle);
        completePuzzleButton.onClick.AddListener(CompletePuzzle);
        closeButton.onClick.AddListener(Close);
    }

    public void ShowPuzzle(PicturePuzzleItem data)
    {
        base.Open();
        puzzleData = data;
        correctPuzzle = data.GetPuzzleItems();
        LoadPuzzleItems(data.GetPuzzleItems());
    }

    private void LoadPuzzleItems(Sprite[] puzzleItems)
    {
        correctPuzzle = (Sprite[])puzzleItems.Clone();
        ShuffleArray(puzzleItems);

        ClearGrid();

        int numberOfPieces = puzzleItems.Length;
        SetGridDimensions(numberOfPieces);

        foreach (Sprite puzzleItem in puzzleItems)
        {
            GameObject puzzlePiece = Instantiate(puzzlePiecePrefab, puzzleContent);
            puzzlePiece.GetComponent<Image>().sprite = puzzleItem;
            puzzlePiece.GetComponent<Button>().onClick.AddListener(() => OnPieceSelected(puzzlePiece));
        }
    }

    private void SetGridDimensions(int numberOfPieces)
    {
        int rows = Mathf.FloorToInt(Mathf.Sqrt(numberOfPieces));
        int cols = Mathf.CeilToInt((float)numberOfPieces / rows);

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = cols;

        float cellSize = CalculateCellSize(numberOfPieces);
        gridLayoutGroup.cellSize = new Vector2(cellSize, cellSize - 50);
    }

    private float CalculateCellSize(int numberOfPieces)
    {
        float totalWidth = puzzleContent.rect.width;
        int cols = Mathf.CeilToInt(Mathf.Sqrt(numberOfPieces));

        float spacing = gridLayoutGroup.spacing.x;
        return (totalWidth - (spacing * (cols - 1))) / cols;
    }

    private void ShuffleArray(Sprite[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1); 
            Sprite temp = array[i];            
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
    
    private void ClearGrid()
    {
        foreach (Transform child in puzzleContent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnPieceSelected(GameObject selectedPiece)
    {
        if (firstSelectedPiece == null)
        {
            firstSelectedPiece = selectedPiece;
            HighlightPiece(firstSelectedPiece, true);
        } 
        else if(firstSelectedPiece == selectedPiece)
        {
            HighlightPiece(firstSelectedPiece, false);
            firstSelectedPiece = null;
        }
        else
        {
            SwapPieces(firstSelectedPiece, selectedPiece);
            HighlightPiece(firstSelectedPiece, false);
            firstSelectedPiece = null;
            CheckIfPuzzleCompleted();
        }
    }

    private void SwapPieces(GameObject piece1, GameObject piece2)
    {
        Image image1 = piece1.GetComponent<Image>();
        Image image2 = piece2.GetComponent<Image>();

        Sprite temp = image1.sprite;
        image1.sprite = image2.sprite;
        image2.sprite = temp;
    }

    private void HighlightPiece(GameObject piece, bool highlight)
    {
        Color highlightColor = highlight ? Color.yellow : Color.white;
        piece.GetComponent<Image>().color = highlightColor;
    }

    private void ShowCompletePuzzle()
    {
        PicturePuzzleSolutionMenu puzzleSolutionMenu = PanelManager.GetSingleton("puzzlesolution") as PicturePuzzleSolutionMenu;
        if (puzzleSolutionMenu != null)
        {
            Sprite solution = puzzleData.GetPuzzleSolution();
            puzzleSolutionMenu.ShowPuzzleSolution(solution);
        }
    }

    private void CompletePuzzle()
    {
        OnPuzzleSolved?.Invoke();
        PanelManager.GetSingleton("puzzlecomplete").Close();
        Close();
    }

    private void CheckIfPuzzleCompleted()
    {
        bool isCompleted = true;

        for (int i = 0; i < puzzleContent.childCount; i++)
        {
            Image currentPieceImage = puzzleContent.GetChild(i).GetComponent<Image>();
            Sprite currentSprite = currentPieceImage.sprite;

            if (currentSprite.name != correctPuzzle[i].name)
            {
                isCompleted = false;
                break;
            }
        }
        if (isCompleted)
        {
            PanelManager.GetSingleton("puzzlecomplete").Open();
        }
    }
}
