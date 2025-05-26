using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSlotUI))]
public class ItemSlotBackGroundComponent : MonoBehaviour
{
    [SerializeField] private Image background;
    public static Dictionary<EItemType, Color> colors = new Dictionary<EItemType, Color>()
    {
        {EItemType.None, MyColor.Gray},
        {EItemType.Weapon, MyColor.Red},
        {EItemType.Equippable, MyColor.Blue},
        {EItemType.Consumable, MyColor.Orange},
        {EItemType.Resource, MyColor.Brown}, 
        {EItemType.Placeable, MyColor.Purple}, 
    };

    private void Awake()
    {
        GetComponent<ItemSlotUI>().onUpdate += UpdateBackground;
    }

    public void UpdateBackground(ItemSlot itemSlot)
    {
        UpdateBackground(itemSlot.Data);
    }

    public void UpdateBackground(ItemData itemData)
    {
        EItemType type = itemData == null ? EItemType.None : itemData.ItemType;
        background.color = colors[type];
    }
}   
