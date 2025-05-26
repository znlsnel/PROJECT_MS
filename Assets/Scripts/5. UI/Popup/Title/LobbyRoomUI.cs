using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FishNet;
using System.Collections.Generic;
using Steamworks;

public class LobbyRoomUI : PopupUI
{
    [SerializeField] private GameObject _userTagPrefab;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Transform _userTagRoot;
    [SerializeField] private CloseButton _closeButton;
    [SerializeField] private Button _gameStartButton;

    private List<UIPlayerPanel> _userTagList = new List<UIPlayerPanel>();

    private string startGameSound = "Sound/UI/Popup_04.mp3";
    private string closeSound = "Sound/UI/PopupClose_01.mp3";

    protected override void Awake()
    {
        base.Awake();
        _closeButton.OnClick += Close;
        _gameStartButton.onClick.AddListener(GameStart);
    }

    private void Start()
    {
        Managers.Network.OnClientConnected += UpdateUI;
    }

    private void OnDestroy()
    {
        Managers.Network.OnClientConnected -= UpdateUI;
    }

    private void Close()
    {
        (InstanceFinder.IsHostStarted ? (Action)Managers.Network.StopHost : Managers.Network.StopClient)();

        _mainPanel.transform.DOScale(0.0f, 0.4f).SetEase(Ease.OutCubic).onComplete += () => {
            Hide(); 
        };

        Managers.Resource.LoadAsync<AudioClip>(closeSound, (audioClip) =>
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

    private void GameStart()
    {
        // 게임 시작
        NetworkGameSystem.Instance.StartGame();

        Managers.Resource.LoadAsync<AudioClip>(startGameSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }

    private void UpdateUI()
    {
        ResetUI();

        _gameStartButton.gameObject.SetActive(InstanceFinder.IsServerStarted);
        
        if(Managers.Network.Type == NetworkType.Steam)
        {
            List<CSteamID> members = Managers.Steam.RequestLobbyMemberList();
        
            foreach (var member in members)
            {
                UIPlayerPanel uIPlayerPanel = Instantiate(_userTagPrefab, _userTagRoot).GetComponent<UIPlayerPanel>();
                uIPlayerPanel.UpdateUI(member.m_SteamID);
                _userTagList.Add(uIPlayerPanel);
            }
        }
    }

    private void ResetUI()
    {
        foreach (var uIPlayerPanel in _userTagList)
        {
            Destroy(uIPlayerPanel.gameObject);
        }

        _userTagList.Clear();
    }
}
