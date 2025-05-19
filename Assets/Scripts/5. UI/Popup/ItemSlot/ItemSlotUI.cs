using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemIcon;
    [SerializeField] protected TextMeshProUGUI itemAmountText;

    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;

    public Action<ItemSlot> onSetup;
    public Action<ItemData> onUpdate;
    public Action<bool> onSelect;

    public ItemSlot ItemSlot {get; protected set;}


    public virtual void Setup(ItemSlot itemSlot)
    {
        if (itemSlot == null)
            return;
 
        ItemSlot = itemSlot;
        ItemSlot.onChangeStack += UpdateSlotState; 
        UpdateSlotState(itemSlot);
        onSetup?.Invoke(itemSlot);
    }


    public virtual void UnSetup()
    {
        if (ItemSlot != null && ItemSlot.onChangeStack != null)
            ItemSlot.onChangeStack -= UpdateSlotState;
        ItemSlot = null;
    } 


    public virtual void UpdateSlotState(ItemSlot itemSlot)
    {
        onUpdate?.Invoke(itemSlot.Data);
        itemIcon.gameObject.SetActive(itemSlot.Data != null);
        itemAmountText.gameObject.SetActive(itemSlot.Data != null && itemSlot.Data.CanStack);

        if (_slider != null)
            _slider.gameObject.SetActive(itemSlot.Data != null && itemSlot.Data.HasDurability);

        if (itemSlot.Data == null)
            return;


        itemIcon.sprite = itemSlot.Data.Icon; 
        itemAmountText.text = itemSlot.Stack.ToString(); 

        if (_slider != null)
        {
            _slider.value = itemSlot.Durability / itemSlot.Data.MaxDurability;
            _sliderFill.color = Color.Lerp(MyColor.Red, MyColor.Green, _slider.value);
        }
        

        itemIcon.transform.DOScale(1.4f, 0.1f).OnComplete(()=>
        {
            itemIcon.transform.DOScale(1f, 0.1f);  
        });
    }


    protected virtual void ClickAction()
    {
        if (!ItemDragHandler.SelectItemSlot(this))
            ItemDragHandler.SwapItem(this);
    }

    protected virtual void MouseHoverAction(bool isHover)
    {

    }
    


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
