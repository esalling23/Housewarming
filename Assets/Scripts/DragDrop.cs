using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
    bool _isDragging = false;
    Transform _transform;
    Camera _mainCamera;
    BoxCollider2D _collider;
    Vector2 _originalPos;

    void Awake()
    {
        _transform = transform;
        _mainCamera = Camera.main;
        _collider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (_isDragging)
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
        }

        Debug.Log("OnMouseUp");
        _isDragging = false;
        _collider.isTrigger = false;
    }

    void OnMouseDown()
    {
        _originalPos = new Vector2(_transform.position.x, _transform.position.y);

        Debug.Log("OnMouseDown");
        _isDragging = true;
        _collider.isTrigger = true;
    }
}
