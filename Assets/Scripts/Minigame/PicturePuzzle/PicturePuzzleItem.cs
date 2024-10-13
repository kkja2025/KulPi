using System;
using UnityEngine;

public class PicturePuzzleItem : MonoBehaviour
{
    [SerializeField] private Sprite[] puzzleItems;
    [SerializeField] private Sprite puzzleSolution;
    public event Action OnPuzzleCompleted;

    public Sprite[] GetPuzzleItems()
    {
        return puzzleItems;
    }

    public Sprite GetPuzzleSolution()
    {
        return puzzleSolution;
    }

    public void ShowPuzzle()
    {
        PicturePuzzleMenu puzzleMenu = PanelManager.GetSingleton("picturepuzzle") as PicturePuzzleMenu;
        if (puzzleMenu != null)
        {
            puzzleMenu.ShowPuzzle(this);
            puzzleMenu.OnPuzzleSolved += HandlePuzzleSolved;
        }
    }

    private void HandlePuzzleSolved()
    {
        OnPuzzleCompleted?.Invoke();
    }
}
