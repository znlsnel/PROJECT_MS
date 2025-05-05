using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        Managers.SubscribeToInit(Init);
    }

    private void Init()
    {
        Managers.Resource.LoadAllAsync<Object>("default", (key, count, total) =>
        {
            Debug.Log($"{key} {count}/{total}");  
            slider.value = count / total;

            if (count == total)
                SuccessLoad();
        });
    }

    private void SuccessLoad()
    {
        SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive); 
        SceneManager.UnloadSceneAsync("LoadingScene");
    }



    
}
