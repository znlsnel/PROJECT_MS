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
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        CraftingUIHandler.onSlotClick += Setup;
        canvasGroup = GetComponent<CanvasGroup>();
        requiredItems = requiredItemRoot.GetComponentsInChildren<CraftingRequiredItemUI>(true).ToList(); 
    }

    private void Setup(CraftingData data)
    {
        this.data = data;
        Active(data != null);
        if (data == null)
            return;

        itemIcon.sprite = data.itemData.Icon;
        itemName.text = data.itemData.Name;
        itemDescription.text = data.itemData.Description;

        for (int i = 0; i < data.requiredItems.Length; i++)
        {
            requiredItems[i].gameObject.SetActive(data.requiredItems[i] != null);
            requiredItems[i].Setup(data.requiredItems[i]); 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CraftingUIHandler.ClickCrafting(data);
    }

    private void Active(bool active)
    {
        canvasGroup.alpha = active ? 1 : 0; 
        canvasGroup.blocksRaycasts = active;
        canvasGroup.interactable = active;
    }
}
