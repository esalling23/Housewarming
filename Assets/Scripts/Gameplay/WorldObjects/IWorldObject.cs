using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contract for WorldObjects
/// </summary>
public interface IWorldObject
{
    string Name { get; }
    WorldObjectType Type { get; }
    int SelectedStyleIndex { get; }
    void CycleStyle();
}
