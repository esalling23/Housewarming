using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class WorldObject : MonoBehaviour
{
    #region Fields

    // Object details for display
    [SerializeField] string _name;

    // Cycle style support
    [SerializeField] List<Sprite> _spriteStyles;
    [SerializeField] SpriteRenderer _childSprite;
    bool _isTilemap = false;
    Tilemap _tilemap;
    TileManager _tileManager;
    int _selectedStyleIndex;

    #endregion

    #region Properties

    public int SelectedStyleIndex
    {
        get { return _selectedStyleIndex; }
    }

    public bool IsTilemap
    {
        get { return _isTilemap; }
    }
    #endregion

    #region Methods

    void Awake()
    {
        _tilemap = GetComponent<Tilemap>();

        Debug.Log(_tilemap);

        if (_tilemap != null)
        {
            _isTilemap = true;

            _tileManager = GetComponent<TileManager>();

            if (!_tileManager)
            {
                Debug.LogError("WorldObject with Tilemap child must have TileManager component");
            }
        }
        else
        {
            if (!_childSprite || _spriteStyles.Count == 0)
            {
                Debug.LogError("WorldObject not set up properly. Check child sprite and sprite list.");
            }
            _childSprite.sprite = _spriteStyles[0];
        }
    }

    public void CycleStyle()
    {
        int limit = 0;

        if (_isTilemap)
        {
            limit = _tileManager.CycleLimit;
        }
        else if (_spriteStyles.Count == 0)
        {
            return;
        }
        else
        {
            limit = _spriteStyles.Count - 1;
        }

        Debug.Log($"Style cycle limit is: {limit}");

        if (_selectedStyleIndex < limit)
        {
            _selectedStyleIndex++;
        }
        else if (_selectedStyleIndex >= limit)
        {
            _selectedStyleIndex = 0;
        }

        Debug.Log($"Selected style index is: {_selectedStyleIndex}");

        if (_isTilemap)
        {
            _tilemap.RefreshAllTiles();
        }
        else
        {
            _childSprite.sprite = _spriteStyles[_selectedStyleIndex];
        }

    }

    void OnMouseDown() {
        // Ignore UI elements
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EventManager.TriggerEvent(EventName.SelectObject, new Dictionary<string, object> {
                { "name", _name },
                { "obj", this }
            });
        }
    }

    #endregion
}
