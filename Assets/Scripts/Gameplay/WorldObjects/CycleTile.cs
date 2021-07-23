using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Custom Tile object that supports cycling through different tyles based
/// on a selection in the RoomManager
/// </summary>
[CreateAssetMenu(fileName = "New Cycle Tile", menuName = "Tiles/Cycle Tile")]

public class CycleTile : Tile
{
    [SerializeField]
    List<Sprite> tiles;

    [SerializeField]
    TileType type;

    Sprite _newSprite;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(location, tilemap, ref tileData);

        // Debug.Log("In GetTileData");

        if (Application.isPlaying)
        {
            // Debug.Log(RoomManager.Instance.TileStyleSelection[type]);
            _newSprite = tiles[WorldObjectManager.Instance.TileStyleSelection[type]];
            // Debug.Log(_newSprite);

            tileData.sprite = _newSprite;
        }
    }
}
