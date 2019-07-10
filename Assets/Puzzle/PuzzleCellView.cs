using UnityEngine;
using UnityEngine.UI;

public class PuzzleCellView : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    public int Number { get; private set; }

    public void SetNumber(int number)
    {
        _label.text = $"{(number != Puzzle.Area - 1 ? (number + 1).ToString() : "")}";
        Number = number;
    }
}
