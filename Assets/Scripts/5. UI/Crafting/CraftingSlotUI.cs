using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CraftingSlotUI : ItemSlotUI
{
    private CraftingData data;

    public void Setup(CraftingData data)
    {
        this.data = data;
        itemIcon.sprite = data.itemData.Icon;
        InventoryDataHandler.onItemAmountUpdate -= UpdateSlotState;
        InventoryDataHandler.onItemAmountUpdate += UpdateSlotState; 
        UpdateSlotState();
    }

    private void UpdateSlotState()
    {
        onUpdate?.Invoke(data.itemData);
        bool flag = true;
        for (int i = 0; i < data.requiredItems.Length; i++)
        {
            if (data.requiredItems[i] == null)
                continue;

            if (InventoryDataHandler.GetItemAmount(data.requiredItems[i].itemData.Id) < data.requiredItems[i].amount)
            {
                flag = false;
                break;
            }
        } 

        itemIcon.color = flag ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f); 

    }

    protected override void ClickAction()
    {
        CraftingHandler.ClickCraftingSlot(data); 
    }

    protected override void MouseHoverAction(bool isHover)
    {
        
    }
}
