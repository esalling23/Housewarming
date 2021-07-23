using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages tile type and the style cycle limit
/// Required for WorldObjects that contain Tilemaps as their child
/// </summary>
public class TileManager : MonoBehaviour
{
    #region Fields
    [SerializeField] TileType _type;

    [SerializeField] int _cycleLimit;

    #endregion

    #region Properties

    public int CycleLimit
    {
        get { return _cycleLimit; }
    }

    public TileType Type
    {
        get { return _type; }
    }

    #endregion
}
