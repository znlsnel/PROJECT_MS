using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyRoomUI : PopupUI
{
    [SerializeField] private GameObject _userTagPrefab;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Transform _userTagRoot;
    [SerializeField] private CloseButton _closeButton;

    protected override void Awake()
    {
        base.Awake();
        _closeButton.OnClick += Close;
    }
 
    private void Close()
    {
        _mainPanel.transform.DOScale(0.0f, 0.4f).SetEase(Ease.OutCubic).onComplete += () => {
            Hide(); 
        };  
    }

    public override void Show()
    { 
        base.Show(); 
        _mainPanel.transform.localScale = Vector3.one * 0.9f; 
        gameObject.SetActive(true);
        _mainPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack, 10f);   
    }
}
