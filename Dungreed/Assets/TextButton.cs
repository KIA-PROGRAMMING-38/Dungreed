using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TextButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler
{
    public TextMeshProUGUI tmp;

    public Color DefaultColor;
    public Color HighlightColor;
    public UnityEvent OnClick;

    private void Awake()
    {
        if(tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }
        tmp.color = DefaultColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        OnClick?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.color = HighlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.color = DefaultColor;
    }
}
