using System;
using TMPro;
using UnityEngine;

public class UIRoomPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _roomMemberCount;
    [SerializeField] private TextMeshProUGUI _roomOwnerName;
    private ulong _lobbyId;

    public static event Action<ulong> OnJoinLobby;

    public void SetLobbyId(ulong lobbyId)
    {
        _lobbyId = lobbyId;
    }

    public void OnPointerClick()
    {
        Managers.Steam.JoinByID(_lobbyId);
        OnJoinLobby?.Invoke(_lobbyId);
    }

    public void UpdateUI(LobbyInfo lobbyInfo)
    {
        _roomName.text = lobbyInfo.RoomName;
        _roomMemberCount.text = $"{lobbyInfo.MemberCount}/{lobbyInfo.MaxPlayers}";
        _roomOwnerName.text = lobbyInfo.OwnerName;
    }
}
