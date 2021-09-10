using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages the area view such as set up for a new phase area
/// </summary>
public class AreaManager : MonoBehaviour
{
    #region Fields

    // Various area game objects to be loaded in
    [SerializeField] Transform _draggablesContainer;
    // Used for Food phase
    [SerializeField] Transform _diningTable;
    [SerializeField] Transform _foodContainer;
    GameObject _foodGameObj;
    // Used to determine initial random placements of world objects
    [SerializeField] Tilemap _wallTiles;

    [SerializeField] GameObject[] _worldObjects;
    [SerializeField] GameObject[] _plateObjects;
    Vector3 _randPos = new Vector3();
    // Cache camera data
    Transform _cameraTransform;
    float _cameraRoomViewSize;


    #endregion

    #region Methods

    void Awake()
    {
        _foodGameObj = _foodContainer.gameObject;
    }

    void Start()
    {
        EventManager.StartListening(EventName.StartArea, HandleStartAreaEvent);
        _cameraRoomViewSize = GameManager.Instance.MainCamera.orthographicSize;
        _cameraTransform = GameManager.Instance.MainCamera.transform;
    }

    void SetCameraToRoomView()
    {
        GameManager.Instance.MainCamera.orthographicSize = _cameraRoomViewSize;
        _cameraTransform.position = TransformUtils.SetXYPosition(_cameraTransform.position, Vector3.zero);
    }

    /// <summary>
    /// Event handler for starting area
    /// </summary>
    /// <param name="msg">null</param>
    void HandleStartAreaEvent(Dictionary<string, object> msg)
    {
        SetupArea();
    }

    /// <summary>
    /// Manages setting up area view for new phase
    /// </summary>
    void SetupArea()
    {
        Camera camera = GameManager.Instance.MainCamera;

        switch (GameManager.Instance.CurrentPhase.phase)
        {
            case GamePhaseName.Decorating:

                // Maybe: random placement
                // PlaceWorldObjects();
                EventManager.TriggerEvent(EventName.EnableObjectSelect, new Dictionary<string, object>
                {
                    { "types", new WorldObjectType[] { WorldObjectType.Furniture, WorldObjectType.Walls, WorldObjectType.Floor } },
                    { "clear", true }
                });

                // Hide food
                _foodGameObj.SetActive(false);

                // Set camera size
                SetCameraToRoomView();
            break;

            case GamePhaseName.Food:

                EventManager.TriggerEvent(EventName.EnableObjectSelect, new Dictionary<string, object>
                {
                    { "types", new WorldObjectType[] { WorldObjectType.Food } },
                    { "clear", true }
                });

                // Orthographic size is 1/2 the vertical size seen by the camera
                // Sets the camera to be sized around the sprite bounds
                Bounds tableBounds = _diningTable.GetComponentInChildren<SpriteRenderer>().sprite.bounds;
                float cameraSize = tableBounds.size.y * Screen.height / Screen.width;
                camera.orthographicSize = cameraSize;

                // Move camera to dining table, wherever it is
                Vector3 tablePos = _diningTable.position;
                Vector3 camPos = cameraTransform.position;
                camPos.x = tablePos.x;
                camPos.y = tablePos.y;
                cameraTransform.position = camPos;

                // Move Food
                Vector3 foodPos = _foodContainer.position;
                foodPos.x = tablePos.x;
                foodPos.y = tablePos.y;
                _foodContainer.position = foodPos;
                // Show food
                _foodGameObj.SetActive(true);
            break;

            case GamePhaseName.CatChase:


                // Set camera size
                SetCameraToRoomView();

                break;
        }
    }

    void PlaceWorldObjects()
    {
        BoxCollider2D newObject;
        // Instantiate & place objects for decorating phase
        foreach (GameObject worldObj in _worldObjects)
        {
            SetRandomPos();
            newObject = Instantiate(worldObj, _randPos, Quaternion.identity, _draggablesContainer).GetComponent<BoxCollider2D>();

            if (!WorldObjectUtils.CanPlace(newObject))
            {
                Debug.Log("Placing again");
                SetRandomPos();
                newObject.gameObject.transform.position = _randPos;
                Debug.Log("Good place: " + WorldObjectUtils.CanPlace(newObject).ToString());
            }
        }
    }

    void SetRandomPos()
    {
        _randPos = WorldObjectUtils.GetRandomPos(_wallTiles.cellBounds, _randPos);
        _randPos.x = Mathf.RoundToInt(_randPos.x);
        _randPos.y = Mathf.RoundToInt(_randPos.y);
    }
    #endregion
}
