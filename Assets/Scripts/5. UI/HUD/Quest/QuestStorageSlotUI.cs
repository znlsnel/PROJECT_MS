using System;
using TMPro;
using UnityEngine;

public class QuestStorageSlotUI : ItemSlotUI
{
    [SerializeField] private TextMeshProUGUI maxAmountText;

    public override void UpdateSlotState(ItemSlot itemSlot)
    {
        base.UpdateSlotState(itemSlot);

        itemAmountText.gameObject.SetActive(true);
        itemAmountText.text = itemSlot.Stack.ToString(); 
        maxAmountText.text = itemSlot.MaxStack.ToString();
    }

    protected override void ClickAction()
    {
        ItemDragHandler.MoveItem(this);
    }
}
