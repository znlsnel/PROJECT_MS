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
    private static readonly string _clickSound = "Sound/UI/Click_03.mp3";


    private CraftingItemData data;

    public static event Action<CraftingItemData> onSlotClick;

    public override void Setup(ItemSlot itemSlot)
    {
        ItemSlot = itemSlot; 
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

        int result = 100000;
        for (int i = 0; i < data.requiredStorage.Count; i++)
        {
            if (data.requiredStorage.GetSlotByIdx(i).Data == null) 
                continue;

            int amount = InventoryDataHandler.GetItemAmount(data.requiredStorage.GetSlotByIdx(i).Data.Id);
            int requiredAmount = data.requiredStorage.GetSlotByIdx(i).Stack;

            result = Math.Min(result, amount / requiredAmount);
            if (result == 0)
                break; 
        } 


        itemAmountText.text = result == 0 ? "-" : $"{result}";
        // itemIcon.color = Amount ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
    }

    protected override void ClickAction()
    {
        onSlotClick?.Invoke(data);  
        onSelect?.Invoke(false);
        Managers.Sound.Play(_clickSound);
    }


}
