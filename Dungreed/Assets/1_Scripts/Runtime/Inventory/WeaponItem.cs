using UnityEngine;

public class WeaponItem : Item
{
    public WeaponData _data;
    private SpriteRenderer _renderer;
    private BoxCollider2D _boxCollider;
    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_data != null)
        {
            _renderer.sprite = _data.DefaultSprite;
            _boxCollider.size = _renderer.bounds.size;
        }
    }
    public void SetWeaponData(WeaponData data)
    {
        _data = data;
        _renderer.sprite = _data.DefaultSprite;
        _boxCollider.size = _renderer.bounds.size;
    }

    public override void Pickup(PlayerData playerData)
    {
        playerData.Inventory.AddWeapon(_data);
    }
}
