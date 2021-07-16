using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    #region Fields

    // Object details for display
    public string name;

    #endregion

    #region Methods

    void OnMouseDown() {
        EventManager.TriggerEvent(EventName.SelectObject, new Dictionary<string, object> {
            { "name", name },
            { "obj", gameObject }
        });
    }

    #endregion
}
