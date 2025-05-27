using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FishNet;
using System.Collections.Generic;
using Steamworks;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Transporting;
using FishNet.Managing.Scened;

public class LobbyRoomUI : NetworkBehaviour
{
    [SerializeField] private GameObject _userTagPrefab;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Transform _userTagRoot;
    [SerializeField] private CloseButton _closeButton;
    [SerializeField] private Button _gameStartButton;

    private string startGameSound = "Sound/UI/Popup_04.mp3";
    private string closeSound = "Sound/UI/PopupClose_01.mp3";

    private List<UIPlayerPanel> _userTagList = new List<UIPlayerPanel>();

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        _closeButton.OnClick += Close;

        NetworkGameSystem.onGameStart += HideUI;
        InstanceFinder.SceneManager.OnLoadEnd += Show;

        _gameStartButton.gameObject.SetActive(InstanceFinder.IsServerStarted);
        _gameStartButton.onClick.AddListener(GameStart);

        InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
        NetworkGameSystem.onGameStart -= HideUI;
        InstanceFinder.SceneManager.OnLoadEnd -= Show;
    }

    private void OnRemoteConnectionState(NetworkConnection connection, RemoteConnectionStateArgs args)
    {
        switch(args.ConnectionState)
        {
            case RemoteConnectionState.Started:
                CreateUI(connection);
                break;
            case RemoteConnectionState.Stopped:
                break;
        }
    }

    private void Close()
    {
        if(InstanceFinder.IsServerStarted)
        {
            NetworkCommandSystem.Instance.RequestDespawnObject(gameObject.GetComponent<NetworkObject>());
            Managers.Network.StopHost();
        }
        else
        {
            Managers.Network.StopClient();
        }

        Managers.Resource.LoadAsync<AudioClip>(closeSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }

    private void GameStart()
    {
        foreach(var userTag in _userTagList)
        {
            ServerManager.Despawn(userTag.NetworkObject);
        }

        // 게임 시작
        NetworkGameSystem.Instance.StartGame();

        Managers.Resource.LoadAsync<AudioClip>(startGameSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }

    [Server]
    private void CreateUI(NetworkConnection connection)
    {
        if(Managers.Network.Type == NetworkType.Steam)
        {
            ulong m_steamId = Managers.Steam.NetworkConnectionToSteamId[connection];
            if(m_steamId == 0) return;

            CSteamID steamId = new CSteamID(m_steamId);

            UIPlayerPanel uIPlayerPanel = Instantiate(_userTagPrefab, _userTagRoot).GetComponent<UIPlayerPanel>();
            InstanceFinder.ServerManager.Spawn(uIPlayerPanel.gameObject, connection);

            Color color = NetworkRoomSystem.Instance.GetPlayerColor(connection);
            uIPlayerPanel.PlayerName.Value = SteamFriends.GetFriendPersonaName(steamId);
            uIPlayerPanel.PlayerColor.Value = color;

            _userTagList.Add(uIPlayerPanel);
        }
    }

    [ObserversRpc]
    private void Show()
    {
        //InstanceFinder.ServerManager.Spawn(gameObject.GetComponent<NetworkObject>());
        gameObject.SetActive(true);
    }

    [ObserversRpc]
    private void HideUI()
    {
        //InstanceFinder.ServerManager.Despawn(gameObject.GetComponent<NetworkObject>());
        gameObject.SetActive(false);
    }

    [ObserversRpc]
    private void ShowUI()
    {
        gameObject.SetActive(true);
    }

    private void Show(SceneLoadEndEventArgs args)
    {
        foreach(var scene in args.LoadedScenes)
        {
            if(scene.name == "Title")
            {
                ShowUI();
            }
        }
    }
}
