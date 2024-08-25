using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class GameSupporter : MonoBehaviour
{
    public enum map2dObject
    {
        noting = 0,
        wall = 1,
        player = 2,
        moster = 3  
    }
    //public bool TurnEnd { get; set; }
    //public bool TurnStart { get; set; }
    public int MapSizeX { get; set; } = 11;
    public int MapSizeZ { get; set; } = 15;
    public int[,] Map2D { get; set; }
    public int WallPlane { get; set; } = 150;
    public int MonsterPlane { get; set; } = 10;
}
