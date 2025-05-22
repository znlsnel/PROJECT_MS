using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : PopupUI, IPointerClickHandler
{
    [SerializeField] private GameObject _mainPanel;
    private bool _isHide = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Managers.UI.ClosePopupUI();
        Hide();
    }

    public override void Hide()
    {
        if (_isHide)
            return; 

        _isHide = true;
        _mainPanel.transform.DOScale(0, 0.3f).SetEase(Ease.OutExpo).onComplete = () =>
        {
            base.Hide();
            _isHide = false;
            _mainPanel.transform.localScale = Vector3.one; 
        }; 
    }
}
