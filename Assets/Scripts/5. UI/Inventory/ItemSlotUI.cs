using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] private Color defaultBackground;
    [SerializeField] private Color weaponBackground;
    [SerializeField] private Color equipmentBackground;
    [SerializeField] private Color consumableBackground;
    [SerializeField] private Color resourceBackground;
    [SerializeField] private Color placeableBackground;

    [Header("Info")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI itemAmountText;

    private Dictionary<EItemType, Color> colors;

    void Awake()
    {
        colors = new Dictionary<EItemType, Color>()
        {
            {EItemType.None, defaultBackground},
            {EItemType.Weapon, weaponBackground},
            {EItemType.Equippable, equipmentBackground},
            {EItemType.Consumable, consumableBackground},
            {EItemType.Resource, resourceBackground},
            {EItemType.Placeable, placeableBackground},
        };
    }
    
    public void Setup(ItemSlot itemSlot)
    {
        itemSlot.onChangeStack += UpdateItemSlotUI; 
        UpdateItemSlotUI(itemSlot);
    }

    public void UpdateItemSlotUI(ItemSlot itemSlot)
    {

        itemIcon.gameObject.SetActive(itemSlot.Data != null);
        itemAmountText.gameObject.SetActive(itemSlot.Data != null && itemSlot.Data.CanStack);

        if (itemSlot.Data == null)
        {
            SetBackground(EItemType.None);
            return;
        }

        itemIcon.sprite = itemSlot.Data.Icon; 
        itemAmountText.text = itemSlot.Stack.ToString(); 

        EItemType type = EItemType.None;

        if (itemSlot.Data != null)
            type = itemSlot.Data.ItemType;
        
        SetBackground(type);
    }


    private void SetBackground(EItemType itemType)
    {
        background.color = colors[itemType];
    }
}
