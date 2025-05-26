using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class CreateRoomUI : PopupUI
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private CloseButton _closeButton;
    
    private GameObject _lobbyRoomUIPrefab;
    private LobbyRoomUI _lobbyRoomUI;
    
    private string openSound = "Sound/UI/Click_02.mp3";
    private string closeSound = "Sound/UI/PopupClose_01.mp3";

    protected override void Awake()
    {
        base.Awake();
        _createRoomButton.onClick.AddListener(CreateRoom);
        _closeButton.OnClick += Close;
    }

    public void Setup(GameObject lobbyRoomUI)
    {
        _lobbyRoomUIPrefab = lobbyRoomUI; 
    }

    private void Close()
    { 
        _mainPanel.transform.DOScale(0.0f, 0.4f).SetEase(Ease.OutCubic).onComplete += () => {
            Hide(); 
        };

        Managers.Resource.LoadAsync<AudioClip>(closeSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }

    private void CreateRoom()
    {
        Managers.Network.StartHost();

        Hide();

        Managers.Resource.LoadAsync<AudioClip>(openSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }


    public override void Show()
    { 
        base.Show(); 
        _mainPanel.transform.localScale = Vector3.one * 0.9f; 
        gameObject.SetActive(true);
        _mainPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack, 10f);   
    }
}
