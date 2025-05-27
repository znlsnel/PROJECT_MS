using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingUI : PopupUI
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _panelButton;



    protected override void Awake()
    {
        base.Awake();
        _mainPanel.transform.localScale = Vector3.one;
        _panelButton.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        HideWithDoTween(_mainPanel.transform);
    }  

}
