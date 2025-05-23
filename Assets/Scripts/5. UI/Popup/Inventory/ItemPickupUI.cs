
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class ItemPickupUI : PopupUI
{
    private static readonly string _itemPickupSound = "Sound/UI/Click_02.mp3";

    Queue<TextMeshProUGUI> itemNameTexts = new Queue<TextMeshProUGUI>();
    Color defaultColor;
    protected override void Awake()
    {
        base.Awake();
        Managers.onChangePlayer += Init;

        transform.GetComponentsInChildren<TextMeshProUGUI>().ToList().ForEach(text => {
            defaultColor = text.color; 
            text.gameObject.SetActive(false);
            itemNameTexts.Enqueue(text);
        });
    }

    private void OnDestroy()
    {
        Managers.onChangePlayer -= Init;
    }

    public override void Show()
    {
        base.Show();
        Managers.Sound.Play(_itemPickupSound);
    }

    private void Init(AlivePlayer player)
    {
        player.Inventory.onAddItem += Setup;
    } 

    private void Setup(ItemData data)
    {
        TextMeshProUGUI text = itemNameTexts.Dequeue();
        text.text = $"<color=green>{data.Name}</color> 획득!";
        text.gameObject.SetActive(true);
 
        text.DOKill();
 
        text.transform.localScale = Vector3.one;
        text.transform.localPosition = Vector3.zero;
        text.color = defaultColor; 

        text.transform.DOScale(0.9f, 1f).SetEase(Ease.OutCirc);  
        text.transform.DOLocalMoveY(100f, 1f).SetEase(Ease.OutCirc);  
        text.DOFade(0f, 1f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            itemNameTexts.Enqueue(text); 
            text.gameObject.SetActive(false);
        });
    }
}
