using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Cycle Tile", menuName = "Tiles/Cycle Tile")]

public class CycleTile : Tile
{
    [SerializeField]
    List<Sprite> tiles;

    Sprite newSprite;

    public override void GetTileData(Vector3Int location, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(location, tilemap, ref tileData);

        Debug.Log("In GetTileData");

        if (Application.isPlaying)
        {
            Debug.Log(RoomManager.Instance.ChosenTileIndex);
            newSprite = tiles[RoomManager.Instance.ChosenTileIndex];
            Debug.Log(newSprite);

            tileData.sprite = newSprite;
        }
    }
}
