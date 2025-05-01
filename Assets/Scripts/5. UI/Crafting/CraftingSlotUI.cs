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
        Managers.UserData.Inventory.onItemAmountUpdate -= UpdateState;
        Managers.UserData.Inventory.onItemAmountUpdate += UpdateState; 
        UpdateState();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingUIHandler.ClickCraftingSlot(data); 
    }

    private void UpdateState()
    {
        bool flag = true;
        for (int i = 0; i < data.requiredItems.Length; i++)
        {
            if (data.requiredItems[i] == null)
                continue;
                
            if (Managers.UserData.Inventory.GetItemAmount(data.requiredItems[i].itemData.Id) < data.requiredItems[i].amount)
            {
                flag = false;
                break;
            }
        }

        background_active.gameObject.SetActive(flag);
        background_inactive.gameObject.SetActive(!flag);
    }


}
