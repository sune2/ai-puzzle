using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Puzzle : MonoBehaviour
{
    public const int Width = 4;
    public const int Height = 4;
    public const int Area = Width * Height;

    public static readonly int Empty = Width * Height - 1;

    [SerializeField]
    private PuzzleCellView _cellViewPrefab;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    private PuzzleCellView[][] _cellViews;
    private int[][] _puzzle;
    
    private ValueTuple<int, int>[] _correctPositions;

    private Vector2Int _zeroPos;

    // Use this for initialization
    void Awake()
    {
        // Time.timeScale = 0.05f;
        _gridLayoutGroup.constraintCount = Width;
        _cellViews = new PuzzleCellView[Height][];
        _correctPositions = new ValueTuple<int, int>[Area];
        for (int i = 0; i < Height; i++)
        {
            _cellViews[i] = new PuzzleCellView[Width];
            for (int j = 0; j < Width; j++)
            {
                var cellView = Instantiate(_cellViewPrefab, _gridLayoutGroup.transform, false);
                cellView.Initialize(i, j);
                cellView.onClicked += () => OnClickedCell(cellView);
                _cellViews[i][j] = cellView;
                _correctPositions[i * Width + j] = ValueTuple.Create(i, j); 
            }
        }

        // Initialize();
    }

//    private void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//        {
//            Slide(2);
//        }
//        else if (Input.GetKeyDown(KeyCode.RightArrow))
//        {
//            Slide(3);
//        }
//        else if (Input.GetKeyDown(KeyCode.DownArrow))
//        {
//            Slide(0);
//        }
//        else if (Input.GetKeyDown(KeyCode.LeftArrow))
//        {
//            Slide(1);
//        }
//    }

    public void Initialize()
    {
        var numbers = Enumerable.Range(0, Width * Height).OrderBy(x => Guid.NewGuid()).ToArray();

        var cnt = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var cellView = _cellViews[i][j];
                cellView.SetNumber(cnt++);
            }
        }

        _zeroPos = new Vector2Int(Width - 1, Height - 1);

        for (int i = 0; i < Area * Area * 3; i++)
        {
            Slide(Random.Range(0, 4));
        }
    }

    public IList<int> GetCellNumbers()
    {
        var res = new List<int>();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                res.Add(_cellViews[i][j].Number);
            }
        }

        return res;
    }

    public bool Slide(int dir)
    {
        var x = _zeroPos.x + dx[dir];
        var y = _zeroPos.y + dy[dir];

        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        var cellView = _cellViews[_zeroPos.y][_zeroPos.x];
        int number = _cellViews[y][x].Number;
        _cellViews[y][x].SetNumber(cellView.Number);
        cellView.SetNumber(number);

        _zeroPos = new Vector2Int(x, y);

        return true;
    }

    private static readonly int[] dy = {-1, 0, 1, 0};
    private static readonly int[] dx = {0, 1, 0, -1};

    private void OnClickedCell(PuzzleCellView cellView)
    {
        for (int k = 0; k < 4; k++)
        {
            int y = cellView.Y + dy[k];
            int x = cellView.X + dx[k];
            if (y >= 0 && y < Height && x >= 0 && x < Width)
            {
                if (_cellViews[y][x].Number == Empty)
                {
                    int number = _cellViews[y][x].Number;
                    _cellViews[y][x].SetNumber(cellView.Number);
                    cellView.SetNumber(number);
                    _zeroPos = new Vector2Int(cellView.X, cellView.Y);
                }
            }
        }
    }

    public int SameCount()
    {
        var numbers = GetCellNumbers();
        var cnt = 0;
        for (int i = 0; i < Width * Height; i++)
        {
            if (numbers[i] == i) cnt++;
        }

        return cnt;
    }

    public int InversionNumber()
    {
        var numbers = GetCellNumbers();
        var cnt = 0;
        for (int i = 1; i < Width * Height; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (numbers[i] < numbers[j]) cnt++;
            }
        }

        return cnt;
    }

    public int GetFirstDifferentIndex()
    {
        var numbers = GetCellNumbers();

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] != i)
            {
                return i;
            }
        }

        return numbers.Count;
    }
    
    public int GetDistanceSum()
    {
        int res = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var number = _cellViews[i][j].Number;
                int y = _correctPositions[number].Item1;
                int x = _correctPositions[number].Item2;
                res += Mathf.Abs(i - y) + Mathf.Abs(j - x); 
            }
        }

        return res;
    }
}
