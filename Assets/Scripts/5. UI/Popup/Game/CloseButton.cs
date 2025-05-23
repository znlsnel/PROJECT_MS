using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Button _button;
    public event Action OnClick;


    private void Awake()
    {
        _button.onClick.AddListener(()=>OnClick?.Invoke());
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1.1f, 0.2f).SetEase(Ease.OutBack); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill(); 
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }


}
