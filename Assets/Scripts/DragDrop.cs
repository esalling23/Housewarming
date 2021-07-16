using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    #region Fields
    // Caching built-ins
    Transform _transform;
    Camera _mainCamera;

    // Drag support
    bool _isDragging = false;
    Vector2 _originalPos;

    // Drag/hover rotation pivot support
    bool _isHoverRotated = false;
    int _rotateAngle = 15;
    BoxCollider2D _collider;
    float _halfHeight;
    float _halfWidth;
    [SerializeField] GameObject _childObj;
    Transform _childTransform;
    Vector3 _rotatePivot;

    // Color change support
    SpriteRenderer _childSprite;
    Color _hoverColor = new Color(0.9f, 0.9f, 0.9f, 1);

    #endregion

    #region Properties

    Vector3 RotationPivot
    {
        get
        {
            _rotatePivot.x = transform.position.x - _halfWidth;
            _rotatePivot.y = transform.position.y - _halfHeight;
            return _rotatePivot;
        }
    }

    bool ShouldHandleMouseExit
    {
        get
        {
            return _isHoverRotated && !IsDragging;
        }
    }

    bool IsDragging { get { return _isDragging; } }

    #endregion

    #region Methods

    void Awake()
    {
        _transform = transform;
        _mainCamera = Camera.main;
        _collider = GetComponent<BoxCollider2D>();

        _childTransform = _childObj.transform;
        _childSprite = _childObj.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _halfWidth = _collider.size.x / 2;
        _halfHeight = _collider.size.y / 2;
    }

    void Update()
    {
        if (IsDragging)
        {
            Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
            // Rounding position forces grid-snap
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);

            _transform.Translate(mousePos);
        }
    }

    bool CanPlace()
    {
        Vector3 borderVector = new Vector3(0.1f, 0.1f, 0);
        Collider2D[] overlap = Physics2D.OverlapAreaAll(_collider.bounds.min + borderVector, _collider.bounds.max - borderVector);
        if (overlap.Length > 1)
        {
            Debug.Log("Cannot Place");
            Debug.Log(string.Format("Found {0} overlapping object(s)", overlap.Length - 1));
            // Check immediately available spots around element
            return false;
        }

        return true;
    }

    void OnMouseUp()
    {
        if (!CanPlace())
        {
            _transform.position = _originalPos;
            HandleMouseExit();
        }

        if (ShouldHandleMouseExit)
        {
            HandleMouseExit();
        }

        _isDragging = false;
        _collider.isTrigger = false;
    }

    void OnMouseDown()
    {
        _originalPos = new Vector2(_transform.position.x, _transform.position.y);

        _isDragging = true;
        _collider.isTrigger = true;
    }

    void OnMouseExit()
    {
        if (ShouldHandleMouseExit)
        {
            HandleMouseExit();
        }
    }

    void OnMouseEnter()
    {
        if (!IsDragging)
        {
            HandleMouseEnter();
        }
    }

    void HandleMouseEnter()
    {
        _isHoverRotated = true;
        _childTransform.RotateAround(RotationPivot, Vector3.forward, _rotateAngle);

        _childSprite.color = _hoverColor;
    }

    void HandleMouseExit()
    {
        Debug.Log("Handling Exit");
        _isHoverRotated = false;
        // Reset rotate
        _childTransform.RotateAround(RotationPivot, Vector3.forward, -_rotateAngle);

        // Reset color
        _childSprite.color = Color.white;
    }
    #endregion
}
