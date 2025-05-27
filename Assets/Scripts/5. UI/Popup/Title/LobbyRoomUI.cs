using UnityEngine;
using UnityEngine.UI;
using FishNet;
using System.Collections.Generic;
using Steamworks;
using FishNet.Object;
using FishNet.Connection;
using System.Collections;

public class LobbyRoomUI : MonoBehaviour
{
    [SerializeField] private GameObject _userTagPrefab;

    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private Transform _userTagRoot;
    [SerializeField] private CloseButton _closeButton;
    [SerializeField] private Button _gameStartButton;

    private string startGameSound = "Sound/UI/Popup_04.mp3";
    private string closeSound = "Sound/UI/PopupClose_01.mp3";

    public List<UIPlayerPanel> _userTagList = new List<UIPlayerPanel>();

    private void Start()
    {
        _closeButton.OnClick += Close;

        _gameStartButton.gameObject.SetActive(InstanceFinder.IsServerStarted);
        _gameStartButton.onClick.AddListener(GameStart);
    }

    public void OnDestroy()
    {
        _closeButton.OnClick -= Close;
    }

    private void Close()
    {
        Managers.Resource.LoadAsync<AudioClip>(closeSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });

        Destroy(gameObject);
    }

    private void GameStart()
    {
        // 게임 시작
        NetworkGameSystem.Instance.StartGame();

        Managers.Resource.LoadAsync<AudioClip>(startGameSound, (audioClip) =>
        {
            Managers.Sound.Play(audioClip);
        });
    }

    public void ResetUI()
    {
        foreach(var userTag in _userTagList)
        {
            Destroy(userTag.gameObject);
        }

        _userTagList.Clear();
    }

    public void CreateUI(string name, Color color)
    {
        UIPlayerPanel uIPlayerPanel = Instantiate(_userTagPrefab, _userTagRoot).GetComponent<UIPlayerPanel>();
        uIPlayerPanel.UpdateUI(name, color);
        _userTagList.Add(uIPlayerPanel);
    }
}
