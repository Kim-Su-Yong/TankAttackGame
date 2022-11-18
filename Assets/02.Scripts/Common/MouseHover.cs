using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MouseHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static MouseHover mouseHover;
    public bool isHover = false;
    void Awake()
    {
        mouseHover = this;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHover = true;   
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHover = false;
    }
}
