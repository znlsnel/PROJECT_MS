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
    private List<RequiredItemSlotUI> requiredItems;
    private CanvasGroup canvasGroup;


    private void Awake()
    {
        CraftingSlotUI.onSlotClick += Setup;
        button.onClick.AddListener(OnClick);

        canvasGroup = GetComponent<CanvasGroup>();
        requiredItems = requiredItemRoot.GetComponentsInChildren<RequiredItemSlotUI>(true).ToList(); 
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

        for (int i = 0; i < data.requiredStorage.Count; i++)
        {
            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            requiredItems[i].gameObject.SetActive(slot != null);
            requiredItems[i].Setup(slot); 
        }
    }

    private void Active(bool active)
    {
        // canvasGroup.alpha = active ? 1 : 0; 
        // canvasGroup.blocksRaycasts = active; 
        // canvasGroup.interactable = active;
    }

    private void OnClick() 
    {
        if (data == null)
            return;

        MakeItem(data);
    }

    private void MakeItem(CraftingItemData data)
    {  
        for (int i = 0; i < data.requiredStorage.Count; i++){
            if (data.requiredStorage.GetSlotByIdx(i) == null)
                continue;

            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            if (InventoryDataHandler.GetItemAmount(slot.Data.Id) < slot.Stack)
                return;
        }

        Managers.UserData.Inventory.AddItem(data); 
        Managers.Quest.ReceiveReport(ETaskCategory.Crafting, data.Id); 

        for (int i = 0; i < data.requiredStorage.Count; i++){
            if (data.requiredStorage.GetSlotByIdx(i) == null)
                continue;

            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            Managers.UserData.Inventory.RemoveItem(slot.Data, slot.Stack); 
        }

        Debug.Log("ClickCrafting");
    }
}
