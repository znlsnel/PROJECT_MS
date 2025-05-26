using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class RequiredItemSlotUI : ItemSlotUI
{
    [SerializeField] private Outline outline;

    public override void Setup(ItemSlot itemSlot)
    {
        base.Setup(itemSlot);
        ItemSlot = itemSlot;

        Action onUpdate = ()=>UpdateSlotState(itemSlot);

        InventoryDataHandler.onItemAmountUpdate -= onUpdate;
        InventoryDataHandler.onItemAmountUpdate += onUpdate; 
        onUpdate.Invoke();

        onSelect?.Invoke(false); 
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

        onUpdate?.Invoke(itemSlot.Data);
 
        itemIcon.transform.DOScale(1.4f, 0.1f).OnComplete(()=>
        {
            itemIcon.transform.DOScale(1f, 0.1f);  
        });
    }

    protected override void ClickAction()
    {

    }



}
