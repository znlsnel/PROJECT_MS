using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSlotUI))]
public class SlotSelectEffect : MonoBehaviour
{
    [SerializeField] private Image selectImage;
    [SerializeField] private float time = 0.2f;
    [SerializeField] private float overshoot = 5f;
    [SerializeField] private float scale = 1.2f;
    [SerializeField] private float period = 0.2f;

    private void Awake()
    {
        selectImage.gameObject.SetActive(false);
        ItemSlotUI itemSlotUI = GetComponent<ItemSlotUI>();
        itemSlotUI.onSelect += SelectSlot;
    }

    private void SelectSlot(bool isSelect)
    {
        transform.DOKill();
        if (isSelect)
            transform.DOScale(1.1f, 0.5f).SetEase(Ease.InOutBack, 10f, 1f);
        else
            transform.DOScale(1f, 0.5f).SetEase(Ease.InOutBack, 7f, 1f);

        selectImage.gameObject.SetActive(isSelect);
    }
}
