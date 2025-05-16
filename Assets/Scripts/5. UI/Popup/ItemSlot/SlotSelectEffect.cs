using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSlotUI))]
public class SlotSelectEffect : MonoBehaviour
{
    [SerializeField] private Image selectImage;

    private void Awake()
    {
        selectImage.gameObject.SetActive(false);
        ItemSlotUI itemSlotUI = GetComponent<ItemSlotUI>();
        itemSlotUI.onSelect += SelectSlot;
    }

    private void SelectSlot(bool isSelect)
    {
        transform.DOKill();
        transform.DOScale(isSelect ? 1.2f : 1f, 0.5f).SetEase(Ease.InOutBack, 5f);
        selectImage.gameObject.SetActive(isSelect);
    }
}
