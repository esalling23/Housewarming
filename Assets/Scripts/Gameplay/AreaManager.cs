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

    AreaPhase _currentArea;
    // Various area game objects to be loaded in
    [SerializeField] GameObject _areaView;
    Transform _draggablesContainer;
    // Used for Food phase
    GameObject _diningTable;
    // Used to determine initial random placements of world objects
    Tilemap _wallTiles;
    [SerializeField] GameObject[] _worldObjects;
    [SerializeField] GameObject[] _plateObjects;
    Vector3 _randPos = new Vector3();

    #endregion

    #region Methods

    void Awake()
    {
        _areaView = GameObject.FindWithTag("AreaView");
        _diningTable = GameObject.FindWithTag("DiningTable");
        _draggablesContainer = GameObject.FindWithTag("Draggables").transform;
        _wallTiles = GameObject.FindWithTag("Walls").GetComponent<Tilemap>();
    }

    void Start()
    {
        EventManager.StartListening(EventName.StartArea, StartArea);
    }

    /// <summary>
    /// Event handler - manages setting up area view for new phase
    /// </summary>
    /// <param name="msg">null</param>
    void StartArea(Dictionary<string, object> msg)
    {
        Camera camera = GameManager.Instance.MainCamera;
        switch (GameManager.Instance.CurrentPhase.phase)
        {
            case GamePhaseName.Decorating:

                // Maybe: random placement
                // PlaceWorldObjects();
                EventManager.TriggerEvent(EventName.EnableObjectSelect, new Dictionary<string, object>
                {
                    { "types", new WorldObjectType[] { WorldObjectType.Sprite, WorldObjectType.Tile } },
                    { "clear", true }
                });

            break;

            case GamePhaseName.Food:

                EventManager.TriggerEvent(EventName.EnableObjectSelect, new Dictionary<string, object>
                {
                    { "types", new WorldObjectType[] { WorldObjectType.Food } },
                    { "clear", true }
                });
                // Move camera to dining table, wherever it is
                Bounds tableBounds = _diningTable.GetComponentInChildren<SpriteRenderer>().sprite.bounds;
                // Orthographic size is 1/2 the vertical size seen by the camera
                // Sets the camera to be sized around the sprite bounds
                float cameraSize = tableBounds.size.y * Screen.height / Screen.width;
                camera.orthographicSize = cameraSize;

                camera.transform.LookAt(_diningTable.transform.position);

                // Generate plates

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
