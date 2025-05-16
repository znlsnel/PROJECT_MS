using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class QuickSlotUI : MonoBehaviour
{
    [SerializeField] private Transform quickSlotRoot;

    private Dictionary<ItemSlot, ItemSlotUI> quickSlotEffects = new Dictionary<ItemSlot, ItemSlotUI>();

    private ItemSlot selectedItemSlot;

    private void Awake()
    {
        InventorySlotUI[] itemSlotUIs = quickSlotRoot.GetComponentsInChildren<InventorySlotUI>();
        
        for (int i = 0; i < itemSlotUIs.Length; i++)
        {
            ItemSlot itemSlot = Managers.UserData.Inventory.QuickSlotStorage.GetSlotByIdx(i);
            itemSlotUIs[i].Setup(itemSlot);

            quickSlotEffects.Add(itemSlot, itemSlotUIs[i]);
        }

        QuickSlotHandler.onSelectItem += SelectSlot;
    }

    private void SelectSlot(ItemSlot itemSlot)
    {
        if (selectedItemSlot == itemSlot)
            return;

        if (selectedItemSlot != null)
        quickSlotEffects[selectedItemSlot].onSelect?.Invoke(false);
        quickSlotEffects[itemSlot].onSelect?.Invoke(true);
        selectedItemSlot = itemSlot;
    }

    private void OnDestroy()
    {
        QuickSlotHandler.onSelectItem -= SelectSlot;
    }
}
