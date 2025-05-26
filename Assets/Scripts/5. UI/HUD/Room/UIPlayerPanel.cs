using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerPanel : NetworkBehaviour
{
    public readonly SyncVar<string> PlayerName = new SyncVar<string>();
    public readonly SyncVar<Color> PlayerColor = new SyncVar<Color>();

    [SerializeField] private TextMeshProUGUI _playerName;
    [SerializeField] private Image _playerColor;

    private void Init()
    {
        _playerColor.color = PlayerColor.Value;
        _playerName.text = PlayerName.Value;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        NetworkRoomSystem.Instance.PlayerColors.OnChange += OnPlayerColorChanged;

        PlayerName.OnChange += OnPlayerNameChanged;
        PlayerColor.OnChange += OnPlayerColorChanged;

        Init();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();

        NetworkRoomSystem.Instance.PlayerColors.OnChange -= OnPlayerColorChanged;

        PlayerName.OnChange -= OnPlayerNameChanged;
        PlayerColor.OnChange -= OnPlayerColorChanged;
    }

    private void OnPlayerColorChanged(SyncDictionaryOperation op, NetworkConnection key, ColorType value, bool asServer)
    {
        if(key == Owner)
        {
            _playerColor.color = NetworkRoomSystem.Instance.GetPlayerColor(key);
            OnColorChange(_playerColor.color);
        }
    }

    [ServerRpc]
    private void OnColorChange(Color color)
    {
        PlayerColor.Value = color;
    }

    private void OnPlayerColorChanged(Color prev, Color next, bool asServer)
    {
        _playerColor.color = next;
    }

    private void OnPlayerNameChanged(string prev, string next, bool asServer)
    {
        _playerName.text = next;
    }
}
