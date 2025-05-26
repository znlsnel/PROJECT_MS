using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : PopupUI
{
    [Header("Core")]
    [SerializeField] private RectTransform mainPanel;
    [SerializeField] private Image nameBackground;
    [SerializeField] private Transform effectParent;

    [Header("Item Info")]
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private List<ItemInfoSlotUI> effectSlotPrefabs; 

    protected override void Awake()
    {
        base.Awake();
        effectSlotPrefabs = effectParent.GetComponentsInChildren<ItemInfoSlotUI>().ToList(); 
    }

    public void Setup(ItemSlotUI itemSlotUI)
    {
        ItemData itemData = itemSlotUI.ItemSlot.Data;
        if (itemData == null){
            return;
        }

        Vector3 pos = mainPanel.parent.InverseTransformPoint(itemSlotUI.transform.position);
        mainPanel.localPosition = new Vector3(pos.x, pos.y, 0); 

        itemName.text = itemData.Name;
        itemDescription.text = itemData.Description;
        nameBackground.color = ItemSlotBackGroundComponent.colors[itemData.ItemType];

         
        int cnt = 1;

        if (itemData.Damage > 0)
        {
            effectSlotPrefabs[cnt-1].gameObject.SetActive(true);    
            effectSlotPrefabs[cnt-1].Setup(EItemEffectType.Damage, (int)itemData.Damage);
            cnt++;
        }

        if (itemData.MaxDurability > 0)
        {
            effectSlotPrefabs[cnt-1].gameObject.SetActive(true);
            effectSlotPrefabs[cnt-1].Setup(EItemEffectType.Durability, (int)itemData.MaxDurability);
            cnt++;
        }

        if (itemData.Heal > 0)
        {
            effectSlotPrefabs[cnt-1].gameObject.SetActive(true);
            effectSlotPrefabs[cnt-1].Setup(EItemEffectType.Heal, (int)itemData.Heal);
            cnt++;
        }

        if (itemData.RestoreStamina > 0) 
        { 
            effectSlotPrefabs[cnt-1].gameObject.SetActive(true);
            effectSlotPrefabs[cnt-1].Setup(EItemEffectType.Stamina, (int)itemData.RestoreStamina);
            cnt++;
        }

        for (int i = cnt-1; i < effectSlotPrefabs.Count; i++)
        {
            effectSlotPrefabs[i].gameObject.SetActive(false); 
        }

        
        mainPanel.SetHeight(100 + ((cnt /2) * 45));
    }

    
}
