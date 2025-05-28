using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using FishNet.Connection;
using Steamworks;

public class ResultUI : PopupUI
{
    [SerializeField] private GameObject _userSlotPrefab;
    [SerializeField] private Transform _userListRoot;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private Button _lobbyButton;

    protected override void Awake()
    {
        base.Awake();
        _lobbyButton.onClick.AddListener(OnClickLobbyButton);
    }

    public void Setup()
    {
        foreach (Transform child in _userListRoot)
            child.gameObject.SetActive(false);  

        bool isEndTime = Managers.scene.GetComponent<TimeSystem>().IsTimeEnd;
        bool isMafiaWin = true;
        foreach (var pare in NetworkGameSystem.Instance.Players)
        {
            PlayerInfo playerInfo = pare.Value;
            NetworkConnection connection = pare.Key;

            Color color = NetworkRoomSystem.Instance.GetPlayerColor(connection);
            string name = NetworkRoomSystem.Instance.GetPlayerName(connection);
            if (name == "") continue;
 



            GameObject userSlot = Instantiate(_userSlotPrefab, _userListRoot); 

            userSlot.GetComponent<ResultUserSlotUI>().Setup(name, color, playerInfo.role, !playerInfo.isDead, playerInfo.killCount);
 
            if (playerInfo.role == EPlayerRole.Survival && !playerInfo.isDead)
                isMafiaWin = false;
        }

        if (isEndTime || isMafiaWin)
        {
            _title.text = "마피아 승리";
        }
        else
        {
            _title.text = "생존자 승리"; 
        }
    }
 

    private void OnClickLobbyButton()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutCubic).onComplete += () => {
            transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InCubic).onComplete += () => {
                NetworkSceneSystem.Instance.LoadScene("Title");
            };
        };
    }
}
