using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultUI : PopupUI
{
    [SerializeField] private Button _lobbyButton;

    protected override void Awake()
    {
        base.Awake();
        _lobbyButton.onClick.AddListener(OnClickLobbyButton);
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
