using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWorldObject : WorldObject
{
    #region Fields
    // All sprites should be the same size
    [SerializeField] Sprite[] _spriteStyles;
    [SerializeField] SpriteRenderer _childSprite;
    [SerializeField] Vector2 _spriteColliderOffset = Vector2.zero;
    BoxCollider2D _boxCollider;
    Transform _transform;

    #endregion

    #region Methods
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _transform = gameObject.transform;

        if (!_childSprite || _spriteStyles.Length == 0)
        {
            Debug.LogError("WorldObject not set up properly. Check child sprite and sprite list.");
        }
        else if (_boxCollider == null)
        {
            Debug.LogError("WorldObject not set up properly. Object should have BoxCollider2D component");
        }
    }

    void Start()
    {
        _selectedStyleIndex = 0;
        SetSprite();
        SetColliderToSpriteSize();
    }

    /// <summary>
    /// Changes the visual of the object based on different styles
    /// </summary>
    public override void CycleStyle()
    {
        if (_spriteStyles.Length == 0)
        {
            Debug.Log("No sprites to cycle");
            return;
        }
        _selectedStyleIndex = WorldObjectUtils.GetNextStyleIndex(_selectedStyleIndex, _spriteStyles.Length - 1);
        SetSprite();
    }

    void SetSprite()
    {
        _childSprite.sprite = _spriteStyles[_selectedStyleIndex];
    }

    void SetColliderToSpriteSize()
    {
        Vector3 spriteBounds = _childSprite.bounds.size;
        _boxCollider.size = spriteBounds;
        _boxCollider.offset = _spriteColliderOffset;
    }

    #endregion
}
