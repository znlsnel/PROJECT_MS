using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Update()
    {
        slider.value = SceneManagerEx.Instance.LoadingProgress;
    }
}
