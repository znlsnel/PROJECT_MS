using FishNet;
using FishNet.Transporting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject LoadingUI;

    private NetworkSceneSystem networkSceneSystem;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadingUI.SetActive(false);
    }

    private void OnEnable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState += OnClientConnectionState;
    }

    private void OnDisable()
    {
        InstanceFinder.ClientManager.OnClientConnectionState -= OnClientConnectionState;
    }

    private void OnLoadingStart()
    {
        LoadingUI.SetActive(true);
    }

    private void OnLoadingEnd()
    {
        LoadingUI.SetActive(false);
    }

    private void OnClientConnectionState(ClientConnectionStateArgs args)
    {
        switch(args.ConnectionState)
        {
            case LocalConnectionState.Started:
                NetworkSceneSystem.OnLoadingStart += OnLoadingStart;
                NetworkSceneSystem.OnChangeTaskName += (name) => {taskName.text = name;};
                NetworkSceneSystem.OnChangeTaskProgress += (progress) => {slider.value = progress;};
                NetworkSceneSystem.OnLoadingEnd += OnLoadingEnd;
                break;
            case LocalConnectionState.Stopped:
                NetworkSceneSystem.OnLoadingStart -= OnLoadingStart;
                NetworkSceneSystem.OnChangeTaskName -= (name) => {taskName.text = name;};
                NetworkSceneSystem.OnChangeTaskProgress -= (progress) => {slider.value = progress;};
                NetworkSceneSystem.OnLoadingEnd -= OnLoadingEnd;
                break;
        }
    }
}
