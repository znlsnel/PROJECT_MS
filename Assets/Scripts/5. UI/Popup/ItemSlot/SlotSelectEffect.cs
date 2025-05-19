using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSlotUI))]
public class SlotSelectEffect : MonoBehaviour
{
    [SerializeField] private Image selectImage;


    private void Awake()
    {
        if (selectImage != null)
            selectImage.gameObject.SetActive(false);

        ItemSlotUI itemSlotUI = GetComponent<ItemSlotUI>();
        itemSlotUI.onSelect += SelectSlot;
    }

    private void SelectSlot(bool isSelect)
    {
        transform.DOKill();

        transform.localScale = !isSelect ? Vector3.one * 1.1f : Vector3.one;

        if (isSelect)
            transform.DOScale(1.1f, 0.5f).SetEase(Ease.InOutBack, 10f, 1f);
        else
            transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBack, 7f, 1f);

        if (selectImage != null)
            selectImage.gameObject.SetActive(isSelect);
    }
}
