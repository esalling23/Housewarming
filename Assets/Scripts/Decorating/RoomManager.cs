using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class RoomManager : MonoBehaviour
{
    TileManager[] _tileManagers;

    WorldObject _selected;
    BoxCollider2D _selectedCollider;
    Transform _selectedTransform;
    Dictionary <TileType, int> _tileStyleSelection;
    int _chosenTileIndex = 0;

    // Singleton
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

    public Dictionary<TileType, int> TileStyleSelection
    {
        get { return _tileStyleSelection; }
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

        _tileManagers = FindObjectsOfType<TileManager>();
        _tileStyleSelection = new Dictionary<TileType, int>();

        foreach (TileManager manager in _tileManagers)
        {
            // Debug.Log(manager.Type);
            WorldObject worldObject = manager.gameObject.GetComponent<WorldObject>();
            // Debug.Log(worldObject.SelectedStyleIndex);
            _tileStyleSelection.Add(manager.Type, worldObject.SelectedStyleIndex);
        }
    }

    void Start()
    {
        EventManager.StartListening(EventName.SelectObject, HandleUpdateSelected);
        EventManager.StartListening(EventName.RotateObject, HandleRotateSelected);
        EventManager.StartListening(EventName.CycleObject, HandleCycleSelected);
    }

    void HandleUpdateSelected(Dictionary<string, object> msg)
    {
        _selected = (WorldObject) msg["obj"];
        _selectedTransform = _selected.gameObject.transform;

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
        _selected.CycleStyle();

        if (_selected.IsTilemap)
        {
            _tileStyleSelection[_selected.TileManager.Type] = _selected.SelectedStyleIndex;
        }
    }
}
