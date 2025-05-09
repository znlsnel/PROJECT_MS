using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskName;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject LoadingUI;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadingUI.SetActive(false);
    }

    private void OnEnable()
    {
        Managers.Scene.OnLoadingStart += OnLoadingStart;
        Managers.Scene.OnChangeTaskName += (name) => {taskName.text = name;};
        Managers.Scene.OnChangeTaskProgress += (progress) => {slider.value = progress;};
        Managers.Scene.OnLoadingEnd += OnLoadingEnd;
    }

    private void OnDisable()
    {
        Managers.Scene.OnLoadingStart -= OnLoadingStart;
        Managers.Scene.OnChangeTaskName -= (name) => {taskName.text = name;};
        Managers.Scene.OnChangeTaskProgress -= (progress) => {slider.value = progress;};
        Managers.Scene.OnLoadingEnd -= OnLoadingEnd;
    }

    private void OnLoadingStart()
    {
        LoadingUI.SetActive(true);
    }

    private void OnLoadingEnd()
    {
        LoadingUI.SetActive(false);
    }
}
