using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles user interaction with provided UI buttons in Area portion of each phase
/// </summary>
public class AreaHUD : MonoBehaviour
{
    #region Fields
    [SerializeField] Text _selectedObjText;
    [SerializeField] GameObject _confirmPrompt;
    [SerializeField] Button _cycleButton;
    [SerializeField] Button _rotateButton;
    #endregion

    #region Methods

    void Start()
    {
        EventManager.StartListening(EventName.StartArea, ShowHUD);
        EventManager.StartListening(EventName.SelectObject, UpdateSelectedObject);
        CloseHUD();
    }

    /// <summary>
    /// Event handler - shows HUD and handles HUD changes from area to area
    /// </summary>
    /// <param name="msg"></param>
    void ShowHUD(Dictionary<string, object> msg)
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles closing HUD and confirmation prompt
    /// </summary>
    void CloseHUD()
    {
        gameObject.SetActive(false);
        _confirmPrompt.SetActive(false);
    }

    /// <summary>
    /// Event handler - Updates UI with name of selected object
    /// </summary>
    /// <param name="msg"></param>
    void UpdateSelectedObject(Dictionary<string, object> msg)
    {
        Debug.Log($"Selected {msg}");
        IWorldObject obj = (IWorldObject) msg["obj"];
        _selectedObjText.text = "Selected: " + obj.Name;

        // Display options for this type of object
        DisplayObjectOptionBtns(obj.Type);
    }

    /// <summary>
    /// Sets the buttons to display for world object options
    /// </summary>
    /// <param name="type">Type of world object to display buttons for</param>
    void DisplayObjectOptionBtns(WorldObjectType type)
    {
        switch (type)
        {
            case WorldObjectType.Tile:
                _rotateButton.gameObject.SetActive(false);
            break;

            case WorldObjectType.Sprite:
                _rotateButton.gameObject.SetActive(true);
            break;
        }
    }

    /// <summary>
    /// Button click handler for rotating WorldObject - set in inspector
    /// </summary>
    public void HandleRotateBtnClick()
    {
        EventManager.TriggerEvent(EventName.RotateObject, null);
    }

    /// <summary>
    /// Button click handler for cycling WorldObject style - set in inspector
    /// </summary>
    public void HandleCycleBtnClick()
    {
        EventManager.TriggerEvent(EventName.CycleObject, null);
    }

    /// <summary>
    /// Button click handler for completing an area
    /// Triggers a confirmation prompt
    /// </summary>
    public void HandleContinueAreaBtnClick()
    {
        // To Do: Animate
        _confirmPrompt.SetActive(true);
    }

    /// <summary>
    /// Button click handler for confirming completing an area
    /// </summary>
    public void HandleConfirmContinueAreaBtnClick()
    {
        CloseHUD();
        EventManager.TriggerEvent(EventName.CompleteArea, null);
    }

    /// <summary>
    /// Button click handler for going back to an are from the confirmation prompt
    /// </summary>
    public void HandleCancelContinueAreaBtnClick()
    {
        _confirmPrompt.SetActive(false);
    }

    #endregion
}
