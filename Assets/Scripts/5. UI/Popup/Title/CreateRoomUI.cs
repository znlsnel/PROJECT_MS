using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FishNet.Object;
using Steamworks;
using TMPro;


public class CreateRoomUI : PopupUI
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private CloseButton _closeButton;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_Dropdown _roomTypeDropdown;
    
    private GameObject _lobbyRoomUIPrefab;
    private LobbyRoomUI _lobbyRoomUI;
    
    private string openSound = "Sound/UI/Click_02.mp3";
    private string closeSound = "Sound/UI/PopupClose_01.mp3";

    protected override void Awake()
    {
        base.Awake();
        _createRoomButton.onClick.AddListener(CreateRoom);
        _closeButton.OnClick += () => HideWithDoTween(_mainPanel.transform);
    }

    public void Setup(GameObject lobbyRoomUI)
    {
        _lobbyRoomUIPrefab = lobbyRoomUI; 
    }

    private void CreateRoom()
    {
        if(Managers.Network.Type == NetworkType.Steam)
        {
            Managers.Steam.lobbyInfo.RoomName = _roomNameInputField.text;
            Managers.Steam.LobbyType = (ELobbyType)_roomTypeDropdown.value;
        }

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
