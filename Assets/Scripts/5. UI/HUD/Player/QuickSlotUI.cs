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
        Managers.onChangePlayer += Setup;
    }

    private void OnDestroy()
    {
        Managers.Player.QuickSlotHandler.onSelectItem -= SelectSlot;
        Managers.onChangePlayer -= Setup;
    }
    private void Setup(AlivePlayer player)
    {
        ItemSlotUI[] itemSlotUIs = quickSlotRoot.GetComponentsInChildren<ItemSlotUI>();
        
        for (int i = 0; i < itemSlotUIs.Length; i++)
        {
            ItemSlot itemSlot = player.Inventory.QuickSlotStorage.GetSlotByIdx(i);
            itemSlotUIs[i].Setup(itemSlot);

            quickSlotEffects.Add(itemSlot, itemSlotUIs[i]);
        }

        player.QuickSlotHandler.onSelectItem += SelectSlot;
    }



    private void SelectSlot(ItemSlot itemSlot, GameObject selectedItemObject)
    {
        if (selectedItemSlot == itemSlot)
            return;

        if (selectedItemSlot != null)
        quickSlotEffects[selectedItemSlot].onSelect?.Invoke(false);
        quickSlotEffects[itemSlot].onSelect?.Invoke(true);
        selectedItemSlot = itemSlot;
    }


}
