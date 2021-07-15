using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour
{
    // Rotation support on hover
    [SerializeField] GameObject _childSprite;
    Transform _childTransform;
    int _rotateAngle = 15;
    Vector3 _rotatePivot;
    // Used in determining rotation pivot
    BoxCollider2D _collider;
    float _halfHeight;
    float _halfWidth;
    SpriteRenderer _spriteRenderer;


    void Awake()
    {
        _spriteRenderer = _childSprite.GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
        _childTransform = _childSprite.transform;
    }

    void Start()
    {
        _halfWidth = _collider.size.x / 2;
        _halfHeight = _collider.size.y / 2;
    }
    void OnMouseEnter()
    {
        Debug.Log("Mouse entering");

        _rotatePivot = new Vector3(_childTransform.position.x - _halfWidth, _childTransform.position.y - _halfHeight, 0);
        _spriteRenderer.color = new Color(0.9f, 0.9f, 0.9f, 1);
        _childTransform.RotateAround(_rotatePivot, Vector3.forward, _rotateAngle);
    }

    void OnMouseExit()
    {
        _spriteRenderer.color = Color.white;
        _childTransform.RotateAround(_rotatePivot, Vector3.forward, -_rotateAngle);
    }

    void OnMouseDown()
    {
        _childTransform.Rotate(0, 0, 0.25f);
    }
}
