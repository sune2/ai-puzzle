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

    private bool _showView;
    
    public int Y { get; private set; }
    public int X { get; private set; }
    public int Number { get; private set; }

    public void Initialize(int y, int x, bool showView)
    {
        Y = y;
        X = x;
        _showView = showView;
        _button.onClick.AddListener(() => onClicked?.Invoke());
    }

    public void SetNumber(int number)
    {
                if (_showView)
        {
            _label.text = $"{(number < Puzzle.Empty ? (number + 1).ToString() : "")}";
        }
        Number = number;
    }
}
