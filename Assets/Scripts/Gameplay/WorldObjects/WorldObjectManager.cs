using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles room/area view and selecting of different world objects
/// </summary>
public class WorldObjectManager : MonoBehaviour
{
    #region Fields
    // WorldObject selection support
    IWorldObject _selected;
    Transform _selectedTransform;

    // Enable/disable support
    // Consider splitting into seperate lists of each type of world object
    // so we don't have to loop & locate the types we want later on
    IWorldObject[] _worldObjects;
    IWorldObject[] _furniture;

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

    private IWorldObject[] WorldObjects
    {
        get {
            return _worldObjects;
        }
    }

    public IWorldObject[] Furniture
    {
        get {
            return _furniture;
        }
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

        GetWorldObjects();
    }

    void Start()
    {
        EventManager.StartListening(EventName.EnableObjectSelect, HandleEnableSelectEvent);
        EventManager.StartListening(EventName.SelectObject, HandleSelectObjectEvent);
        EventManager.StartListening(EventName.RotateObject, HandleRotateObjectEvent);
        EventManager.StartListening(EventName.CycleObject, HandleCycleObjectEvent);
    }

    /// <summary>
    /// Gets all objects in scene that implement an IWorldObject component
    /// </summary>
    void GetWorldObjects()
    {
        _worldObjects = FindObjectsOfType<MonoBehaviour>()
            .OfType<IWorldObject>().ToArray();

        _furniture = (IWorldObject[]) _worldObjects.Where(o => o.Type == WorldObjectType.Furniture).ToArray();
    }

    /// <summary>
    /// Event handler to toggle on/off types of world objects
    /// Disables all world objects not set to be enabled and vise-versa
    /// </summary>
    /// <param name="msg">Dictionary with "types" and "clear" keys</param>
    void HandleEnableSelectEvent(Dictionary<string, object> msg)
    {
        WorldObjectType[] types = (WorldObjectType[]) msg["types"];

        foreach (IWorldObject obj in _worldObjects)
        {
            if (types.Contains(obj.Type))
            {
                ToggleEnableObject(obj, true);
            }
            // Optionally disable all other types of objects
            else if ((bool) msg["clear"])
            {
                ToggleEnableObject(obj, false);
            }
        }
    }

    /// <summary>
    /// Enables or disables this world object and any other interactive components
    /// </summary>
    /// <param name="obj">IWorldObject to toggle</param>
    /// <param name="enabled">To enable or disable</param>
    void ToggleEnableObject(IWorldObject obj, bool enabled)
    {
        obj.Active = enabled;

        if (obj.GameObject.TryGetComponent(out DragDrop dragDrop))
        {
            dragDrop.enabled = enabled;
        }
    }

    /// <summary>
    /// Event handler for newly selected WorldObject in scene
    /// Updates local references to selected object
    /// </summary>
    /// <param name="msg">selected IWorldObject</param>
    void HandleSelectObjectEvent(Dictionary<string, object> msg)
    {
        _selected = (IWorldObject) msg["obj"];
        _selectedTransform = _selected.GameObject.transform;

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
