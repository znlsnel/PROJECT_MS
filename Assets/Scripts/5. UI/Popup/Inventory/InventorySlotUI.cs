using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlotUI : ItemSlotUI
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;


    public ItemSlot ItemSlot {get; protected set;}

    public Action<ItemSlot> onSetup;
    

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

    protected override void ClickAction()
    {
        if (!ItemDragHandler.SelectItemSlot(this))
            ItemDragHandler.SwapItem(this);
    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }
}
