using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CraftingTableUI : PopupUI
{
    [SerializeField] private CloseButton closeButton;
    [SerializeField] private Transform itemSlotRoot;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject emptyStatePanel;
    [SerializeField] private GameObject mainPanel;

    private List<CraftingSlotUI> slots = new List<CraftingSlotUI>();

    protected override void Awake()
    {
        base.Awake();
        closeButton.OnClick += () => HideWithDoTween(mainPanel.transform);  
        Managers.SubscribeToInit(Setup);
    }

    public override void Show()
    {
        base.Show();
        emptyStatePanel.SetActive(true);
    }

    private void Setup()
    {
        List<CraftingItemData> craftings = Managers.Data.craftings.GetAll();
        
        for (int i = 0; i < craftings.Count; i++) 
        {
            GameObject slot = Instantiate(slotPrefab, itemSlotRoot, false);
            CraftingSlotUI slotUI = slot.GetComponent<CraftingSlotUI>();

            ItemSlot itemSlot = new ItemSlot();
            itemSlot.Setup(craftings[i]);

            slotUI.Setup(itemSlot);
            slots.Add(slotUI); 
        }
    }
}

