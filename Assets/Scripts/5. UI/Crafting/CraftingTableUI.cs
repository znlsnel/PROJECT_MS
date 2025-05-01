using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingTableUI : PopupUI
{
    public static Action onShow;

    [SerializeField] private Transform itemSlotRoot;
    [SerializeField] private GameObject slotPrefab;
    private List<CraftingSlotUI> slots = new List<CraftingSlotUI>();

    protected override void Awake()
    {
        base.Awake();
        Managers.SubscribeToInit(Setup);
    }

    public override void Show()
    {
        base.Show();
        onShow?.Invoke(); 
    }

    private void Setup()
    {
        List<CraftingData> craftings = Managers.Data.craftings.GetAll();
        
        for (int i = 0; i < craftings.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab, itemSlotRoot, false);
            CraftingSlotUI slotUI = slot.GetComponent<CraftingSlotUI>();
            slotUI.Setup(craftings[i]);
            slots.Add(slotUI); 
        }
    }
}
