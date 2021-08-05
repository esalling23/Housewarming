using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
// test
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

        Debug.Log("In GetTileData");

        if (Application.isPlaying)
        {
            Debug.Log(RoomManager.Instance.TileStyleSelection[type]);
            _newSprite = tiles[RoomManager.Instance.TileStyleSelection[type]];
            Debug.Log(_newSprite);

            tileData.sprite = _newSprite;
        }
    }
}
