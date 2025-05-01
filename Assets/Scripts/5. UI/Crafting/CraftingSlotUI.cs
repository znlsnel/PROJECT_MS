using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameData;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CraftingSlotUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image background_active;
    [SerializeField] private Image background_inactive;
    [SerializeField] private Image itemIcon;

    private CraftingData data;


    public void Setup(CraftingData data)
    {
        this.data = data;
        itemIcon.sprite = data.itemData.Icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingUIHandler.ClickCraftingSlot(data); 
    }


}
