using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages the area view such as set up for a new phase area
/// </summary>
public class AreaManager : MonoBehaviour
{
    #region Fields
    [SerializeField] AreaHUD _areaHUD;

    // Various area game objects to be loaded in
    [SerializeField] Transform _draggablesContainer;

    // Food phase support
    [SerializeField] Transform _diningTable;
    [SerializeField] Transform _foodContainer;
    GameObject _foodGameObj;

    // Cat Chase phase support
    private int _catFoundCount = 0;
    [SerializeField] private int _catFoundLimit = 2;
    IWorldObject[] _hidingSpots;

    // Cache camera data
    Transform _cameraTransform;
    float _cameraRoomViewSize;

    // Vector3 _randPos = new Vector3();

    #endregion

    #region Methods

    void Awake()
    {
        _foodGameObj = _foodContainer.gameObject;
        _hidingSpots = WorldObjectManager.Instance.Furniture;
    }

    void Start()
    {
        EventManager.StartListening(EventName.StartArea, HandleStartAreaEvent);
        EventManager.StartListening(EventName.CatFound, HandleCatFoundEvent);
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

    void HandleCatFoundEvent(Dictionary<string, object> msg)
    {
        IWorldObject hidingSpot = (IWorldObject) msg["worldObject"];
        _catFoundCount++;

        if (_catFoundCount >= _catFoundLimit)
        {
            _areaHUD.ScreenFlash(() =>
            {
                EventManager.TriggerEvent(EventName.CompleteArea, null);
            });
        }
        else
        {
            _hidingSpots = _hidingSpots
                .Where(o => !Object.ReferenceEquals(o, hidingSpot))
                .ToArray();

            // Find another cat!
            HideCats(hidingSpot.GameObject.GetComponent<CatHiding>());
        }
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

                Vector3 tablePos = _diningTable.position;

                // Move camera to dining table, wherever it is
                _cameraTransform.position = TransformUtils.SetXYPosition(_cameraTransform.position, tablePos);
                // Move Food
                _foodContainer.position = TransformUtils.SetXYPosition(_foodContainer.position, tablePos);
                // Show food
                _foodGameObj.SetActive(true);
            break;

            case GamePhaseName.CatChase:

                // Choose a random sprite world object
                // & give it the CatHiding component
                HideCats(null);

                // Set camera size
                SetCameraToRoomView();

                break;
        }
    }

    void HideCats(CatHiding oldSpot)
    {
        int numSpots = _hidingSpots.Length;

        if (numSpots == 0)
        {
            Debug.LogError("No furniture in scene to hide in");
        }

        int rand = Random.Range(0, numSpots);

        IWorldObject newSpot = _hidingSpots[rand];

        Debug.Log(newSpot);

        _areaHUD.ScreenFlash(() =>
        {
            if (oldSpot != null)
            {
                oldSpot.Clear();
            }
            newSpot.GameObject.AddComponent<CatHiding>();
        });
    }

    // void PlaceWorldObjects()
    // {
    //     BoxCollider2D newObject;
    //     // Instantiate & place objects for decorating phase
    //     foreach (GameObject worldObj in _worldObjects)
    //     {
    //         SetRandomPos();
    //         newObject = Instantiate(worldObj, _randPos, Quaternion.identity, _draggablesContainer).GetComponent<BoxCollider2D>();

    //         if (!WorldObjectUtils.CanPlace(newObject))
    //         {
    //             Debug.Log("Placing again");
    //             SetRandomPos();
    //             newObject.gameObject.transform.position = _randPos;
    //             Debug.Log("Good place: " + WorldObjectUtils.CanPlace(newObject).ToString());
    //         }
    //     }
    // }

    // void SetRandomPos()
    // {
    //     _randPos = WorldObjectUtils.GetRandomPos(_wallTiles.cellBounds, _randPos);
    //     _randPos.x = Mathf.RoundToInt(_randPos.x);
    //     _randPos.y = Mathf.RoundToInt(_randPos.y);
    // }
    #endregion
}
