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
    IWorldObject _selected;
    Transform _selectedTransform;

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

    public IWorldObject SelectedObject
    {
        get { return _selected; }
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
    }

    void Start()
    {
        EventManager.StartListening(EventName.SelectObject, HandleSelectObjectEvent);
        EventManager.StartListening(EventName.RotateObject, HandleRotateObjectEvent);
        EventManager.StartListening(EventName.CycleObject, HandleCycleObjectEvent);
    }

    /// <summary>
    /// Event handler for newly selected WorldObject in scene
    /// Updates local references to selected object
    /// </summary>
    /// <param name="msg">selected IWorldObject</param>
    void HandleSelectObjectEvent(Dictionary<string, object> msg)
    {
        _selected = (IWorldObject) msg["obj"];
        WorldObject obj = (WorldObject) _selected;
        _selectedTransform = obj.gameObject.transform;

        Debug.Log($"We have selected a new object: {_selected}");
    }

    /// <summary>
    /// Event handler for clicking on the rotate button
    /// </summary>
    /// <param name="msg">null</param>
    void HandleRotateObjectEvent(Dictionary<string, object> msg)
    {
        Debug.Log("Rotating");
        _selectedTransform.Rotate(0, 0, 90);
    }

    /// <summary>
    /// Event handler for cycling the style of the selected WorldObject
    /// </summary>
    /// <param name="msg">null</param>
    void HandleCycleObjectEvent(Dictionary<string, object> msg)
    {
        Debug.Log("Cycling");
        _selected.CycleStyle();
    }

    #endregion
}
