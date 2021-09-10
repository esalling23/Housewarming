using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformUtils
{
    /// <summary>
    /// Sets the X and Y positions of `posMod` to match `posSet`
    /// </summary>
    /// <param name="posMod">The position to modify & return</param>
    /// <param name="posSet">The position to use to set the X and Y values of `posMod`</param>
    /// <returns>Modified `posMod` vector</returns>
    public static Vector3 SetXYPosition(Vector3 posMod, Vector3 posSet)
    {
        posMod.x = posSet.x;
        posMod.y = posSet.y;
        return posMod;
    }
}
