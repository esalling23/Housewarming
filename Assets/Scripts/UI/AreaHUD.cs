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
    [SerializeField] Button _cycleButton;
    [SerializeField] Button _rotateButton;
    HUDAnimator _animator;

    #endregion

    #region Methods

    private void Awake()
    {
        _animator = GetComponent<HUDAnimator>();
    }

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
        _animator.AnimateHUDIn();
    }

    /// <summary>
    /// Handles closing HUD and confirmation prompt
    /// </summary>
    void CloseHUD()
    {
        _animator.AnimateHUDOut();
    }

    /// <summary>
    /// Event handler - Updates UI with name of selected object
    /// </summary>
    /// <param name="msg">Selected IWorldObject</param>
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
            case WorldObjectType.Floor:
            case WorldObjectType.Walls:
                _rotateButton.gameObject.SetActive(false);
                _cycleButton.gameObject.SetActive(true);
            break;

            case WorldObjectType.Furniture:
                _rotateButton.gameObject.SetActive(true);
                _cycleButton.gameObject.SetActive(true);
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
        _animator.AnimateConfirmPromptIn();
        _animator.AnimateOptionBtnsOut();
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
        _animator.AnimateConfirmPromptOut();
        _animator.AnimateOptionBtnsIn();
    }

    #endregion
}
