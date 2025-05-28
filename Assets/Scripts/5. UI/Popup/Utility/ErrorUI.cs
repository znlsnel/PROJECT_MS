using UnityEngine;
using UnityEngine.UI;

public class ErrorUI : MonoBehaviour
{
    [SerializeField] private Button _quitButton;

    void Start()
    {
        _quitButton.onClick.AddListener(Managers.Instance.Quit);
    }
}
