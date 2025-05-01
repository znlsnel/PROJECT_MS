using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CraftingSelectedUI : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private Transform requiredItemRoot;

    private CraftingData data;
    private List<CraftingRequiredItemUI> requiredItems;
    private void Awake()
    {
        CraftingUIHandler.onSlotClick += Setup;

        requiredItems = requiredItemRoot.GetComponentsInChildren<CraftingRequiredItemUI>(true).ToList();
    }

    private void Setup(CraftingData data)
    {
        this.data = data;
        itemIcon.sprite = data.itemData.Icon;
        itemName.text = data.itemData.Name;
        itemDescription.text = data.itemData.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingUIHandler.ClickCrafting(data);
    }
}
