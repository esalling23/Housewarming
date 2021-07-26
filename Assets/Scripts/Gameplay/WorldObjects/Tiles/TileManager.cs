using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages selected tile styles across all TileWorldObjects
/// </summary>
public class TileManager : MonoBehaviour
{
    #region Fields
    TileWorldObject[] _tileObjects;
    Dictionary<TileType, int> _tileStyleSelection;
    static TileManager _instance;

    #endregion

    #region Properties

    /// <summary>
    /// Singleton instance property
    /// </summary>
    /// <value>TileManager instance object</value>
    public static TileManager Instance
    {
        get
        {
            if (!_instance)
            {
                TileManager objManager = GameObject.FindObjectOfType(typeof(TileManager)) as TileManager;

                if (!objManager)
                {
                    Debug.LogError("Must have one enabled TileManager object in the scene");
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
        _tileObjects = FindObjectsOfType<TileWorldObject>();
        _tileStyleSelection = new Dictionary<TileType, int>();

        foreach (TileWorldObject obj in _tileObjects)
        {
            _tileStyleSelection.Add(obj.TileType, obj.SelectedStyleIndex);
        }
    }

    /// <summary>
    /// Updates dictionary of tile types and style selection
    /// </summary>
    /// <param name="type">The type of tile we're updating</param>
    /// <param name="index">The style index to use for the tiles</param>
    public void UpdateTileSelection(TileType type, int index)
    {
        _tileStyleSelection[type] = index;
    }

    #endregion
}
