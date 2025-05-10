using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;


public class InventorySlotUI : ItemSlotUI
{
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

        if (itemSlot.Data == null)
            return;


        itemIcon.sprite = itemSlot.Data.Icon; 
        itemAmountText.text = itemSlot.Stack.ToString(); 


        itemIcon.transform.DOScale(1.4f, 0.1f).OnComplete(()=>
        {
            itemIcon.transform.DOScale(1f, 0.1f);  
        });
    }

    protected override void ClickAction()
    {
        ItemDragHandler.SelectItemSlot(this); 
    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }
}
