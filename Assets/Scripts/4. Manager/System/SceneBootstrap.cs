
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBootstrap
{
    public static string StartingSceneName { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Bootstrap()
    {
        if(string.IsNullOrEmpty(StartingSceneName))
        {
            StartingSceneName = SceneManager.GetActiveScene().name;
        }
        else
        {
            Debug.Log($"Starting Scene Already Set: {StartingSceneName}");
        }

        if(StartingSceneName != "Bootstrap")
        {
            Debug.Log("Non-Title Scene Detected. Forcing Initialization...");
            
            SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
        }
        else
        {
            Debug.Log("Title Scene Detected. No Forced Initialization Required.");

            SceneManager.LoadScene("Title", LoadSceneMode.Additive);
        }
    }
}
