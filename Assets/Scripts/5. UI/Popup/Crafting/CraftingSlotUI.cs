using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;




public class CraftingSlotUI : InventorySlotUI
{
    private CraftingItemData data;

    public static event Action<CraftingItemData> onSlotClick;

    public override void Setup(ItemSlot itemSlot)
    {
        this.data = itemSlot.Data as CraftingItemData;
        itemIcon.sprite = data.Icon;

        Action onUpdate = () => UpdateSlotState(itemSlot);
        InventoryDataHandler.onItemAmountUpdate -= onUpdate;
        InventoryDataHandler.onItemAmountUpdate += onUpdate; 
        
        onUpdate?.Invoke();
    }

    public override void UpdateSlotState(ItemSlot itemSlot)
    {
        onUpdate?.Invoke(data);
        bool flag = true;
        for (int i = 0; i < data.requiredStorage.Count; i++)
        {
            if (data.requiredStorage.GetSlotByIdx(i) == null)
                continue;

            if (InventoryDataHandler.GetItemAmount(data.requiredStorage.GetSlotByIdx(i).Data.Id) < data.requiredStorage.GetSlotByIdx(i).Stack)
            {
                flag = false;
                break;
            }
        } 

        itemIcon.color = flag ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f); 

    }

    protected override void ClickAction()
    {
        onSlotClick?.Invoke(data);  
    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }
}
