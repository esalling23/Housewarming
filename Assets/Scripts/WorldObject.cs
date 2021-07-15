using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    #region Fields
    // Rotation support on hover
    Vector3 _startingPos;
    DragDrop _dragDrop;
    [SerializeField] GameObject _childObj;
    Transform _childTransform;
    int _rotateAngle = 15;
    Vector3 _rotatePivot;


    // Used in determining rotation pivot
    BoxCollider2D _collider;
    float _halfHeight;
    float _halfWidth;

    // Hover effect support
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

    bool IsNormalRotation
    {
        get
        {
            return _childTransform.localRotation.z == 0;
        }
    }

    bool ShouldHandleMouseEvent
    {
        get
        {
            return !IsNormalRotation && !_dragDrop.IsDragging;
        }
    }

    #endregion

    #region Methods
    void Awake()
    {
        _dragDrop = GetComponent<DragDrop>();
        _collider = GetComponent<BoxCollider2D>();

        _childTransform = _childObj.transform;
        _childSprite = _childObj.GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _startingPos = Vector3.zero;
        _halfWidth = _collider.size.x / 2;
        _halfHeight = _collider.size.y / 2;
    }

    void OnMouseEnter()
    {
        if (!_dragDrop.IsDragging)
        {
            HandleMouseEnter();
        }
    }

    void OnMouseExit()
    {
        if (ShouldHandleMouseEvent)
        {
            HandleMouseExit();
        }
    }

    void OnMouseUp()
    {
        if (ShouldHandleMouseEvent)
        {
            HandleMouseExit();
        }
    }

    void HandleMouseEnter()
    {
        _childTransform.RotateAround(RotationPivot, Vector3.forward, _rotateAngle);

        _childSprite.color = _hoverColor;
    }

    void HandleMouseExit()
    {
        // Reset rotate
        _childTransform.RotateAround(RotationPivot, Vector3.forward, -_rotateAngle);

        // Reset color
        _childSprite.color = Color.white;
    }

    #endregion
}
