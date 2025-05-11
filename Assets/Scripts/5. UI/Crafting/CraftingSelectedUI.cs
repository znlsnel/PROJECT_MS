using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CraftingSelectedUI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    [SerializeField] private Transform requiredItemRoot;
    [SerializeField] private Button button;
    private CraftingItemData data;
    private List<CraftingRequiredItemUI> requiredItems;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        CraftingHandler.onSlotClick += Setup;
        button.onClick.AddListener(OnClick);

        canvasGroup = GetComponent<CanvasGroup>();
        requiredItems = requiredItemRoot.GetComponentsInChildren<CraftingRequiredItemUI>(true).ToList(); 
        CraftingTableUI.onShow += ()=>Active(false);
    }

    private void Setup(CraftingItemData data)
    {
        this.data = data;
        Active(data != null);
        if (data == null)
            return;

        itemIcon.sprite = data.Icon;
        itemName.text = data.Name;
        itemDescription.text = data.Description;

        for (int i = 0; i < data.requiredItems.Length; i++)
        {
            requiredItems[i].gameObject.SetActive(data.requiredItems[i] != null);
            requiredItems[i].Setup(data.requiredItems[i]); 
        }
    }

    private void Active(bool active)
    {
        canvasGroup.alpha = active ? 1 : 0; 
        canvasGroup.blocksRaycasts = active;
        canvasGroup.interactable = active;
    }

    private void OnClick() 
    {
        if (data == null)
            return;

        CraftingHandler.ClickCrafting(data);
    }
}
