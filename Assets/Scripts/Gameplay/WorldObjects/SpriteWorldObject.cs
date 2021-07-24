using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWorldObject : WorldObject
{
    #region Fields
    [SerializeField] Sprite[] _spriteStyles;
    [SerializeField] SpriteRenderer _childSprite;
    [SerializeField] Vector2 _spriteColliderOffset = Vector2.zero;
    BoxCollider2D _boxCollider;

    #endregion

    #region Methods
    void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();

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
        _childSprite.sprite = _spriteStyles[0];

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
        WorldObjectUtils.GetNextStyleIndex(_selectedStyleIndex, _spriteStyles.Length - 1);
        _childSprite.sprite = _spriteStyles[_selectedStyleIndex];
        SetColliderToSpriteSize();
    }

    void SetColliderToSpriteSize()
    {
        Vector3 spriteBounds = _childSprite.bounds.size;
        _boxCollider.size = spriteBounds;
        _boxCollider.offset = _spriteColliderOffset;
    }

    #endregion
}
