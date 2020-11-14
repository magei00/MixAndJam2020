﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu()]
public class Level : SerializedScriptableObject
{
    [TableMatrix(HorizontalTitle="Grid Layout", SquareCells=true)]
    public Cell[,] Grid;
    
    [ValueDropdown("GetNextAvailablePathCell")]
    public int2[] Path;

    public Dictionary<int2, StepUnit> Enemies;

    private IEnumerable<int2> GetNextAvailablePathCell()
    {
        if (Grid == null) return Enumerable.Empty<int2>();

        if (Path.Length == 0)
        {
            var WidthPossibilities = Enumerable.Range(0, Grid?.GetLength(0) ?? 0);
            var HeightPossibilities = Enumerable.Range(0, Grid?.GetLength(1) ?? 0);

            var AllPossibilities = from x in WidthPossibilities
                                   from y in HeightPossibilities
                                   select new int2(x, y);

            return AllPossibilities;
        }

        int2 Previous = Path[Path.Length - 1];
        List<int2> Candidates = new List<int2>() { 
            Previous + new int2(0, 1), 
            Previous + new int2(0, -1), 
            Previous + new int2(1, 0), 
            Previous + new int2(-1, 0) 
        };
        Debug.Log($"Previous {Previous}");
        Debug.Log($"Candiates Length: {Candidates.Count}");

        
        var Final = Candidates.Where(x => IsValidCell(x) && !Previous.Equals(x));
        Debug.Log($"Final Length: {Final.ToList().Count}");
        return Final;
    }

    private bool IsValidCell(int2 Cell)
    {
        return Cell.x < Grid.GetLength(0) && Cell.x >= 0 && Cell.y >= 0 && Cell.y < Grid.GetLength(1);
    }
}
