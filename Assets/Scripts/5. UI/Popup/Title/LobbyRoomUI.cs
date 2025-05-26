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

public class LobbyRoomUI : NetworkBehaviour
{
    [SerializeField] private GameObject _userTagPrefab;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Transform _userTagRoot;
    [SerializeField] private CloseButton _closeButton;
    [SerializeField] private Button _gameStartButton;

    private List<UIPlayerPanel> _userTagList = new List<UIPlayerPanel>();

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        _closeButton.OnClick += Close;
        
        _gameStartButton.gameObject.SetActive(InstanceFinder.IsServerStarted);
        _gameStartButton.onClick.AddListener(GameStart);

        InstanceFinder.ServerManager.OnRemoteConnectionState += OnRemoteConnectionState;
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        InstanceFinder.ServerManager.OnRemoteConnectionState -= OnRemoteConnectionState;
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
        (InstanceFinder.IsServerStarted ? (Action)Managers.Network.StopHost : Managers.Network.StopClient)();
    }

    private void GameStart()
    {
        foreach(var userTag in _userTagList)
        {
            ServerManager.Despawn(userTag.NetworkObject);
        }

        // 게임 시작
        NetworkGameSystem.Instance.StartGame();
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
}
