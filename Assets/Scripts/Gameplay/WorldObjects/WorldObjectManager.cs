using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles room/area view and selecting of different world objects
/// </summary>
public class WorldObjectManager : MonoBehaviour
{
    #region Fields
    // WorldObject selection support
    WorldObject _selected;
    Transform _selectedTransform;

    // Support for tile based WorldObjects
    TileManager[] _tileManagers;
    Dictionary <TileType, int> _tileStyleSelection;

    // Singleton instance
    static WorldObjectManager _instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance property
    /// </summary>
    /// <value>WorldObjectManager instance object</value>
    public static WorldObjectManager Instance
    {
        get {
            if (!_instance)
            {
                WorldObjectManager objManager = GameObject.FindObjectOfType(typeof(WorldObjectManager)) as WorldObjectManager;

                if (!objManager)
                {
                    Debug.LogError("Must have one enabled WorldObjectManager object in the scene");
                }

                _instance = objManager;
            }

            return _instance;
        }
    }

    /// <summary>
    /// Dictionary storage of different tile types and the chosen style index for each
    /// Allows for all tiles of a given type (floor, wall) to use the same style
    /// </summary>
    /// <value>Dictionary of types and selected style index</value>
    public Dictionary<TileType, int> TileStyleSelection
    {
        get { return _tileStyleSelection; }
    }

    #endregion

    #region Methods

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

        // Locate all TileManagers in the scene to determine starter style for those tiles
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

    /// <summary>
    /// Event handler for newly selected WorldObject in scene
    /// Updates local references to selected object
    /// </summary>
    /// <param name="msg">null</param>
    void HandleUpdateSelected(Dictionary<string, object> msg)
    {
        _selected = (WorldObject) msg["obj"];
        _selectedTransform = _selected.gameObject.transform;

        Debug.Log($"We have selected a new object: {_selected}");
    }

    /// <summary>
    /// Event handler for clicking on the rotate button
    /// </summary>
    /// <param name="msg">null</param>
    void HandleRotateSelected(Dictionary<string, object> msg)
    {
        Debug.Log("Rotating");
        _selectedTransform.Rotate(0, 0, 90);
    }

    /// <summary>
    /// Event handler for cycling the style of the selected WorldObject
    /// </summary>
    /// <param name="msg">null</param>
    void HandleCycleSelected(Dictionary<string, object> msg)
    {
        Debug.Log("Cycling");
        _selected.CycleStyle();

        if (_selected.IsTilemap)
        {
            _tileStyleSelection[_selected.TileManager.Type] = _selected.SelectedStyleIndex;
        }
    }

    #endregion
}
