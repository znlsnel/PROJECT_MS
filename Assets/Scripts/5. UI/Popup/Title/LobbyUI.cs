using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyUI : PopupUI
{
    [Header("UI Prefab")]
    [SerializeField] private GameObject _createRoomUIPrefab;
    [SerializeField] private GameObject _lobbyRoomUIPrefab;
    [SerializeField] private GameObject _roomUIPrefab;
 

    [Header("UI Button")]
    [SerializeField] private Button _createRoomButton;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private CloseButton _closeButton;


    [Header("Room List Root")]
    [SerializeField] private GameObject _roomListRoot; 

    [Header("UI Root")]
    [SerializeField] private Transform _mainPanel;
    

    private CreateRoomUI _createRoomUI;
    private LobbyRoomUI _lobbyRoomUI;

    protected override void Awake()
    {
        base.Awake();
        _createRoomButton.onClick.AddListener(OpenCreateRoomUI); 
        _refreshButton.onClick.AddListener(RefreshRoomList); 
        _closeButton.OnClick += Close;
    } 
 
    public override void Show()
    {
        base.Show();
        _mainPanel.localScale = Vector3.zero; 
        _mainPanel.DOScale(1.0f, 0.4f).SetEase(Ease.OutCubic); 
    }

    private void Close()
    { 
        _mainPanel.DOScale(0.0f, 0.4f).SetEase(Ease.OutCubic).onComplete += () => {
            Hide();  
        };  
    }

    private void OpenCreateRoomUI()
    {
        if (_createRoomUI == null)
        {
            _createRoomUI = Instantiate(_createRoomUIPrefab).GetComponent<CreateRoomUI>();
            _createRoomUI.Setup(_lobbyRoomUIPrefab);   
        }

        Managers.UI.ShowPopupUI(_createRoomUI);  
    }

    private void OpenLobbyRoomUI()
    {
        if (_lobbyRoomUI == null)
            _lobbyRoomUI = Instantiate(_lobbyRoomUIPrefab).GetComponent<LobbyRoomUI>();

        Managers.UI.ShowPopupUI(_lobbyRoomUI);   
    }

    // 룸 정보 갱신
    private void RefreshRoomList()
    {
        
    }

}
