using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;


public class CraftingRequiredItemUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemAmount;

    private RequireItem requireItem;
    private Outline outline;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Setup(RequireItem requireItem)
    {
        this.requireItem = requireItem;
        if (requireItem == null)
            return;
             
        InventoryDataHandler.onItemAmountUpdate -= UpdateAmount;
        InventoryDataHandler.onItemAmountUpdate += UpdateAmount; 
        itemIcon.sprite = requireItem.itemData.Icon;
        UpdateAmount();
    } 

    private void UpdateAmount()
    {
        int amount = InventoryDataHandler.GetItemAmount(requireItem.itemData.Id);
        itemAmount.text = $"{Math.Min(amount, requireItem.amount)} / {requireItem.amount}";

        if (amount >= requireItem.amount)
            outline.effectColor = Color.green;
        else
            outline.effectColor = Color.red; 
    }

}
