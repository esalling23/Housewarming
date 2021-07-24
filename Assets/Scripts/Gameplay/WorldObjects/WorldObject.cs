using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

/// <summary>
/// WorldObject fundamental functionality - baseclass
/// </summary>
public class WorldObject : MonoBehaviour, IWorldObject
{
    #region Fields

    // Object details for display
    [SerializeField] protected string _name;
    [SerializeField] protected WorldObjectType _type;

    // Cycle style support
    protected int _selectedStyleIndex;

    #endregion

    #region Properties

    public string Name
    {
        get { return _name; }
    }

    public int SelectedStyleIndex
    {
        get { return _selectedStyleIndex; }
    }

    public WorldObjectType Type
    {
        get { return _type; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Changes the visual of the object based on different styles
    /// </summary>
    public virtual void CycleStyle()
    {
        Debug.Log("Whoops we're in the base class CycleStyle");
    }

    /// <summary>
    /// Selects the object on mouse click
    /// </summary>
    void OnMouseDown() {
        // Ignore UI elements
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EventManager.TriggerEvent(EventName.SelectObject, new Dictionary<string, object> {
                { "obj", this }
            });
        }
    }

    #endregion
}
