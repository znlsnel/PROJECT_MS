using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MovingSlotUI : PopupUI
{
    [SerializeField] private RectTransform panelRT;
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCountText;

    public void SetItem(ItemData itemData, int amount)
    {
        if (itemData == null)
            return;

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
 