using UnityEngine;
using UnityEngine.UI;
public class PicturePuzzleSolutionMenu : Panel
{
    [SerializeField] private Button closeButton = null;
    private Image puzzleSolutionImage = null;

    public override void Initialize()
    {
        base.Initialize();
        closeButton.onClick.AddListener(Close);
        puzzleSolutionImage = transform.Find("Container/PuzzleSolution").GetComponent<Image>();
    }
    
    public void ShowPuzzleSolution(Sprite puzzleSolution)
    {
        base.Open();
        if (puzzleSolutionImage != null)
        {
            puzzleSolutionImage.sprite = puzzleSolution;
        }
        else
        {
            Debug.LogError("Puzzle solution image is not assigned!");
        }
    }
}
