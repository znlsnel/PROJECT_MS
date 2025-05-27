using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultUI : PopupUI
{
    [SerializeField] private GameObject _userSlotPrefab;
    [SerializeField] private Transform _userListRoot;
    [SerializeField] private Button _lobbyButton;

    protected override void Awake()
    {
        base.Awake();
        _lobbyButton.onClick.AddListener(OnClickLobbyButton);
    }

    public void Setup()
    {
        foreach (PlayerInfo playerInfo in NetworkGameSystem.Instance.Players.Values)
        {
            GameObject userSlot = Instantiate(_userSlotPrefab, _userListRoot);
            userSlot.GetComponent<ResultUserSlotUI>().Setup(playerInfo.playerName, playerInfo.role, playerInfo.isDead, playerInfo.killCount);
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
