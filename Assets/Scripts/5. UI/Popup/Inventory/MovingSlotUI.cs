using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MovingSlotUI : PopupUI
{
    private static readonly string _movingSlotSound = "Sound/UI/Click_03.mp3";

    [SerializeField] private RectTransform panelRT;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;

    protected override void Awake() 
    {
        base.Awake();
        _canStack = false; 
    }

    public override void Show()
    {
        base.Show();
        Managers.Sound.Play(_movingSlotSound);
    } 

    public override void Hide()
    { 
        base.Hide();
        Managers.Sound.Play(_movingSlotSound);
    }

    public void SetItem(ItemSlot itemSlot)
    {
        if (itemSlot.Data == null)
            return;


        itemSlot.onChangeStack += UpdateSlotInfo; 
        UpdateSlotInfo(itemSlot);
    }

    private void UpdateSlotInfo(ItemSlot itemSlot)
    {
        ItemData itemData = itemSlot.Data;
        int amount = itemSlot.Stack;
        if (amount <= 0)
        {
            return;
        }

        itemCountText.transform.parent.gameObject.SetActive(itemData.CanStack); 


        itemImage.sprite = itemData.Icon;
        itemCountText.text = amount.ToString();  
        Vector3 pos = panelRT.parent.InverseTransformPoint(Input.mousePosition);
        panelRT.localPosition = pos;
        itemImage.transform.DOScale(1.4f, 0.1f).OnComplete(()=>
        {
            itemImage.transform.DOScale(1f, 0.1f); 
        });
    }

    private void Update()
    {
        Vector3 pos = panelRT.parent.InverseTransformPoint(Input.mousePosition);
        panelRT.localPosition = pos;
    }
}
 