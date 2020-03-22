using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverResponse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BuyMenu Menu;
    public PauseMenu PauseMenu;
    public int Data;

    private bool mouseover = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseover = true;
        Menu?.Hover(Data);
        PauseMenu?.Hover(Data);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseover = false;
        Menu?.UnHover(Data);
        PauseMenu?.UnHover(Data);
    }
}
