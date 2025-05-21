
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemPickupUI : PopupUI
{
    private static readonly string _itemPickupSound = "Sound/UI/Click_02.mp3";

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private Transform panel;

    protected override void Awake()
    {
        base.Awake();
        Managers.onChangePlayer += Init;
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
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<ItemPickupUI>(this); 
        itemIcon.sprite = data.Icon;
        itemName.text = data.Name;

        // 기존에 panel에 적용된 모든 트윈을 종료
        panel.DOKill();

        panel.localScale = Vector3.one;
        panel.DOScale(1.2f, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
        {
            DOVirtual.DelayedCall(0.5f, () =>
            {
                panel.DOScale(1f, 0.2f).SetEase(Ease.InOutSine).OnComplete(() =>
                {
                    Managers.UI.ClosePopupUI(this);
                });
            });
        });

    }
}
