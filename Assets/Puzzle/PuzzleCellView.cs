using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuzzleCellView : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    [SerializeField]
    private Button _button;

    public Action onClicked;
    
    public int Y { get; private set; }
    public int X { get; private set; }
    public int Number { get; private set; }

    public void Initialize(int y, int x)
    {
        Y = y;
        X = x;
        _button.onClick.AddListener(() => onClicked?.Invoke());
    }

    public void SetNumber(int number)
    {
        _label.text = $"{(number < Puzzle.Empty ? (number + 1) : 0)}";
        Number = number;
    }
}
