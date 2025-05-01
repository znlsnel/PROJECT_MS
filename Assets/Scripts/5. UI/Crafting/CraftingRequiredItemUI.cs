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
    [SerializeField] private Image outline;
    [SerializeField] private TextMeshProUGUI itemAmount;

    private RequireItem requireItem;


    public void Setup(RequireItem requireItem)
    {
        this.requireItem = requireItem;
        if (requireItem == null)
            return;
             
        Managers.UserData.Inventory.onItemAmountUpdate -= UpdateAmount;
        Managers.UserData.Inventory.onItemAmountUpdate += UpdateAmount; 
        itemIcon.sprite = requireItem.itemData.Icon;
        UpdateAmount();
    } 

    private void UpdateAmount()
    {
        itemAmount.text = Managers.UserData.Inventory.GetItemAmount(requireItem.itemData.Id).ToString();
        itemAmount.text += $" / {requireItem.amount}";
    }

}
