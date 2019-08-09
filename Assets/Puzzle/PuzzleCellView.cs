using UnityEngine;
using UnityEngine.UI;

public class PuzzleCellView : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    [SerializeField]
    private Image _image;

    public int Number { get; private set; }

    public void SetNumber(int number)
    {
        _label.text = $"{(number != Puzzle.Area - 1 ? (number + 1).ToString() : "")}";
        Number = number;
    }

    public void SetClearState(bool isClear)
    {
        _image.color = isClear ? new Color32(0xFF, 0xEE, 0x99, 0xFF) : new Color32(0xFF, 0xFF, 0xFF, 0xFF);
    }
}
