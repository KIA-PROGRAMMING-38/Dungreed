using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] public KeyCode Key;
    [SerializeField] private WeaponData _data;
    public WeaponData Data { get { return _data; } }
    private Image _weaponIcon;
    private Image _slotImage;
    private Sprite _defaultImage;
    [field: SerializeField] public TextMeshProUGUI IndexText { get; private set; }
    [field: SerializeField] public TextMeshProUGUI AmmoText { get; private set; }
    [field: SerializeField] public Sprite SelectedImage { get; private set; }

    private void Awake()
    {
        _weaponIcon = transform.GetChild(0).GetComponent<Image>();
        _weaponIcon.preserveAspect = true;

        _slotImage = GetComponent<Image>();
        _defaultImage = _slotImage.sprite;

        _weaponIcon.sprite = _data.DefaultSprite;
    }

    public void Select()
    {
        _slotImage.sprite = SelectedImage;
    }

    public void UnSelect()
    {
        _slotImage.sprite = _defaultImage;
    }

}
