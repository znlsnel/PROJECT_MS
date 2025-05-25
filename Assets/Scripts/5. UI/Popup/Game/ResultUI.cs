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
        AlivePlayer[] alivePlayers = FindObjectsByType<AlivePlayer>(FindObjectsSortMode.None);
        foreach (AlivePlayer alivePlayer in alivePlayers)
        {
            GameObject userSlot = Instantiate(_userSlotPrefab, _userListRoot);
         //   userSlot.GetComponent<ResultUserSlotUI>().Setup(alivePlayer.GetComponent<Player>().GetNickname(), alivePlayer.GetComponent<Player>().GetRole(), alivePlayer.GetComponent<Player>().IsSurvive(), alivePlayer.GetComponent<Player>().GetKill());
        }
    }


    private void OnClickLobbyButton()
    {
        transform.DOScale(Vector3.one * 1.1f, 0.1f).SetEase(Ease.OutCubic).onComplete += () => {
            transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InCubic).onComplete += () => {
                SceneManager.LoadScene("Lobby");
            };
        };
    }
}
