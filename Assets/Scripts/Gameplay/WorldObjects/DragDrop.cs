using UnityEngine;

public class DragDrop : MonoBehaviour
{
    #region Fields
    // Caching built-ins
    Transform _transform;

    // Drag support
    bool _isDragging = false;
    Vector2 _originalPos;

    // Drag/hover rotation pivot support
    [SerializeField] GameObject _childObj;
    Transform _childTransform;
    bool _isHoverRotated = false;
    int _rotateAngle = 2;
    Vector3 _rotatePivot;
    BoxCollider2D _collider;
    float _halfHeight;
    float _halfWidth;

    // Color change support
    SpriteRenderer _childSprite;
    Color _hoverColor = new Color(0.9f, 0.9f, 0.9f, 1);

    #endregion

    #region Properties

    /// <summary>
    /// Provides pivot for rotation which will be the bottom left corner of the sprite
    /// </summary>
    /// <value>Vector3 of the bottom left corner location</value>
    Vector3 RotationPivot
    {
        get
        {
            _rotatePivot.x = transform.position.x - _halfWidth;
            _rotatePivot.y = transform.position.y - _halfHeight;
            return _rotatePivot;
        }
    }

    /// <summary>
    /// Determines if we should handle a mouse exit by resetting the rotation etc.
    /// </summary>
    /// <value>boolean based on if we're already rotated or dragging the element</value>
    bool ShouldHandleMouseExit
    {
        get
        {
            return !WorldObjectUtils.IsOverUI && _isHoverRotated && !IsDragging;
        }
    }

    /// <summary>
    /// Gets if we are dragging element
    /// </summary>
    /// <value></value>
    bool IsDragging { get { return _isDragging; } }

    /// <summary>
    /// Returns if script is disabled
    /// </summary>
    bool IsDisabled { get { return !this.enabled; } }

    #endregion

    #region Methods

    void Awake()
    {
        _transform = transform;
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
            Vector2 mousePos = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - _transform.position;
            // Rounding position forces grid-snap
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);

            _transform.Translate(mousePos);
        }
    }

    /// <summary>
    /// Handles user releasing mouse
    /// - Ends drag
    /// - Stops handling collision triggers
    /// </summary>
    void OnMouseUp()
    {
        if (IsDisabled) return;
        // Return object to original position if user tries to drop over
        // another object
        if (!WorldObjectUtils.CanPlace(_collider))
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


    /// <summary>
    /// Handles user clicking on object
    /// - Stores original position for return to that spot if we cannot place later on
    /// </summary>
    void OnMouseDown()
    {
        if (IsDisabled) return;

        if (!WorldObjectUtils.IsOverUI)
        {
            _originalPos = new Vector2(_transform.position.x, _transform.position.y);

            _isDragging = true;
            _collider.isTrigger = true;
        }
    }

    /// <summary>
    /// Handles user moving mouse away from object
    /// - Only do something if we aren't dragging
    /// </summary>
    void OnMouseExit()
    {
        if (IsDisabled) return;

        if (ShouldHandleMouseExit)
        {
            HandleMouseExit();
        }
    }

    /// <summary>
    /// Handles user moving mouse over object
    /// </summary>
    void OnMouseEnter()
    {
        if (IsDisabled) return;

        if (!WorldObjectUtils.IsOverUI && !IsDragging)
        {
            HandleMouseEnter();
        }
    }

    /// <summary>
    /// Logical steps for drag-hover effect
    /// </summary>
    void HandleMouseEnter()
    {
        _isHoverRotated = true;
        _childTransform.RotateAround(RotationPivot, Vector3.forward, _rotateAngle);

        _childSprite.color = _hoverColor;
    }

    /// <summary>
    /// Logical steps for removing drag-hover effect
    /// </summary>
    void HandleMouseExit()
    {
        _isHoverRotated = false;
        // Reset rotate
        _childTransform.RotateAround(RotationPivot, Vector3.forward, -_rotateAngle);

        // Reset color
        _childSprite.color = Color.white;
    }

    #endregion
}
