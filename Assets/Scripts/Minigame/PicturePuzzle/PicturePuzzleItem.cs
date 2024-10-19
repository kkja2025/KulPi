using System;
using UnityEngine;

public class PicturePuzzleItem : MonoBehaviour
{
    [SerializeField] private Sprite[] puzzleItems;
    [SerializeField] private Sprite puzzleSolution;

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
        }
    }

    public void HandlePuzzleSolved()
    {
        QuestItemInteraction itemInteraction = GetComponent<QuestItemInteraction>();
        itemInteraction.OnQuestItemCompletion();
    }
}
