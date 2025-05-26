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
    private static readonly string _successSound = "Sound/UI/Click_02.mp3"; 
    private static readonly string _failedSound = "Sound/UI/Click_01.mp3"; 
    [SerializeField] private GameObject emptyStatePanel;
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
    }

    private void Setup(CraftingItemData data)
    {
        this.data = data;
        if (data == null) 
            return;

        for (int i = 0; i < data.requiredStorage.Count; i++)
        {
            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            requiredItems[i].gameObject.SetActive(slot.Data != null); 

            if (slot.Data != null)
                requiredItems[i].Setup(slot); 
        }

        emptyStatePanel.SetActive(false); 
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
            if (data.requiredStorage.GetSlotByIdx(i).Data == null)
                continue;

            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            if (InventoryDataHandler.GetItemAmount(slot.Data.Id) < slot.Stack)
            {
                Managers.Sound.Play(_failedSound);
                return;
            }
        }

        Managers.Sound.Play(_successSound);

        for (int i = 0; i < data.requiredStorage.Count; i++){
            if (data.requiredStorage.GetSlotByIdx(i).Data == null)
                continue;

            ItemSlot slot = data.requiredStorage.GetSlotByIdx(i);
            Managers.Player.Inventory.RemoveItem(slot.Data, slot.Stack); 
        }

        Managers.Player.Inventory.AddItem(data); 
        Managers.Quest.ReceiveReport(ETaskCategory.Crafting, data.Id); 
    }
}
