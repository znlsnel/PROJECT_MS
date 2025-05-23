using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [Header("UI Prefab")]
    [SerializeField] private GameObject _settingUIPrefab;
    [SerializeField] private GameObject _lobbyUIPrefab;

    [Header("UI Button")]
    [SerializeField] private Button _settingButton;
    [SerializeField] private Button _gameStartButton;
    [SerializeField] private Button _gameExitButton;

    private LobbyUI _lobbyUI; 
    private SettingUI _settingUI;

    private void Awake()
    {
        _settingButton.onClick.AddListener(OpenSettingUI);
        _gameStartButton.onClick.AddListener(OpenLobbyUI);
        _gameExitButton.onClick.AddListener(GameExit);
    }
 

    private void OpenSettingUI()
    {
        
        if (_settingUI == null)
            _settingUI = Instantiate(_settingUIPrefab).GetComponent<SettingUI>(); 
        
 
            
        Managers.UI.ShowPopupUI(_settingUI); 
    }

    private void OpenLobbyUI()
    {
        if (_lobbyUI == null)
            _lobbyUI = Instantiate(_lobbyUIPrefab).GetComponent<LobbyUI>(); 

        Managers.UI.ShowPopupUI(_lobbyUI);
    }

    private void GameExit()
    {
        Application.Quit();
    }

}
