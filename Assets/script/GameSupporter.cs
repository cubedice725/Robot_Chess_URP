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
    // 생성할 개수
    private int wallPlane = 150;
    private int monsterPlane = 10;
    private int mapSizeX = 11;
    private int mapSizeZ = 15;
    //public bool TurnEnd { get; set; }
    //public bool TurnStart { get; set; }
    public int MapSizeX { get { return mapSizeX; } set { mapSizeX = value; } }
    public int MapSizeZ { get { return mapSizeZ; } set { mapSizeZ = value; } }
    public int[,] Map2D { get; set; }
    public int WallPlane { get { return wallPlane; } set { wallPlane = value; } }
    public int MonsterPlane { get { return monsterPlane; } set { monsterPlane = value; } }
}
