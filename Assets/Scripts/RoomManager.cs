using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    GameObject _selected;
    BoxCollider2D _selectedCollider;
    Transform _selectedTransform;

    // Tile change support
    // [SerializeField]
    // Tilemap _floorTileMap;
    // CycleTile _floorTile;

    int _chosenTileIndex = 0;

    // Singleton (i think)
    static RoomManager _instance;

    public static RoomManager Instance
    {
        get {
            if (!_instance)
            {
                GameObject roomManager = GameObject.Find("RoomManager");

                if (!roomManager)
                {
                    Debug.LogError("Must have one enabled RoomManager object in the scene");
                }

                _instance = roomManager.GetComponent<RoomManager>();
            }

            return _instance;
        }
    }

    public int ChosenTileIndex
    {
        get { return _chosenTileIndex; }
    }

    void Awake()
    {
        if (!_instance)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        EventManager.StartListening(EventName.SelectObject, HandleUpdateSelected);
        EventManager.StartListening(EventName.RotateObject, HandleRotateSelected);
        EventManager.StartListening(EventName.RotateObject, HandleCycleSelected);
    }

    void Update()
    {

    }

    void HandleUpdateSelected(Dictionary<string, object> msg)
    {
        _selected = (GameObject) msg["obj"];
        _selectedTransform = _selected.transform;
        _selectedCollider = _selected.GetComponent<BoxCollider2D>();

        Debug.Log(_selected);
    }

    void HandleRotateSelected(Dictionary<string, object> msg)
    {
        Debug.Log("Rotating");
        _selectedTransform.Rotate(0, 0, 90);
    }

    void HandleCycleSelected(Dictionary<string, object> msg)
    {
        Debug.Log("Cycling");
    }

    // public void HandleCycleFloorTiles(int limit)
    // {
    //     Debug.Log($"Chosen tile index is {_chosenTileIndex}");
    //     Debug.Log($"Limit index count is {limit}");
    //     if (_chosenTileIndex < limit)
    //     {
    //         _chosenTileIndex++;
    //     }
    //     else
    //     {
    //         _chosenTileIndex = 0;
    //     }
    //     Debug.Log(_floorTileMap);
    //     _floorTileMap.RefreshAllTiles();
    // }
}
