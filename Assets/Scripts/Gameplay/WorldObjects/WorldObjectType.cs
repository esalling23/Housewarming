using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describes types of IWorldObjects
/// </summary>
public enum WorldObjectType
{
    Food,
    // Props might be on top of furniture (a book, plate, box)
    Props,
    Furniture,
    Walls,
    Floor
}
