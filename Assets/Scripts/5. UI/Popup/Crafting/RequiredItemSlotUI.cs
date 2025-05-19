using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class RequiredItemSlotUI : InventorySlotUI
{
    [SerializeField] private Outline outline;

    private ItemSlot itemSlot;

    public override void Setup(ItemSlot itemSlot)
    {
        base.Setup(itemSlot);
        this.itemSlot = itemSlot;

        Action onUpdate = ()=>UpdateSlotState(itemSlot);

        InventoryDataHandler.onItemAmountUpdate -= onUpdate;
        InventoryDataHandler.onItemAmountUpdate += onUpdate; 
        onUpdate.Invoke();
    } 

    public override void UpdateSlotState(ItemSlot itemSlot)
    {
        itemIcon.sprite = itemSlot.Data.Icon;

        int amount = InventoryDataHandler.GetItemAmount(itemSlot.Data.Id);
        itemAmountText.text = $"{Math.Min(amount, itemSlot.Stack)} / {itemSlot.Stack}";

        if (amount >= itemSlot.Stack)
            outline.effectColor = Color.green;
        else
            outline.effectColor = Color.red; 
    }

    protected override void ClickAction()
    {

    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }

}
