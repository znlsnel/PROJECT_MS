
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

public class ItemPickupUI : PopupUI
{
    private static readonly string _itemPickupSound = "Sound/UI/Click_02.mp3";

    private Queue<TextMeshProUGUI> itemNameTexts = new Queue<TextMeshProUGUI>();
    private Queue<TextMeshProUGUI> useTexts = new Queue<TextMeshProUGUI>();

    Color defaultColor;
    protected override void Awake()
    {
        base.Awake();
        Managers.onChangePlayer += Init;

        transform.GetComponentsInChildren<TextMeshProUGUI>(true).ToList().ForEach(text => {
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
        TextMeshProUGUI text = GetText();
 
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
            ReturnText();  
        });
    }


    private TextMeshProUGUI GetText()
    {
        TextMeshProUGUI text = null;
        if (itemNameTexts.Count > 0)
            text = itemNameTexts.Dequeue();
        else
            text = useTexts.Dequeue();
 
        useTexts.Enqueue(text);
        return text;
    }

    private void ReturnText()
    {
        if (useTexts.Count > 0)
        {
            itemNameTexts.Enqueue(useTexts.Dequeue());  
        }
    }
}
