using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RoomSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI _roomName;
    [SerializeField] private TextMeshProUGUI _roomMemberCount;
    [SerializeField] private TextMeshProUGUI _roomOwnerName;

    [Header("UI Image")]
    [SerializeField] private Image _outline;


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
        NetworkRoomSystem.Instance.OnJoinRoom();
    }

    public void UpdateUI(LobbyInfo lobbyInfo)
    {
        _roomName.text = lobbyInfo.RoomName;
        _roomMemberCount.text = $"{lobbyInfo.MemberCount}/{lobbyInfo.MaxPlayers}";
        _roomOwnerName.text = lobbyInfo.OwnerName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPointerClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       // _outline 컬러 변경 + 확대
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // _outline 컬러 변경 + 축소
        
    }
} 
