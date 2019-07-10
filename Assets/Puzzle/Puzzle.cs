using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Puzzle : MonoBehaviour
{
    private const int Width = 4;
    private const int Height = 4;
    public const int Area = Width * Height;

    public static readonly int Empty = Width * Height - 1;
    
    private static readonly int[] dy = {-1, 0, 1, 0};
    private static readonly int[] dx = {0, 1, 0, -1};

    [SerializeField]
    private PuzzleCellView _cellViewPrefab;

    [SerializeField]
    private GridLayoutGroup _gridLayoutGroup;

    private PuzzleCellView[][] _cellViews;
    private int[][] _puzzle;

    private Vector2Int _emptyPos;

    // Use this for initialization
    void Awake()
    {
        // Time.timeScale = 0.05f;
        _gridLayoutGroup.constraintCount = Width;
        _cellViews = new PuzzleCellView[Height][];
        for (int i = 0; i < Height; i++)
        {
            _cellViews[i] = new PuzzleCellView[Width];
            for (int j = 0; j < Width; j++)
            {
                var cellView = Instantiate(_cellViewPrefab, _gridLayoutGroup.transform, false);
                _cellViews[i][j] = cellView;
            }
        }
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
        var cnt = 1;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                var cellView = _cellViews[i][j];
                cellView.SetNumber(cnt++);
            }
        }

        // 右下のセルは0にして空白を表現する
        _cellViews[Height - 1][Width - 1].SetNumber(0);
        _emptyPos = new Vector2Int(Width - 1, Height - 1);

        // 適当な回数ランダムでスライドして初期配置を作る
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
        var x = _emptyPos.x + dx[dir];
        var y = _emptyPos.y + dy[dir];

        if (x < 0 || x >= Width || y < 0 || y >= Height) return false;

        var cellView = _cellViews[_emptyPos.y][_emptyPos.x];
        int number = _cellViews[y][x].Number;
        _cellViews[y][x].SetNumber(cellView.Number);
        cellView.SetNumber(number);

        _emptyPos = new Vector2Int(x, y);

        return true;
    }

    public int GetFirstDifferentIndex()
    {
        var cnt = 0;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (cnt != _cellViews[i][j].Number)
                {
                    return cnt;
                }

                cnt++;
            }
        }

        return Area;
    }
}
