using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SceneManagerEx : MonoBehaviour
{
    private static SceneManagerEx instance;
    public static SceneManagerEx Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("SceneManagerEx");
                instance = go.AddComponent<SceneManagerEx>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    [Header("씬 이동 설정")]
    [SerializeField] private string[] availableScenes;
    [SerializeField] private string loadingSceneName = "LoadingScene";
    [SerializeField] private bool useLoadingScene = true;
    
    [Header("로딩 UI")]
    [SerializeField] private float fadeInOutDuration = 0.5f;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Slider progressBar;

    private bool isInTransition = false;
    public float LoadingProgress { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Clear()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        Clear();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject.FindAnyObjectByType<SceneInitializer>()?.Initialize();
    }

    /// <summary>
    /// 지정된 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneName">이동할 씬 이름</param>
    public void LoadScene(string sceneName)
    {
        if (isInTransition)
            return;

        LoadSceneAsync(sceneName).Forget();
    }

    /// <summary>
    /// 지정된 인덱스의 씬으로 이동합니다.
    /// </summary>
    /// <param name="sceneIndex">availableScenes 배열에서의 인덱스</param>
    public void LoadScene(int sceneIndex)
    {
        if (isInTransition || sceneIndex < 0 || sceneIndex >= availableScenes.Length)
            return;

        LoadSceneAsync(availableScenes[sceneIndex]).Forget();
    }

    private async UniTaskVoid LoadSceneAsync(string sceneName)
    {
        isInTransition = true;
        LoadingProgress = 0f;

        if (useLoadingScene)
        {
            // 페이드 아웃
            if (fadeImage != null)
            {
                await FadeOut();
            }

            // 로딩 씬 로드
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(loadingSceneName);
            while (!loadingOperation.isDone)
            {
                await UniTask.Yield();
            }

            // 실제 씬 로드
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            // 로딩 진행 상황 업데이트
            while (operation.progress < 0.9f)
            {
                LoadingProgress = operation.progress / 0.9f;
                if (progressBar != null)
                    progressBar.value = LoadingProgress;
                
                await UniTask.Yield();
            }

            LoadingProgress = 1f;
            if (progressBar != null)
                progressBar.value = 1f;

            // 로딩 완료 후 딜레이
            await UniTask.Delay(500);

            operation.allowSceneActivation = true;
            await UniTask.WaitUntil(() => operation.isDone);

            // 페이드 인
            if (fadeImage != null)
            {
                await FadeIn();
            }
        }
        else
        {
            // 로딩 씬 없이 직접 전환
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            
            while (!operation.isDone)
            {
                LoadingProgress = operation.progress;
                await UniTask.Yield();
            }
        }

        isInTransition = false;
    }

    /// <summary>
    /// 지정된 씬을 추가로 로드합니다.
    /// </summary>
    /// <param name="sceneName">추가로 로드할 씬 이름</param>
    public void LoadSceneAdditive(string sceneName)
    {
        LoadSceneAdditiveAsync(sceneName).Forget();
    }

    private async UniTaskVoid LoadSceneAdditiveAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        
        while (!operation.isDone)
        {
            await UniTask.Yield();
        }
    }

    /// <summary>
    /// 지정된 씬을 언로드합니다.
    /// </summary>
    /// <param name="sceneName">언로드할 씬 이름</param>
    public void UnloadScene(string sceneName)
    {
        UnloadSceneAsync(sceneName).Forget();
    }

    private async UniTaskVoid UnloadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.UnloadSceneAsync(sceneName);
        
        while (operation != null && !operation.isDone)
        {
            await UniTask.Yield();
        }
    }

    private async UniTask FadeIn()
    {
        float elapsedTime = 0;
        Color color = fadeImage.color;
        
        while (elapsedTime < fadeInOutDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = 1 - (elapsedTime / fadeInOutDuration);
            fadeImage.color = color;
            await UniTask.Yield();
        }
        
        color.a = 0;
        fadeImage.color = color;
    }

    private async UniTask FadeOut()
    {
        float elapsedTime = 0;
        Color color = fadeImage.color;
        
        while (elapsedTime < fadeInOutDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = elapsedTime / fadeInOutDuration;
            fadeImage.color = color;
            await UniTask.Yield();
        }
        
        color.a = 1;
        fadeImage.color = color;
    }
}
