using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject ImagePrefab;
    public InventoryObject _inventory;
    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEMS;
    Dictionary<InventorySlot, GameObject> itemDisplayed = new Dictionary<InventorySlot, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        for(int i = 0; i < _inventory.Container.Count; ++i)
        {
            if (itemDisplayed.ContainsKey(_inventory.Container[i]) == false)
            {
                Sprite sprite = _inventory.Container[i].item.DefaultSprite;
                var obj = Instantiate(ImagePrefab, Vector3.zero, Quaternion.identity, transform);
                Image img = obj.GetComponent<Image>();
                img.sprite = sprite;
                img.preserveAspect = true;
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                itemDisplayed.Add(_inventory.Container[i], obj);
            }
        }
    }

    public void CreateDisplay()
    {
        for(int i = 0; i < _inventory.Container.Count; i++)
        {
            Sprite sprite = _inventory.Container[i].item.DefaultSprite;
            var obj = Instantiate(ImagePrefab, Vector3.zero, Quaternion.identity, transform);
            Image img = obj.GetComponent<Image>();
            img.sprite = sprite;
            img.preserveAspect = true;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + ((-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN))), 0f);
    }
}
