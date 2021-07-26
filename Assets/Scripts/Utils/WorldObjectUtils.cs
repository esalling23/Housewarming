using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.Log("Cannot Place");
            Debug.Log(string.Format("Found {0} overlapping object(s)", overlap.Length - 1));
            return false;
        }

        return true;
    }

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
