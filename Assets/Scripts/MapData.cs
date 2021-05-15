using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class MapData
{
    public Vector2Int mapSize;
    public int[] mapData;

    public Vector2Int playerPosition;
}
