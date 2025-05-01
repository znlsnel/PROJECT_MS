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
        if (requireItem == null)
            return;
             
        this.requireItem = requireItem;
        itemIcon.sprite = requireItem.itemData.Icon;
        itemAmount.text = requireItem.amount.ToString();  
    } 
}
