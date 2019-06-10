using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MLAgents;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleAgent : Agent
{
    [SerializeField]
    Puzzle _puzzle;
    
    private int _currentMaxIndex;

    public override void AgentReset()
    {
        base.AgentReset();
        _puzzle.Initialize();
        _currentMaxIndex = _puzzle.GetFirstDifferentIndex();
    }

    public override void CollectObservations()
    {
        base.CollectObservations();
        var vec = new float[Puzzle.Area * Puzzle.Area];
        var numbers = _puzzle.GetCellNumbers();
        // Debug.Log(string.Join(",", numbers));
        for (int i = 0; i < Puzzle.Area; i++)
        {
            vec[numbers[i] * Puzzle.Area + i] = 1;
        }

        AddVectorObs(vec);
    }
    

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        base.AgentAction(vectorAction, textAction);
        var index = Mathf.RoundToInt(vectorAction[0]);

        var success = _puzzle.Slide(index);
        if (!success) AddReward(-1);

        var val = _puzzle.GetFirstDifferentIndex();

        if (val > _currentMaxIndex)
        {
            for (int i = _currentMaxIndex; i < val; i++)
            {
                AddReward(5 * i);                
            }
            _currentMaxIndex = val;
        }

        AddReward(-0.001f * (Puzzle.Area - val));

        if (val == Puzzle.Area)
        {
            // AddReward(200);
            Done();
        }
    }
}
