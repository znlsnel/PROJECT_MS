using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemAmountText;

    public Action<ItemData> onUpdate;
    public Action<bool> onSelect;

    protected abstract void ClickAction();
    protected abstract void MouseHoverAction(bool isHover);


    public void OnPointerClick(PointerEventData eventData)
    {
        ClickAction();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseHoverAction(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseHoverAction(false);
    }
}
