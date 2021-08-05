using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    Text _selectedObjText;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventName.SelectObject, UpdateSelectedObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateSelectedObject(Dictionary<string, object> msg)
    {
        Debug.Log($"Selected {msg["name"]}");
        _selectedObjText.text = "Selected: " + msg["name"].ToString();
    }

    public void HandleRotateBtnClick()
    {
        EventManager.TriggerEvent(EventName.RotateObject, null);
    }

    public void HandleCycleBtnClick()
    {
        EventManager.TriggerEvent(EventName.CycleObject, null);
    }
}
