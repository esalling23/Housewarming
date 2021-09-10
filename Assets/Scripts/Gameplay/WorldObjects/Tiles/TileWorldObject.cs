using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// WorldObject variant, one per type of tile used in the scene
/// </summary>
public class TileWorldObject : WorldObject
{
    #region Fields

    Tilemap _tilemap;
    [SerializeField] TileType _tileType;
    [SerializeField] int _cycleLimit = 0;

    #endregion

    #region Properties
    public int CycleLimit
    {
        get { return _cycleLimit; }
    }

    public TileType TileType
    {
        get { return _tileType; }
    }

    #endregion

    #region Methods

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();
        if (_tilemap == null)
        {
            throw new Exception("WorldObject with Tilemap child must have TileManager component");
        }
    }

    public override void CycleStyle()
    {
        _selectedStyleIndex = WorldObjectUtils.GetNextStyleIndex(_selectedStyleIndex, CycleLimit);
        // Tell the tile manager
        TileManager.Instance.UpdateTileSelection(TileType, _selectedStyleIndex);
        _tilemap.RefreshAllTiles();
    }

    #endregion
}
