using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    TileType _type;

    [SerializeField] int _cycleLimit;

    public int CycleLimit
    {
        get { return _cycleLimit; }
    }

    public TileType Type
    {
        get { return _type; }
    }
}
