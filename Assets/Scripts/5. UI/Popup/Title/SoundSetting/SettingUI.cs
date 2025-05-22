using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : PopupUI
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _panelButton;
    private bool _isHide = false;

    protected override void Awake()
    {
        base.Awake();
        _mainPanel.transform.localScale = Vector3.one;
        _panelButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
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
