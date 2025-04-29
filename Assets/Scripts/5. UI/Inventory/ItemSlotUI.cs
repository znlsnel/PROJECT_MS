using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
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
    public ItemSlot ItemSlot {get; private set;}
    
    public Action<ItemSlot> onSetup;



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
        ItemSlot = itemSlot;
        ItemSlot.onChangeStack += UpdateItemSlotUI; 
        UpdateItemSlotUI(itemSlot);
        onSetup?.Invoke(itemSlot);
    }

    public void UnSetup()
    {
        if (ItemSlot != null && ItemSlot.onChangeStack != null)
            ItemSlot.onChangeStack -= UpdateItemSlotUI;
        ItemSlot = null;
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


        itemIcon.transform.DOScale(1.4f, 0.1f).OnComplete(()=>
        {
            itemIcon.transform.DOScale(1f, 0.1f);  
        });
    }


    private void SetBackground(EItemType itemType)
    {
        background.color = colors[itemType];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ItemDragHandler.SelectItemSlot(this); 
    }
}
