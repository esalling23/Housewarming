using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class WorldObjectUtils
{
    #region Fields
    static ContactFilter2D _contactFilter = new ContactFilter2D();
    #endregion

    #region Properties

    /// <summary>
    /// Determine which layers to collide with to avoid placement on top
    /// </summary>
    public static LayerMask LayerMask
    {
        get
        {
            return _contactFilter.layerMask;
        }
    }

    /// <summary>
    /// Is the mouse over a UI element or a world object?
    /// </summary>
    public static bool IsOverUI
    {
        get { return EventSystem.current.IsPointerOverGameObject(); }
    }

    #endregion

    #region Methods

    public static void Init()
    {
        LayerMask layerMask = (1 << LayerMask.NameToLayer("Walls"))
                                | (1 << LayerMask.NameToLayer("Furniture"))
                                | (1 << LayerMask.NameToLayer("Props"));
        _contactFilter.SetLayerMask(layerMask);
    }

    /// <summary>
    /// Determines if object can be placed in desired drop location
    /// </summary>
    public static bool CanPlace(BoxCollider2D collider)
    {
        Collider2D[] overlap = Physics2D.OverlapAreaAll(collider.bounds.min, collider.bounds.max, LayerMask);
        if (overlap.Length > 1)
        {
            // Debug.Log("Cannot Place");
            Debug.Log(string.Format("Found {0} overlapping object(s)", overlap.Length - 1));
            foreach (Collider2D o in overlap) { Debug.Log(o.gameObject, o.gameObject); }
            return false;
        }

        return true;
    }

    /// <summary>
    /// Loops index based on current and limit
    /// </summary>
    /// <param name="currIndex">Current index to change</param>
    /// <param name="limit">Limit of where loop should restart at 0</param>
    /// <returns>Next index in loop</returns>
    public static int GetNextStyleIndex(int currIndex, int limit)
    {
        if (currIndex < limit)
        {
            currIndex++;
        }
        else
        {
            currIndex = 0;
        }

        return currIndex;
    }

    /// <summary>
    /// Gets a random position within a certain bounds
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="randPos"></param>
    /// <returns></returns>
    public static Vector3 GetRandomPos(BoundsInt bounds, Vector3 randPos)
    {
        float xMin = bounds.xMin;
        float xMax = bounds.xMax;
        float yMin = bounds.yMin;
        float yMax = bounds.yMax;

        randPos.x = Random.Range(xMin, xMax);
        randPos.y = Random.Range(yMin, yMax);
        return randPos;
    }
    #endregion
}
