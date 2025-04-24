using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public static string loadSceneName = "";
    public Image loadImage;
    public TextMeshProUGUI loadText;

    private static bool isLoadingCompleted = false;
    private static AsyncOperation asyncOperation;

    private float nowTime = 0f;

    private void Start()
    {
        nowTime = 0;
        isLoadingCompleted = false;
        StartCoroutine(LoadSceneAsync(loadSceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        Debug.Log($"Loading Scene: {sceneName}");

        asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        // 씬전환 비활성화
        asyncOperation.allowSceneActivation = false;

        while(!isLoadingCompleted)
        {
            UpdateProgress();

            yield return null;
        }

        loadImage.fillAmount = 1;
		loadText.text = "100%";

        InitializeScene();
    }

    private void InitializeScene()
    {
        SceneInitializer initializer = FindFirstObjectByType<SceneInitializer>();

        if(initializer != null)
        {
            initializer.Initialize();
        }
        else
        {
            Debug.LogWarning($"No SceneInitializer found in the current scene.");
        }
    }

    private void UpdateProgress()
    {
        nowTime += Time.deltaTime;
        
        if(nowTime < 5f) {
			loadImage.fillAmount = Mathf.Lerp(loadImage.fillAmount, 0.5f, Time.deltaTime);
			loadText.text = ((int)(loadImage.fillAmount * 100)).ToString() + "%";
		}

		if(asyncOperation.progress >= 0.5f && nowTime > 5f) {
			float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            loadImage.fillAmount = progress + 0.1f;
            loadText.text = ((int)(loadImage.fillAmount * 100)).ToString() + "%";
		}

		if(asyncOperation.progress >= 0.9f && nowTime > 5f) {
            isLoadingCompleted = true;
			asyncOperation.allowSceneActivation = true;
		}
    }
}
